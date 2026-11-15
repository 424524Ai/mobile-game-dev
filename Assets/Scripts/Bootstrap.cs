using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60;
        // Record a new session when the application starts.
        Telemetry.Log("session_start",
        ("app_version", Application.version),
        ("device_model", SystemInfo.deviceModel),
        ("android", SystemInfo.operatingSystem));
        DontDestroyOnLoad(gameObject);
    }
    private async Awaitable Start()
    {
        await SceneLoader.Instance.Load(
            "Menu",
            Application.exitCancellationToken
        );
    }
}
