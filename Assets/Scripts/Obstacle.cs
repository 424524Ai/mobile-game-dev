using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private ObstaclePool pool;

    [SerializeField] private float despawnDistance = 10f;

    private Transform runner;

    private Vector3 spawnForward;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            runner = player.transform;
        }
    }

    private void Update()
    {
        if (runner == null) return;

        Vector3 directionToObstacle = transform.position - runner.position;

        float forwardDistance =
            Vector3.Dot(directionToObstacle, spawnForward);

        if (forwardDistance < -despawnDistance)
        {
            Despawn();
        }
    }

    public Obstacle Init(ObstaclePool p)
    {
        pool = p;
        return this;
    }

    public void Despawn()
    {
        if (pool != null)
        {
            pool.Release(this);
        }
    }

    public void SetSpawnDirection(Vector3 direction)
    {
        spawnForward = direction.normalized;
    }
}