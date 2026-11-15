using UnityEngine;
// Manages the player's coins and upgrade progression.
public class Wallet : MonoBehaviour
{
    public int Coins { get; private set; }
    public int Level { get; private set; }

    // Tracks coins earned through gameplay, excluding purchases.
    public int GameplayCoinsEarned { get; private set; }

    public event System.Action Changed;
    public int NextCost => (int)(100 * Mathf.Pow(1.5f, Level));

    public void Earn(int n) 
    { 
        Coins += n; 
        Changed?.Invoke(); 
    }

    // Adds coins collected during gameplay.
    public void EarnFromGameplay(int amount)
    {
        if (amount <= 0)
            return;

        GameplayCoinsEarned += amount;
        Earn(amount);
    }
    public void Restore(int c, int l)
    { Coins = c; 
      Level = l; 
      Changed?.Invoke(); 
    }

    public bool TryUpgrade()
    {
        // Store the current upgrade cost before increasing the level.
        int cost = NextCost;

        if (Coins < cost)
            return false;

        Coins -= cost;
        Level++;

        // Record a successful upgrade purchase.
        Telemetry.Log("upgrade_purchased",
            ("upgrade_id", "speed"),
            ("level", Level),
            ("cost", cost),
            ("coins_left", Coins));

        Changed?.Invoke();
        return true;
    }
}
