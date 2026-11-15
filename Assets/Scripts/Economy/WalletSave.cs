
using UnityEngine;

// Saves and loads wallet data using PlayerPrefs.
public static class WalletSave
{
    private const string CoinsKey = "wallet.coins";
    private const string LevelKey = "wallet.level";

    // Saves the current coins and upgrade level.
    public static void Save(Wallet wallet)
    {
        PlayerPrefs.SetInt(CoinsKey, wallet.Coins);
        PlayerPrefs.SetInt(LevelKey, wallet.Level);
        PlayerPrefs.Save();
    }

    // Restores the saved wallet data.
    public static void Load(Wallet wallet)
    {
        wallet.Restore(
            PlayerPrefs.GetInt(CoinsKey, 0),
            PlayerPrefs.GetInt(LevelKey, 0)
        );
    }
}
