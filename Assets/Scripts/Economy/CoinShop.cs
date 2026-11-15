
using UnityEngine;

// Handles simulated coin pack purchases.
public class CoinShop : MonoBehaviour
{
    [SerializeField] private Wallet wallet;

    private IPurchaseProvider store = new FakePurchaseProvider();

    // Called when the Buy 500 coins button is clicked.
    public async void OnBuyPack()
    {
        if (wallet == null)
        {
            wallet = FindFirstObjectByType<Wallet>();
        }

        if (wallet == null)
        {
            Debug.LogError("CoinShop: Wallet not found.");
            return;
        }

        if (await store.PurchaseAsync("coins_500"))
        {
            wallet.Earn(500);
        }
    }
}
