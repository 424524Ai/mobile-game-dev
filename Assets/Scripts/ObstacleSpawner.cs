using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private int obstacleCount = 40;  // number of obstacle spawn in game at start
    [SerializeField] private float startZ = 15f;  // where first obstacle will be spawn
    [SerializeField] private float spacing = 8f;  // how far each obstacle is from each other

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < obstacleCount; i++)
        {
            float x = 0f;
            float z = startZ + i * spacing;

            Vector3 spawnPosition = new Vector3(x, 1.5f, z);

            Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        }
    }
}
