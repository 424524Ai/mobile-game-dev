using UnityEngine;
// Manages the player's coins and upgrade progression.
public class Wallet : MonoBehaviour
{
    public int Coins { get; private set; }
    public int Level { get; private set; }
    public event System.Action Changed;
    public int NextCost => (int)(10 * Mathf.Pow(1.5f, Level));

    public void Earn(int n) 
    { 
        Coins += n; 
        Changed?.Invoke(); 
    }
    public void Restore(int c, int l)
    { Coins = c; 
      Level = l; 
      Changed?.Invoke(); 
    }

    public bool TryUpgrade()
    {
        if (Coins < NextCost) return false;
        Coins -= NextCost; 
        Level++; 
        Changed?.Invoke(); 
        return true;
    }
}
