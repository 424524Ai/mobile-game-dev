
using UnityEngine;
using TMPro;

// Displays the build version and device information on the About panel.
public class BuildStamp : MonoBehaviour
{
    [SerializeField] private TMP_Text label;

    private void Start()
    {
        if (label == null)
        {
            Debug.LogWarning("[BuildStamp] BuildLabel is not assigned.");
            return;
        }

        string buildId = Application.buildGUID;
        if (string.IsNullOrEmpty(buildId))
            buildId = "Unknown";
        else
            buildId = buildId.Substring(0, Mathf.Min(8, buildId.Length));

        float ramGB = SystemInfo.systemMemorySize / 1024f;

        Resolution resolution = Screen.currentResolution;
        double refreshRate = resolution.refreshRateRatio.value;

        label.text =
            $"Forward\n" +
            $"Version: v{Application.version}\n" +
            $"Build ID: {buildId}\n" +
            $"Device: {SystemInfo.deviceModel}\n" +
            $"Android: {SystemInfo.operatingSystem}\n" +
            $"RAM: {ramGB:0.0} GB\n" +
            $"Resolution: {Screen.width} x {Screen.height}\n" +
            $"Refresh Rate: {refreshRate:0} Hz";
    }
}
