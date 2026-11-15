// Runtime script: spins and bobs the collectable, and collects it when the player touches it.
// Keep this OUTSIDE any Editor folder (e.g. Assets/Scripts/CollectibleSpin.cs).
using UnityEngine;
using UnityEngine.Events;

public class CollectibleSpin : MonoBehaviour
{
    public float spinSpeed = 90f;     // degrees per second around Y
    public float bobHeight = 0.12f;   // metres up/down
    public float bobSpeed = 2f;
    public string playerTag = "Player";
    public int value = 1;
    public UnityEvent<int> onCollected;   // hook up score / sound / particles in the Inspector

    Vector3 startPos;

    void Start() { startPos = transform.position; }

    void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.World);
        var p = startPos; p.y += Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = p;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        onCollected?.Invoke(value);
        Destroy(gameObject);
    }
}
