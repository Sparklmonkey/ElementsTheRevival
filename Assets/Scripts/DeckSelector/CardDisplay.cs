using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField]
    private Image cardImage, elementImage, cardBack, cardType, cardTypeFrame, creatureVFrame, upgradeShine, rareIndicator;

    [SerializeField]
    private TextMeshProUGUI cardName, cardCost, cardDescription, creatureValues;
    private Card card;
    private CardDisplayDetail cardDisplayDetail;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(cardDisplayDetail == null) { return; }
        cardDisplayDetail.gameObject.SetActive(true);
        cardDisplayDetail.SetupCardView(card, false, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (cardDisplayDetail == null) { return; }
        cardDisplayDetail.gameObject.SetActive(false);
    }

    public void SetupCardView(Card cardToDisplay, CardDisplayDetail cardDisplayDetail)
    {
        card = cardToDisplay;
        this.cardDisplayDetail = cardDisplayDetail;
        cardImage.sprite = cardToDisplay.cardImage;// ImageHelper.GetCardImage(cardToDisplay.imageID);
        
        cardName.text = cardToDisplay.name;
        string backGroundString = cardToDisplay.name == "Animate Weapon" ? "Air" :
                                cardToDisplay.name == "Luciferin" || cardToDisplay.name == "Luciferase" ? "Light" :
                                cardToDisplay.element.ToString();
        cardBack.sprite = ImageHelper.GetCardBackGroundImage(backGroundString);
        cardCost.text = cardToDisplay.cost.ToString();
        elementImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        elementImage.sprite = ImageHelper.GetElementImage(cardToDisplay.element.FastElementString());
        cardDescription.text = cardToDisplay.description;
        if (cardToDisplay.cost == 0)
        {
            cardCost.text = "";
            elementImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
        }

        if (cardToDisplay.element.Equals(Element.Other))
        {
            elementImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
        }

        if (cardToDisplay.type.Equals(CardType.Creature))
        {
            cardTypeFrame.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
            cardType.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
            creatureVFrame.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            creatureValues.text = cardToDisplay.power + "/" + cardToDisplay.hp;
        }
        else
        {
            creatureVFrame.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
            creatureValues.text = "";
            cardType.sprite = ImageHelper.GetCardTypeImage(cardToDisplay.type.FastCardTypeString());
        }
        upgradeShine.color = cardToDisplay.isUpgradable ? new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0) : new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

        rareIndicator.color = !cardToDisplay.isRare ? new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue) : new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
    }


}

