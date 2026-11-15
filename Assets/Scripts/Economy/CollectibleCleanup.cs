// Removes uncollected collectibles after the runner passes them.

using UnityEngine;

public class CollectibleCleanup : MonoBehaviour
{
    [SerializeField] private float despawnDistance = 10f;

    private Transform runner;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
            runner = player.transform;
        else
            Debug.LogWarning("CollectibleCleanup: Player not found.");
    }

    private void Update()
    {
        if (runner == null)
            return;

        // Remove the collectible when it is far behind the runner.
        if (transform.position.z < runner.position.z - despawnDistance)
        {
            Destroy(gameObject);
        }
    }
}