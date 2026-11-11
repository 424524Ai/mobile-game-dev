using UnityEngine;
using System.Collections.Generic;

// Generates connected track segments and registers their corners.
public class TrackGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] trackPrefabs;
    [SerializeField] private Transform lastTrack;
    [SerializeField] private float trackLength = 60f;
    [SerializeField] private float trackWidth = 8f;
    [SerializeField] private CornerTrigger cornerPrefab;
    [SerializeField] private RunnerMovement runner;
    [SerializeField] private float generationDistance = 20f; // generate new track when the runner is this far from the last track
    [SerializeField] private int maxActiveTracks = 4;

    private readonly List<Transform> tracks = new();
    public IReadOnlyList<Transform> Tracks => tracks;

    private void Start()
    {
        tracks.Add(lastTrack);
        GenerateNextTrack();
    }

    private void Update()
    {
        if (runner == null || lastTrack == null)
            return;

        // Calculate the end position of the last generated track.
        Vector3 trackEnd =
            lastTrack.position + lastTrack.forward * (trackLength / 2f);

        // Measure how far the runner has progressed along this track.
        float remainingDistance = Vector3.Dot(
            trackEnd - runner.transform.position,
            lastTrack.forward
        );

        // Generate another track when the runner approaches the end.
        if (remainingDistance <= generationDistance)
        {
            GenerateNextTrack();
        }
    }

    private void GenerateNextTrack()
    {
        // Calculate the end of the current track.
        Vector3 cornerPosition =
            lastTrack.position + lastTrack.forward * (trackLength / 2f);

        // Randomly choose a left or right turn.
        bool turnRight = Random.value > 0.5f;
        float turnAngle = turnRight ? 90f : -90f;

        Quaternion newRotation =
            lastTrack.rotation * Quaternion.Euler(0f, turnAngle, 0f);

        Debug.Log("Generated Turn: " + (turnRight ? "Right" : "Left"));

        // Calculate the turn position.
        Vector3 turnPosition =
            cornerPosition - lastTrack.forward * (trackWidth / 2f);

        // Calculate the centre of the next track.
        Vector3 spawnPosition =
            turnPosition
            + (newRotation * Vector3.forward) * (trackLength / 2f);

        // Randomly select a track prefab.
        int randomIndex = Random.Range(0, trackPrefabs.Length);

        GameObject selectedPrefab = trackPrefabs[randomIndex];

        // Generate the selected track.
        GameObject newTrack = Instantiate(
            selectedPrefab,
            spawnPosition,
            newRotation
        );

        Debug.Log("Generated Track: " + selectedPrefab.name);

        // Create and register the corner.
        CornerTrigger newCorner = Instantiate(
            cornerPrefab,
            turnPosition,
            Quaternion.identity
        );

        newCorner.SetDirection(
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

    private void CleanupOldTracks()
    {
        while (tracks.Count > maxActiveTracks)
        {
            Transform oldestTrack = tracks[0];

            tracks.RemoveAt(0);

            if (oldestTrack != null)
            {
                Destroy(oldestTrack.gameObject);
            }
        }
    }
}
