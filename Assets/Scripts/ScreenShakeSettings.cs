
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ScreenShakeSettings : MonoBehaviour
{
    private const string Key = "screenShake";
    private Toggle toggle;

    void Awake()
    {
        toggle = GetComponent<Toggle>();

        // Load the saved setting. Enabled by default.
        bool enabled = PlayerPrefs.GetInt(Key, 1) == 1;
        toggle.SetIsOnWithoutNotify(enabled);

        toggle.onValueChanged.AddListener(OnValueChanged);
    }

    void OnValueChanged(bool enabled)
    {
        PlayerPrefs.SetInt(Key, enabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    void OnDestroy()
    {
        if (toggle != null)
            toggle.onValueChanged.RemoveListener(OnValueChanged);
    }
}
