using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private int obstacleCount = 40;  // number of obstacle spawn in game at start
    [SerializeField] private float startZ = 15f;  // where first obstacle will be spawn
    [SerializeField] private float spacing = 8f;  // how far each obstacle is from each other
    [SerializeField] private float maxX = 7f;  // make sure the obstacle doesn't spawn on the edge of floor

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < obstacleCount; i++)
        {
            float x = Random.Range(-maxX, maxX);
            float z = startZ + i * spacing;

            Vector3 spawnPosition = new Vector3(x, 1.5f, z);

            Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        }
    }
}
