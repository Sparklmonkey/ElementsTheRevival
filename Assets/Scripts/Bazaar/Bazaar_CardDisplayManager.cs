using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bazaar_CardDisplayManager : MonoBehaviour
{
    [SerializeField]
    private CardDisplay cardDisplay;
    [SerializeField]
    private TextMeshProUGUI cardPrice;
    public void SetupCardDisplay(Card cardToDisplay, bool isBuy)
    {
        cardDisplay.SetupCardView(cardToDisplay, null);
        if (isBuy)
        {
            cardPrice.text = cardToDisplay.buyPrice.ToString();
            return;
        }
        cardPrice.text = cardToDisplay.sellPrice.ToString();
    }
}
