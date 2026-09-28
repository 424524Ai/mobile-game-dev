using UnityEngine;
using UnityEngine.UI;

public class HapticsToggle : MonoBehaviour
{
    private Toggle toggle;

    void Awake()
    {
        toggle = GetComponent<Toggle>();

        // get previous toggle setting
        toggle.SetIsOnWithoutNotify(Haptics.Enabled);

        // update toggle setting
        toggle.onValueChanged.AddListener(OnHapticsChange);
    }

    void OnHapticsChange(bool value)
    {
        Haptics.Enabled = value;
    }

    public void TestVibration()
    {
        Haptics.Pulse();
    }
}
