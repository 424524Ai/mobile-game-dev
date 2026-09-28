using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject panel;
    [SerializeField] LifecycleGuard guard;

    void OnEnable() { LifecycleGuard.PausedChanged += Show; Show(LifecycleGuard.IsPaused); }
    void OnDisable() { LifecycleGuard.PausedChanged -= Show; }
    void Show(bool paused) {
        panel.SetActive(paused);
        if(!paused)
            settingsPanel.SetActive(false);
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null || !kb.escapeKey.wasPressedThisFrame) return;
        guard.SetPaused(!LifecycleGuard.IsPaused);
    }

    public void OnResumePressed() => guard.SetPaused(false);

    public void OnSettingsPressed() {
        panel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnSettingsBackPressed()
    {
        settingsPanel.SetActive(false); 
        panel.SetActive(true);
    }
}
