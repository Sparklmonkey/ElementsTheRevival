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
        cardName.text = card.CardName;
        cardImage.sprite = ImageHelper.GetCardImage(card.ImageId);
        headerBackground.sprite = ImageHelper.GetCardHeadBackground(card.Element);

        await new WaitForSeconds(0.5f);
        container.SetActive(false);
    }
}
