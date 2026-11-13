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
    [SerializeField] private float aheadDistance = 100f;
    [SerializeField] private TrackPool trackPool;
    [SerializeField] private CornerPool cornerPool;

    // Reusable buffer for track overlap detection.
    private readonly Collider[] overlapResults = new Collider[32];
    [SerializeField] private int prewarmCountPerPrefab = 4;
    [SerializeField] private int prewarmCornerCount = 4;

    private readonly List<Transform> tracks = new();
    public IReadOnlyList<Transform> Tracks => tracks;
    private Transform initialTrack;

    // Index of the next track prefab to generate.
    private int nextTrackIndex = 1;

    private void Start()
    {
        // Pre-create tracks before gameplay starts.
        trackPool.Prewarm(trackPrefabs, prewarmCountPerPrefab);

        initialTrack = lastTrack;
        tracks.Add(lastTrack);

        // Generate enough tracks before gameplay begins.
        GenerateTracksAhead();
    }

    private void Update()
    {
        if (LifecycleGuard.IsPaused || runner == null || lastTrack == null)
            return;

        GenerateTracksAhead();
    }

    private void GenerateTracksAhead()
    {
        if (runner == null || lastTrack == null)
            return;

        // Calculate the end of the last generated track.
        Vector3 trackEnd =
            lastTrack.position + lastTrack.forward * (trackLength / 2f);

        // Calculate how far the generated track extends ahead of the runner.
        float distanceAhead = Vector3.Dot(
            trackEnd - runner.transform.position,
            lastTrack.forward
        );

        // Keep generating until enough track exists ahead.
        while (distanceAhead < aheadDistance)
        {
            GenerateNextTrack();

            distanceAhead += trackLength;
        }
    }

    private void GenerateNextTrack()
    {
        using (generateTrackMarker.Auto())
        {
            if (lastTrack == null || trackPrefabs.Length == 0)
                return;

            // Keep the same direction as the previous track.
            Quaternion newRotation = lastTrack.rotation;

            // Position the new track directly after the previous one.
            Vector3 spawnPosition =
                lastTrack.position +
                lastTrack.forward * trackLength;

            // Alternate between track prefabs.
            GameObject selectedPrefab = trackPrefabs[nextTrackIndex];
            nextTrackIndex = (nextTrackIndex + 1) % trackPrefabs.Length;

            // Reuse a track from the pool.
            GameObject newTrack = trackPool.GetTrack(
                selectedPrefab,
                spawnPosition,
                newRotation
            );

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
