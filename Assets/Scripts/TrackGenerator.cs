using UnityEngine;
using System.Collections.Generic;
using Unity.Profiling;

// Generates and recycles straight track segments using object pooling.
public class TrackGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] trackPrefabs;
    [SerializeField] private RunnerMovement runner;
    [SerializeField] private TrackPool trackPool;
    [SerializeField] private int maxActiveTracks = 8;
    [SerializeField] private float aheadDistance = 100f;
    [SerializeField] private float trackLength = 20f;
    [SerializeField] private int prewarmCountPerPrefab = 4;
    [SerializeField] private Vector3 firstTrackPosition = Vector3.zero;

    private readonly List<Transform> tracks = new();
    public IReadOnlyList<Transform> Tracks => tracks;

    private Transform lastTrack;

    // Index of the next track prefab to generate.
    private int nextTrackIndex = 1;

    private void Start()
    {
        // Pre-create tracks before gameplay starts.
        trackPool.Prewarm(trackPrefabs, prewarmCountPerPrefab);

        // Generate the first track.
        GameObject firstTrack = trackPool.GetTrack(
            trackPrefabs[0],
            firstTrackPosition,
            Quaternion.identity
        );

        lastTrack = firstTrack.transform;
        tracks.Add(lastTrack);

        // The next track should be SchoolWalkwayB.
        nextTrackIndex = 1;

        // Generate additional tracks ahead of the runner.
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

    private void CleanupOldTracks()
    {
        while (tracks.Count > maxActiveTracks)
        {
            Transform oldestTrack = tracks[0];
            tracks.RemoveAt(0);

            if (oldestTrack == null)
                continue;

            // All tracks now belong to the pool.
            trackPool.ReturnTrack(oldestTrack.gameObject);
        }
    }

    // Measures track generation performance.
    private static readonly ProfilerMarker generateTrackMarker =
        new ProfilerMarker("TrackGenerator.GenerateNextTrack");
}
