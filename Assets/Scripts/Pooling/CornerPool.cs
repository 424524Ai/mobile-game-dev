
using System.Collections.Generic;
using UnityEngine;

// Manages reusable corner triggers.
public class CornerPool : MonoBehaviour
{
    [SerializeField] private CornerTrigger cornerPrefab;

    private readonly Queue<CornerTrigger> availableCorners = new();
    private readonly HashSet<CornerTrigger> ownedCorners = new();

    // Pre-creates corner triggers for reuse.
    public void Prewarm(int count)
    {
        for (int i = 0; i < count; i++)
        {
            CornerTrigger corner = Instantiate(cornerPrefab, transform);

            corner.gameObject.SetActive(false);

            ownedCorners.Add(corner);
            availableCorners.Enqueue(corner);
        }
    }

    public CornerTrigger GetCorner(
        Vector3 position,
        CornerTrigger.TurnDirection direction)
    {
        CornerTrigger corner;

        if (availableCorners.Count > 0)
        {
            corner = availableCorners.Dequeue();
        }
        else
        {
            corner = Instantiate(cornerPrefab);
            ownedCorners.Add(corner);
        }

        corner.transform.SetPositionAndRotation(
            position,
            Quaternion.identity
        );

        corner.SetDirection(direction);
        corner.gameObject.SetActive(true);

        return corner;
    }

    public void ReturnCorner(CornerTrigger corner)
    {
        if (corner == null)
            return;

        // Initial scene corners are not owned by this pool.
        if (!ownedCorners.Contains(corner))
        {
            Destroy(corner.gameObject);
            return;
        }

        corner.gameObject.SetActive(false);
        availableCorners.Enqueue(corner);
    }
}
