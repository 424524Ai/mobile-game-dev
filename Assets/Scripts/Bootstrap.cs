using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    void Awake()
    {
        // Get the first eight characters of the build GUID.
        string buildId = Application.buildGUID;
        if (string.IsNullOrEmpty(buildId))
            buildId = "Unknown";
        else
            buildId = buildId.Substring(0, Mathf.Min(8, buildId.Length));

        // Record the selected quality tier at session start.
        int tierIndex = QualitySettings.GetQualityLevel();
        string tierName = QualitySettings.names[tierIndex];

        // Record a new session when the application starts.
        Telemetry.Log("session_start",
        ("app_version", Application.version),
        ("device_model", SystemInfo.deviceModel),
        ("android", SystemInfo.operatingSystem),
        ("tier", tierName));
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
