
using UnityEngine;

// Logs when the Menu scene reaches its first Update.
public class MenuReady : MonoBehaviour
{
    private bool hasLogged;

    private void Update()
    {
        if (hasLogged)
            return;

        hasLogged = true;
        Debug.Log($"[perf] interactive {Time.realtimeSinceStartup:F2}s");
    }
}
