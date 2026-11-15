using System.Collections.Generic;
using UnityEngine;

// Manages reusable instances of different obstacle prefabs.
public class ObstaclePool : MonoBehaviour
{
    // Obstacle prefabs that can be spawned by this pool.
    [SerializeField] private Obstacle[] obstaclePrefabs;

    // Number of instances to create for each prefab at startup.
    [SerializeField] private int prewarmCountPerPrefab = 4;

    // Stores available instances separately for each obstacle prefab.
    private readonly Dictionary<Obstacle, Queue<Obstacle>> pools = new();
    // Tracks which prefab each obstacle instance belongs to.
    private readonly Dictionary<Obstacle, Obstacle> prefabLookup = new();

    private void Awake()
    {
        // Create a separate pool for each obstacle type.
        foreach (Obstacle prefab in obstaclePrefabs)
        {
            if (prefab == null) continue;

            Queue<Obstacle> pool = new Queue<Obstacle>();
            pools.Add(prefab, pool);

            // Pre-create obstacle instances to reduce runtime instantiation.
            for (int i = 0; i < prewarmCountPerPrefab; i++)
            {
                Obstacle obstacle = Create(prefab);
                Release(obstacle);
            }
        }
    }

    // Creates a new obstacle instance and registers it with the pool.
    private Obstacle Create(Obstacle prefab)
    {
        Obstacle obstacle = Instantiate(prefab, transform);
        // Allow the obstacle to return itself to this pool.
        obstacle.Init(this);
        // Remember the original prefab for correct recycling.
        prefabLookup.Add(obstacle, prefab);

        return obstacle;
    }

    // Retrieves an available obstacle or creates one if the pool is empty.
    public Obstacle Spawn(Obstacle prefab, Vector3 position)
    {
        if (!pools.TryGetValue(prefab, out Queue<Obstacle> pool))
        {
            Debug.LogError("Obstacle prefab is not registered in the pool.");
            return null;
        }

        // Reuse an existing instance whenever possible.
        Obstacle obstacle = pool.Count > 0
            ? pool.Dequeue()
            : Create(prefab);

        // Position and activate the obstacle.
        obstacle.transform.position = position;
        obstacle.gameObject.SetActive(true);

        return obstacle;
    }

    // Deactivates an obstacle and returns it to its original prefab pool.
    public void Release(Obstacle obstacle)
    {
        if (!prefabLookup.TryGetValue(obstacle, out Obstacle prefab))
        {
            Debug.LogWarning("Obstacle does not belong to this pool.");
            return;
        }
        obstacle.gameObject.SetActive(false);
        pools[prefab].Enqueue(obstacle);
    }
}