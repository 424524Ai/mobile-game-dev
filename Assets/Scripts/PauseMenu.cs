using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject panel;
    [SerializeField] LifecycleGuard guard;
    [SerializeField] private GameObject gameplayHUD;
    [SerializeField] private GameObject aboutPanel;

    private bool returningToMenu;
    void OnEnable() { LifecycleGuard.PausedChanged += Show; Show(LifecycleGuard.IsPaused); }
    void OnDisable() { LifecycleGuard.PausedChanged -= Show; }
    // Updates the pause menu and gameplay HUD visibility.
    void Show(bool paused)
    {
        bool isAboutOpen = aboutPanel != null && aboutPanel.activeSelf;
        bool isSettingsOpen = settingsPanel != null && settingsPanel.activeSelf;

        // Only show the Pause panel if no sub-menu is open.
        panel.SetActive(paused && !isAboutOpen && !isSettingsOpen);

        if (gameplayHUD != null)
            gameplayHUD.SetActive(!paused);

        if (!paused)
        {
            settingsPanel.SetActive(false);

            if (aboutPanel != null)
                aboutPanel.SetActive(false);
        }
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null || !kb.escapeKey.wasPressedThisFrame) return;
        guard.SetPaused(!LifecycleGuard.IsPaused);
    }

    // Pauses the game when the HUD pause button is pressed.
    public void OnPausePressed() => guard.SetPaused(true);

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

    // Returns to the main menu after clearing the pause state.
    public async void OnMainMenuPressed()
    {
        // Prevent repeated requests while the scene is loading.
        if (returningToMenu)
            return;

        returningToMenu = true;

        // Restore time, audio, and the shared pause state.
        guard.SetPaused(false);

        // Load the main menu using the existing scene loader.
        if (SceneLoader.Instance != null)
        {
            await SceneLoader.Instance.Load("Menu", CancellationToken.None);
        }
        else
        {
            Debug.LogError("SceneLoader instance was not found.");
            returningToMenu = false;
        }
    }
}
