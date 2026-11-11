
using System.Collections.Generic;
using UnityEngine;

// Manages reusable track instances for each track prefab.
public class TrackPool : MonoBehaviour
{
    private readonly Dictionary<GameObject, Queue<GameObject>> pools = new();
    private readonly Dictionary<GameObject, GameObject> prefabLookup = new();

    public GameObject GetTrack(
        GameObject prefab,
        Vector3 position,
        Quaternion rotation)
    {
        if (!pools.TryGetValue(prefab, out Queue<GameObject> pool))
        {
            pool = new Queue<GameObject>();
            pools.Add(prefab, pool);
        }

        GameObject track;

        if (pool.Count > 0)
        {
            track = pool.Dequeue();
        }
        else
        {
            track = Instantiate(prefab);
            prefabLookup.Add(track, prefab);
        }

        track.transform.SetPositionAndRotation(position, rotation);
        track.SetActive(true);

        return track;
    }

    public void ReturnTrack(GameObject track)
    {
        if (track == null)
            return;

        if (!prefabLookup.TryGetValue(track, out GameObject prefab))
        {
            Debug.LogWarning("Track does not belong to this pool.");
            return;
        }

        track.SetActive(false);
        pools[prefab].Enqueue(track);
    }
}
