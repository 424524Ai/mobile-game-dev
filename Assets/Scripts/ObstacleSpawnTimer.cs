using System;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawnTimer : MonoBehaviour
{
    [SerializeField] private ObstaclePool obstaclePool;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private Transform runner;
    [SerializeField] private float obstacleHeight = 1.5f;
    [SerializeField] private TrackGenerator trackGenerator;
    [SerializeField] private float spawnRange = 20f;

    [SerializeField] private float minSpacing = 6f;

    private CancellationTokenSource cts;

    private void OnEnable()
    {
        cts = CancellationTokenSource.CreateLinkedTokenSource(
            Application.exitCancellationToken
        );

        _ = SpawnLoop(cts.Token);
    }

    private async Awaitable SpawnLoop(CancellationToken ct)
    {
        try
        {
            while (!ct.IsCancellationRequested)
            {
                await Awaitable.WaitForSecondsAsync(
                    spawnInterval,
                    ct
                );

                ct.ThrowIfCancellationRequested();

                if (runner == null) return;

                IReadOnlyList<Transform> tracks = trackGenerator.Tracks;

                if (tracks.Count == 0) return;

                // Find the track closest to the runner.
                int currentTrackIndex = 0;
                float closestDistance = float.MaxValue;

                for (int i = 0; i < tracks.Count; i++)
                {
                    float distance = Vector3.Distance(
                        runner.position,
                        tracks[i].position
                    );

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        currentTrackIndex = i;
                    }
                }

                // Choose the current or next track.
                int selectedIndex = Mathf.Min(
                    currentTrackIndex + 1,
                    tracks.Count - 1
                );

                Transform selectedTrack = tracks[selectedIndex];

                // Choose a random position along the track.
                Vector3 spawnPosition = Vector3.zero;
                bool validPosition = false;
                for (int attempt = 0; attempt < 10; attempt++)
                {
                    float offset = UnityEngine.Random.Range(-spawnRange, spawnRange);

                    spawnPosition =
                        selectedTrack.position + selectedTrack.forward * offset;

                    spawnPosition.y = obstacleHeight;

                    validPosition = true;
                    foreach (Transform child in obstaclePool.transform)
                    {
                        if (!child.gameObject.activeSelf)
                        {
                            continue;
                        }

                        if (Vector3.Distance(spawnPosition, child.position) < minSpacing)
                        {
                            validPosition = false;
                            break;
                        }
                    }
                    if (validPosition)
                    {
                        break;
                    }
                }
                if (!validPosition)
                {
                    continue;
                }

                Obstacle obstacle = obstaclePool.Spawn(spawnPosition);
                obstacle.transform.rotation = selectedTrack.rotation;
                obstacle.SetSpawnDirection(selectedTrack.forward);

                Debug.Log("Obstacle spawned from pool");
            }
        }
        catch (OperationCanceledException)
        {
            // Expected when the timer is cancelled.
        }
    }

    private void OnDisable()
    {
        cts?.Cancel();
        cts?.Dispose();
        cts = null;
    }
}