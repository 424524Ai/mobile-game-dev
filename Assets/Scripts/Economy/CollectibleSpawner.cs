// Spawns a row of five collectibles in a selected lane.

using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject collectiblePrefab;

    [Header("Spawn Settings")]
    [SerializeField] private int collectiblesPerGroup = 5;
    [SerializeField] private float collectibleSpacing = 2f;
    [SerializeField] private float laneWidth = 2.4f;
    [SerializeField] private float spawnHeight = 0.5f;

    // Spawns a group of collectibles in one lane.
    // Lane: 0 = Left, 1 = Middle, 2 = Right.
    public void SpawnGroup(int lane, float startZ)
    {
        if (collectiblePrefab == null)
        {
            Debug.LogWarning("CollectibleSpawner: Prefab is missing.");
            return;
        }

        // Try the selected lane first, then the other two lanes.
        bool foundSafeLane = false;

        for (int attempt = 0; attempt < 3; attempt++)
        {
            int candidateLane = (lane + attempt) % 3;

            if (!IsLaneSafe(candidateLane, startZ))
                continue;

            lane = candidateLane;
            foundSafeLane = true;
            break;
        }

        if (!foundSafeLane)
        {
            Debug.Log("CollectibleSpawner: No safe lane found.");
            return;
        }

        float spawnX = (lane - 1) * laneWidth;

        for (int i = 0; i < collectiblesPerGroup; i++)
        {
            Vector3 spawnPosition = new Vector3(
                spawnX,
                spawnHeight,
                startZ + i * collectibleSpacing
            );

            Instantiate(
                collectiblePrefab,
                spawnPosition,
                Quaternion.identity
            );
        }
    }

    // Checks whether all collectible positions are clear of blocking obstacles.
    private bool IsLaneSafe(int lane, float startZ)
    {
        float x = (lane - 1) * laneWidth;

        for (int i = 0; i < collectiblesPerGroup; i++)
        {
            Vector3 position = new Vector3(
                x,
                spawnHeight,
                startZ + i * collectibleSpacing
            );

            Collider[] hits = Physics.OverlapBox(
                position,
                new Vector3(0.6f, 0.6f, 1f),
                Quaternion.identity,
                ~0,
                QueryTriggerInteraction.Collide
            );

            foreach (Collider hit in hits)
            {
                Obstacle obstacle = hit.GetComponentInParent<Obstacle>();

                if (obstacle == null)
                    continue;

                // ExamBanner is a roll obstacle and does not block collectibles.
                if (obstacle.GetComponentInChildren<ExamBannerMarker>() != null)
                    continue;

                return false;
            }
        }

        return true;
    }
}