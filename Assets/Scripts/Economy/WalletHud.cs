// Displays wallet information and handles upgrade button clicks.

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WalletHud : MonoBehaviour
{
    [SerializeField] private Wallet wallet;
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text upgradeText;
    [SerializeField] private Button upgradeButton;

    private void OnEnable()
    {
        if (wallet == null)
            wallet = FindFirstObjectByType<Wallet>();

        if (wallet == null)
        {
            Debug.LogWarning("WalletHud: Wallet not found.");
            return;
        }

        wallet.Changed += Refresh;
        upgradeButton.onClick.AddListener(OnUpgradeClicked);

        Refresh();
    }

    private void OnDisable()
    {
        if (wallet != null)
            wallet.Changed -= Refresh;

        if (upgradeButton != null)
            upgradeButton.onClick.RemoveListener(OnUpgradeClicked);
    }

    // Updates the HUD when wallet data changes.
    private void Refresh()
    {
        coinsText.text = $"Coins: {wallet.Coins}";
        upgradeText.text = $"Upgrade Lv.{wallet.Level} - {wallet.NextCost} Coins";
        upgradeButton.interactable = wallet.Coins >= wallet.NextCost;
    }

    // Attempts to purchase the next upgrade.
    private void OnUpgradeClicked()
    {
        wallet.TryUpgrade();
    }
}