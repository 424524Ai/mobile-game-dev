using UnityEngine;
// Defines the interface for handling in-app purchases.
public interface IPurchaseProvider     // real store SDK sits behind
{ Awaitable<bool> PurchaseAsync(string productId); }