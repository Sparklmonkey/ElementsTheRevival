using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardPlayedDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private Image cardImage, headerBackground;
    [SerializeField] private GameObject container;

    public async Task ShowCardPlayed(CardData card)
    {
        container.SetActive(true);
        cardName.text = card.cardName;
        cardImage.sprite = ImageHelper.GetCardImage(card.imageId);
        headerBackground.sprite = ImageHelper.GetCardHeadBackground(card.element);

        await new WaitForSeconds(0.5f);
        container.SetActive(false);
    }
}
