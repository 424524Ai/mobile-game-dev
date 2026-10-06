using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Halve the render scale. Frame time drops a lot: GPU-bound.
// Frame time barely moves: CPU-bound. Now you know where to look.

public class RenderScaleProbe : MonoBehaviour
{
    [Range(0.3f, 1f)] public float lowScale = 0.5f;

    // reference to the probe button in the scene
    [SerializeField] GameObject probeButton;

    UniversalRenderPipelineAsset _urp;
    float _full;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Only show the Probe button in a development build
        probeButton.SetActive(Debug.isDebugBuild);

        _urp = GraphicsSettings.currentRenderPipeline
            as UniversalRenderPipelineAsset;
        _full = _urp.renderScale;
    }

    public void Toggle()                // bind to a debug button
    {
        bool atFull = Mathf.Approximately(_urp.renderScale, _full);
        _urp.renderScale = atFull ? lowScale : _full;
        Debug.Log($"[Probe] renderScale = {_urp.renderScale}");
    }
}
