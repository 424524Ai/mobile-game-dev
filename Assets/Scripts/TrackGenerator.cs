using UnityEngine;
using System.Collections.Generic;
using Unity.Profiling;

// Generates connected track segments and registers their corners.
public class TrackGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] trackPrefabs;
    [SerializeField] private Transform lastTrack;
    [SerializeField] private float trackLength = 60f;
    [SerializeField] private float trackWidth = 5f;
    [SerializeField] private CornerTrigger cornerPrefab;
    [SerializeField] private RunnerMovement runner;
    [SerializeField] private float generationDistance = 20f; // generate new track when the runner is this far from the last track
    [SerializeField] private float minimumProgress = 10f;
    [SerializeField] private int maxActiveTracks = 4;
    [SerializeField] private TrackPool trackPool;
    [SerializeField] private CornerPool cornerPool;

    // Reusable buffer for track overlap detection.
    private readonly Collider[] overlapResults = new Collider[32];
    [SerializeField] private int prewarmCountPerPrefab = 4;
    [SerializeField] private int prewarmCornerCount = 4;

    private readonly List<Transform> tracks = new();
    public IReadOnlyList<Transform> Tracks => tracks;
    private Transform initialTrack;

    private void Start()
    {
        // Pre-create tracks before gameplay starts.
        trackPool.Prewarm(trackPrefabs, prewarmCountPerPrefab);
        cornerPool.Prewarm(prewarmCornerCount);

        initialTrack = lastTrack;
        tracks.Add(lastTrack);

        GenerateNextTrack();
    }

    private void Update()
    {
        if (runner == null || lastTrack == null)
            return;

        // Calculate the starting point of the last generated track.
        Vector3 trackStart =
            lastTrack.position - lastTrack.forward * (trackLength / 2f);

        // Measure the runner's progress along the last track.
        float progress = Vector3.Dot(
            runner.transform.position - trackStart,
            lastTrack.forward
        );

        // Wait until the runner has entered the last track.
        if (progress < minimumProgress)
            return;

        // Calculate the remaining distance to the end.
        float remainingDistance = trackLength - progress;

        // Generate the next track when approaching the end.
        if (remainingDistance <= generationDistance)
        {
            GenerateNextTrack();
        }
    }

    private void GenerateNextTrack()
    {
        using (generateTrackMarker.Auto())
        {
            // Calculate the end of the current track.
            Vector3 cornerPosition =
            lastTrack.position + lastTrack.forward * (trackLength / 2f);

            // Randomly choose a left or right turn.
            bool turnRight = Random.value > 0.5f;
            float turnAngle = turnRight ? 90f : -90f;

            Quaternion newRotation =
                lastTrack.rotation * Quaternion.Euler(0f, turnAngle, 0f);


            // Calculate the turn position.
            Vector3 turnPosition =
                cornerPosition - lastTrack.forward * (trackWidth / 2f);

            // Calculate the centre of the next track.
            Vector3 spawnPosition =
                turnPosition
                + (newRotation * Vector3.forward) * (trackLength / 2f);


            // Try the opposite direction if the selected track overlaps.
            if (IsTrackOverlapping(spawnPosition, newRotation))
            {
                Debug.LogWarning("Track overlap detected. Trying opposite direction.");

                turnRight = !turnRight;
                turnAngle = turnRight ? 90f : -90f;

                // Recalculate rotation using the opposite direction.
                newRotation =
                    lastTrack.rotation * Quaternion.Euler(0f, turnAngle, 0f);

                // Recalculate the next track position.
                spawnPosition =
                    turnPosition
                    + (newRotation * Vector3.forward) * (trackLength / 2f);
            }

            Debug.Log("Generated Turn: " + (turnRight ? "Right" : "Left"));

            // Randomly select a track prefab.
            int randomIndex = Random.Range(0, trackPrefabs.Length);

            GameObject selectedPrefab = trackPrefabs[randomIndex];

            // Get a track from the pool.
            GameObject newTrack = trackPool.GetTrack(
                selectedPrefab,
                spawnPosition,
                newRotation
            );

            Debug.Log("Generated Track: " + selectedPrefab.name);

            // Get a corner trigger from the pool.
            CornerTrigger newCorner = cornerPool.GetCorner(
                turnPosition,
                turnRight
                    ? CornerTrigger.TurnDirection.Right
                    : CornerTrigger.TurnDirection.Left
            );

            runner.RegisterCorner(newCorner);

            // Update the track list and generation reference.
            tracks.Add(newTrack.transform);
            lastTrack = newTrack.transform;

            CleanupOldTracks();
        }
    }

    private bool IsTrackOverlapping(Vector3 position, Quaternion rotation)
    {
        // Check the central area of the candidate track.
        Vector3 halfExtents = new Vector3(
            trackWidth / 2f - 0.2f,
            0.5f,
            trackLength / 2f - trackWidth
        );

        int hitCount = Physics.OverlapBoxNonAlloc(
            position,
            halfExtents,
            overlapResults,
            rotation
        );

        for (int i = 0; i < hitCount; i++)
        {
            if (overlapResults[i].CompareTag("Track"))
            {
                return true;
            }
        }

        return false;
    }


    private void CleanupOldTracks()
    {
        while (tracks.Count > maxActiveTracks)
        {
            Transform oldestTrack = tracks[0];
            tracks.RemoveAt(0);

            if (oldestTrack == null)
                continue;

            if (oldestTrack == initialTrack)
            {
                // The initial track was not created by the pool.
                Destroy(oldestTrack.gameObject);
            }
            else
            {
                // Return generated tracks to the pool for reuse.
                trackPool.ReturnTrack(oldestTrack.gameObject);
            }
        }
    }

    // Returns a completed corner trigger to the pool.
    public void ReturnCorner(CornerTrigger corner)
    {
        if (cornerPool != null)
        {
            cornerPool.ReturnCorner(corner);
        }
    }

    // Measures track generation performance.
    private static readonly ProfilerMarker generateTrackMarker =
        new ProfilerMarker("TrackGenerator.GenerateNextTrack");
}
