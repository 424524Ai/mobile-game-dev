// Connects collectible rewards to the player's wallet.

using UnityEngine;

public class CollectibleWalletReward : MonoBehaviour
{
    private Wallet wallet;

    private void Awake()
    {
        wallet = FindFirstObjectByType<Wallet>();
    }

    // Adds collected coins to the wallet.
    public void AwardCoins(int amount)
    {
        if (wallet == null)
        {
            Debug.LogWarning("CollectibleWalletReward: Wallet not found.");
            return;
        }

        wallet.EarnFromGameplay(amount);
    }
}