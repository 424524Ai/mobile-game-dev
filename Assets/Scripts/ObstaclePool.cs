using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool : MonoBehaviour
{
    [SerializeField] private Obstacle obstaclePrefab;
    [SerializeField] private int size = 40;

    private readonly Stack<Obstacle> available = new();

    private void Awake()
    {
        // Prewarm the pool
        for (int i = 0; i < size; i++)
        {
            Obstacle obstacle = Create();
            Release(obstacle);
        }
    }

    private Obstacle Create()
    {
        Obstacle obstacle = Instantiate(obstaclePrefab, transform);
        obstacle.Init(this);
        return obstacle;
    }

    public Obstacle Spawn(Vector3 position)
    {
        Obstacle obstacle = available.Count > 0
            ? available.Pop()
            : Create();

        obstacle.transform.position = position;
        obstacle.gameObject.SetActive(true);

        return obstacle;
    }

    public void Release(Obstacle obstacle)
    {
        obstacle.gameObject.SetActive(false);
        available.Push(obstacle);
    }
}