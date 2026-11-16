
using UnityEngine;
using UnityEngine.InputSystem;

// Controls navigation between the Settings and About panels.
public class AboutPanelController : MonoBehaviour
{
    [SerializeField] private GameObject aboutPanel;
    [SerializeField] private GameObject settingsPanel;

    public void OpenAbout()
    {
        if (aboutPanel == null || settingsPanel == null)
            return;

        settingsPanel.SetActive(false);
        aboutPanel.SetActive(true);
    }

    public void CloseAbout()
    {
        if (aboutPanel == null || settingsPanel == null)
            return;

        aboutPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    private void Update()
    {
        if (aboutPanel != null &&
            aboutPanel.activeSelf &&
            Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CloseAbout();
        }
    }
}
