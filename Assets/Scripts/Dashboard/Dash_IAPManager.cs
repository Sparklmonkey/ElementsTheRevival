using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class Dash_IAPManager : MonoBehaviour
{
    [SerializeField]
    private Error_Animated error_Animated;

    public void OnPurchaseComplete(Product product)
    {
        error_Animated.DisplayAnimatedError("Thank you so much for your support!!");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        error_Animated.DisplayAnimatedError("Something went wrong. You were not charged.");
    }

    public void OpenSparkyDevDiscord()
    {
        Application.OpenURL("https://discord.gg/A4vuKHZA");
    }

    public void OpenOetgDevDiscord()
    {
        Application.OpenURL("https://discord.gg/qAmfB8T");
    }

    public void OpenEmail()
    {
        Application.OpenURL("https://discord.gg/A4vuKHZA");
    }
}
