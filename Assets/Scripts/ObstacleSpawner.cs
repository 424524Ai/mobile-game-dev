using UnityEngine;
// Generates four obstacle chunk layouts ahead of the runner using object pooling.
public class ObstacleSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ObstaclePool obstaclePool;
    [SerializeField] private Transform runner;
    [SerializeField] private CollectibleSpawner collectibleSpawner;

    [Header("Obstacle Prefabs")]
    [SerializeField] private Obstacle homeworkPrefab;
    [SerializeField] private Obstacle examBannerPrefab;
    [SerializeField] private Obstacle teacherPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float startDistance = 30f;
    [SerializeField] private float spawnAheadDistance = 100f;
    [SerializeField] private float spawnSpacing = 20f;
    [SerializeField] private float laneWidth = 2.4f;

    // World Z position of the next obstacle to spawn.
    private float nextSpawnZ;
    private float firstObstacleZ;
    void Start()
    {
        if (runner == null || obstaclePool == null)
        {
            Debug.LogError("ObstacleSpawner references are missing.");
            enabled = false;
            return;
        }

        // Start spawning ahead of the runner's initial position.
        nextSpawnZ = runner.position.z + startDistance;
        firstObstacleZ = nextSpawnZ;
        SpawnObstaclesAhead();
    }

    private void Update()
    {
        // Maintain obstacles ahead as the runner moves forward.
        SpawnObstaclesAhead();
    }

    // Generates obstacles until the required forward distance is covered.
    private void SpawnObstaclesAhead()
    {
        float spawnLimitZ = runner.position.z + spawnAheadDistance;

        while (nextSpawnZ <= spawnLimitZ)
        {
            // Randomly select one of the four chunk types.
            int chunkType = Random.Range(0, 4);

            switch (chunkType)
            {
                case 0:
                    SpawnDoubleObstacle(nextSpawnZ);
                    break;
                case 1:
                    SpawnStaggeredObstacles(nextSpawnZ);
                    break;
                case 2:
                    SpawnObstacleInLane(examBannerPrefab, 1, nextSpawnZ);
                    break;
                case 3:
                    SpawnObstacleInLane(
                        homeworkPrefab,
                        Random.Range(0, 3),
                        nextSpawnZ
                    );
                    break;
            }

            // Both surrounding obstacle groups now exist.
            // Fill the gap behind the newly spawned group.
            if (collectibleSpawner != null && nextSpawnZ > firstObstacleZ)
            {
                int collectibleLane = Random.Range(0, 3);
                float collectibleStartZ = nextSpawnZ - spawnSpacing + 7f;

                collectibleSpawner.SpawnGroup(
                    collectibleLane,
                    collectibleStartZ
                );
            }

            nextSpawnZ += spawnSpacing;
        }
    }

    // Spawns two obstacles in different lanes at the same Z position.
    private void SpawnDoubleObstacle(float spawnZ)
    {
        // Select two different lanes (0 = Left, 1 = Middle, 2 = Right).
        int firstLane = Random.Range(0, 3);
        int secondLane = (firstLane + Random.Range(1, 3)) % 3;

        SpawnObstacleInLane(homeworkPrefab, firstLane, spawnZ);
        SpawnObstacleInLane(teacherPrefab, secondLane, spawnZ);
    }

    // Spawns an obstacle in a specified lane using the existing pool.
    private void SpawnObstacleInLane(Obstacle prefab, int lane, float spawnZ)
    {
        if (prefab == null)
        {
            Debug.LogWarning("Obstacle prefab is missing.");
            return;
        }

        float spawnX = (lane - 1) * laneWidth;
        Vector3 spawnPosition = new Vector3(spawnX, 0f, spawnZ);

        Obstacle obstacle = obstaclePool.Spawn(prefab, spawnPosition);

        if (obstacle != null)
        {
            obstacle.SetSpawnDirection(Vector3.forward);
        }
    }

    [Header("Staggered Obstacle Settings")]
    [SerializeField] private float staggeredSpacing = 5f;

    // Spawns two obstacles sequentially in adjacent lanes.
    private void SpawnStaggeredObstacles(float spawnZ)
    {
        int firstLane = Random.Range(0, 3);
        int secondLane;

        // The second obstacle must be in an adjacent lane.
        if (firstLane == 0)
        {
            secondLane = 1;
        }
        else if (firstLane == 2)
        {
            secondLane = 1;
        }
        else
        {
            secondLane = Random.value < 0.5f ? 0 : 2;
        }

        // The second obstacle is always further ahead.
        float secondSpawnZ = spawnZ + staggeredSpacing;

        SpawnObstacleInLane(homeworkPrefab, firstLane, spawnZ);
        SpawnObstacleInLane(teacherPrefab, secondLane, secondSpawnZ);
    }
}
