
using UnityEngine;
using UnityEngine.Rendering;
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

    private UniversalRenderPipelineAsset runtimeAsset;
    private UniversalRenderPipelineAsset originalAsset;
    private int activeTier;

    private void Start()
    {
        activeTier = QualitySettings.GetQualityLevel();
        originalAsset = QualitySettings.renderPipeline as UniversalRenderPipelineAsset;

        if (originalAsset == null)
        {
            Debug.LogError("PerformanceMode: No URP asset assigned to the current quality level.");
            return;
        }

        // Clone the asset so runtime changes do not affect the project asset.
        runtimeAsset = Instantiate(originalAsset);
        runtimeAsset.name = originalAsset.name + " (Runtime)";
        runtimeAsset.hideFlags = HideFlags.DontSave;

        QualitySettings.renderPipeline = runtimeAsset;

        Apply((PerfMode)PlayerPrefs.GetInt(Key, 0));
    }

    public void Apply(PerfMode mode)
    {
        bool high = mode == PerfMode.HighFps;

        int targetFps;
        float renderScale;

        // Keep low-tier devices at the lowest performance budget
        switch (activeTier)
        {
            case 0: // Low
                targetFps = 30;
                renderScale = 0.75f;
                break;

            case 1: // Mid
                targetFps = 60;
                renderScale = high ? 0.8f : 0.9f;
                break;
            
            case 2: // High
                targetFps = high ? 120 : 60;
                renderScale = high ? 0.8f : 1.0f;
                break;

            default:
                targetFps = 60;
                renderScale = 1.0f;
                break;
        }
        Application.targetFrameRate = targetFps;

        if (runtimeAsset != null)
            runtimeAsset.renderScale = renderScale;

        PlayerPrefs.SetInt(Key, (int)mode);
        PlayerPrefs.Save();

        Debug.Log($"tier={QualitySettings.names[activeTier]} " +
            $"perf_mode={mode} targetFps={targetFps} renderScale={renderScale}");
    }

    // Allows a UI Toggle to switch performance modes.
    public void SetHighFps(bool on)
    {
        Apply(on ? PerfMode.HighFps : PerfMode.Balanced);
    }

    private void OnDestroy()
    {
        // Restore the original pipeline before releasing the runtime copy.
        if (runtimeAsset == null)
            return;

        if (QualitySettings.renderPipeline == runtimeAsset)
            QualitySettings.renderPipeline = originalAsset;

        Destroy(runtimeAsset);
    }
}
