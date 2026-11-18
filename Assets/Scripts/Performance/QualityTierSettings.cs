
using TMPro;
using UnityEngine;

// Manages the quality tier dropdown and saves the user's preference.
public class QualityTierSettings : MonoBehaviour
{
    private const string OverrideKey = "tier_override";

    [SerializeField] private TMP_Dropdown qualityDropdown;

    private void Start()
    {
        if (qualityDropdown == null)
        {
            Debug.LogError("QualityTierSettings: Dropdown is not assigned.");
            return;
        }

        // Restore the saved preference when the settings UI starts.
        int savedTier = PlayerPrefs.GetInt(OverrideKey, -1);
        int option = savedTier >= 0 && savedTier <= 2
            ? savedTier + 1
            : 0;

        qualityDropdown.SetValueWithoutNotify(option);
        qualityDropdown.onValueChanged.AddListener(SetTierOverride);
    }

    // Saves the selected tier; changes take effect on the next launch.
    public void SetTierOverride(int option)
    {
        if (option == 0)
            PlayerPrefs.DeleteKey(OverrideKey);
        else
            PlayerPrefs.SetInt(OverrideKey, option - 1);

        PlayerPrefs.Save();

        Debug.Log($"quality_preference={qualityDropdown.options[option].text} (next launch)");
    }

    private void OnDestroy()
    {
        if (qualityDropdown != null)
            qualityDropdown.onValueChanged.RemoveListener(SetTierOverride);
    }
}
