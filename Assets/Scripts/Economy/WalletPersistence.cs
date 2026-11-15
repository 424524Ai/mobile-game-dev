
using UnityEngine;

// Loads wallet data and saves changes during gameplay.
[RequireComponent(typeof(Wallet))]
public class WalletPersistence : MonoBehaviour
{
    private Wallet wallet;

    private void Awake()
    {
        wallet = GetComponent<Wallet>();

        // Restore saved data before subscribing to changes.
        WalletSave.Load(wallet);
        wallet.Changed += SaveWallet;
    }

    private void OnDestroy()
    {
        if (wallet != null)
        {
            wallet.Changed -= SaveWallet;
        }
    }

    // Save when coins or upgrade levels change.
    private void SaveWallet()
    {
        WalletSave.Save(wallet);
    }

    // Save when the application enters the background.
    private void OnApplicationPause(bool paused)
    {
        if (paused && wallet != null)
        {
            WalletSave.Save(wallet);
        }
    }
}
