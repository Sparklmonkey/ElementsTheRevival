using TMPro;
using UnityEngine;

public class BazaarCardDisplayManager : MonoBehaviour
{
    [SerializeField]
    private CardDisplay cardDisplay;
    [SerializeField]
    private TextMeshProUGUI cardPrice;
    public void SetupCardDisplay(Card cardToDisplay, bool isBuy)
    {
        cardDisplay.SetupCardView(cardToDisplay);
        if (isBuy)
        {
            cardPrice.text = cardToDisplay.BuyPrice.ToString();
            return;
        }
        cardPrice.text = cardToDisplay.SellPrice.ToString();
    }
}
