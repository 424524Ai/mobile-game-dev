
using UnityEngine;
using UnityEngine.Rendering.Universal;

// Controls the target frame rate and URP render scale.
public enum PerfMode
{
    Balanced = 0,
    HighFps = 1
}

public class PerformanceMode : MonoBehaviour
{
    private const string Key = "perf_mode";

    private void Start()
    {
        Apply((PerfMode)PlayerPrefs.GetInt(Key, 0));
    }

    public void Apply(PerfMode mode)
    {
        bool high = mode == PerfMode.HighFps;

        Application.targetFrameRate = high ? 120 : 60;

        var urp = UniversalRenderPipeline.asset;
        if (urp != null)
            urp.renderScale = high ? 0.8f : 1.0f;

        PlayerPrefs.SetInt(Key, (int)mode);
        PlayerPrefs.Save();

        Debug.Log($"perf_mode={mode} high={high}");
    }

    // Allows a UI Toggle to switch performance modes.
    public void SetHighFps(bool on)
    {
        Apply(on ? PerfMode.HighFps : PerfMode.Balanced);
    }
}
