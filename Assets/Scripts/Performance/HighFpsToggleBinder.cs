
using UnityEngine;
using UnityEngine.UI;

// Connects the settings toggle to the persistent performance manager.
[RequireComponent(typeof(Toggle))]
public class HighFpsToggleBinder : MonoBehaviour
{
    private Toggle highFpsToggle;

    private void Awake()
    {
        highFpsToggle = GetComponent<Toggle>();
    }

    private void Start()
    {
        // Restore the saved setting without triggering the toggle event.
        bool highFpsEnabled = PlayerPrefs.GetInt("perf_mode", 0) == 1;
        highFpsToggle.SetIsOnWithoutNotify(highFpsEnabled);

        // Find the persistent manager created in the Boot scene.
        PerformanceMode performanceMode = FindFirstObjectByType<PerformanceMode>();

        if (performanceMode == null)
        {
            Debug.LogWarning("High FPS Toggle: PerformanceMode not found.");
            return;
        }

        // Connect changes to the performance mode manager.
        highFpsToggle.onValueChanged.AddListener(performanceMode.SetHighFps);
    }

    private void OnDestroy()
    {
        if (highFpsToggle != null)
            highFpsToggle.onValueChanged.RemoveAllListeners();
    }
}
