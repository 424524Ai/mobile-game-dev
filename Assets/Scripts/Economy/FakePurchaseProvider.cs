
using UnityEngine;

// Simulates an in-app purchase without a real payment service.
public class FakePurchaseProvider : IPurchaseProvider
{
    public async Awaitable<bool> PurchaseAsync(string productId)
    {
        Debug.Log($"[iap-stub] purchase {productId}");

        // Simulate a short purchase delay.
        await Awaitable.WaitForSecondsAsync(0.5f);

        return true;
    }
}
