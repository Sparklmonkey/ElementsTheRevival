using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplayDetail : MonoBehaviour
{

    [SerializeField]
    private Image cardImage, elementImage, cardBack, cardType, cardTypeFrame, creatureVFrame, upgradeShine;

    [SerializeField]
    private TextMeshProUGUI cardName, cardCost, cardDescription, creatureValues;
    private Card card;

    public void SetupCardView(Card cardToDisplay, bool belongsToPlayer, bool isBattlefield)
    {
        card = cardToDisplay;

        if (cardToDisplay.name.Contains("Pendulum") && isBattlefield)
        {
            cardImage.sprite = ImageHelper.GetPendulumImage(cardToDisplay.imageID, belongsToPlayer ? PlayerData.shared.markElement.FastElementString() : BattleVars.shared.enemyAiData.mark.FastElementString());
        }
        else
        {
            cardImage.sprite = cardToDisplay.cardImage;// ImageHelper.GetCardImage(cardToDisplay.imageID);
        }
        
        cardName.text = cardToDisplay.name;
        string backGroundString = cardToDisplay.name == "Animate Weapon" ? "Air" :
                                cardToDisplay.name == "Luciferin" || cardToDisplay.name == "Luciferase" ? "Light" :
                                cardToDisplay.element.ToString();
        cardBack.sprite = ImageHelper.GetCardBackGroundImage(backGroundString);
        cardCost.text = cardToDisplay.cost.ToString();
        elementImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        elementImage.sprite = ImageHelper.GetElementImage(cardToDisplay.element.FastElementString());
        cardDescription.text = cardToDisplay.description;


        if (cardToDisplay.element.Equals(Element.Aether) || cardToDisplay.element.Equals(Element.Other) || cardToDisplay.element.Equals(Element.Air) || cardToDisplay.element.Equals(Element.Light) || !cardToDisplay.isUpgradable)
        {
            cardName.color = new Color32(byte.MinValue, byte.MinValue, byte.MinValue, byte.MaxValue);
        }
        else
        {
            cardName.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        }

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
            if(cardTypeFrame != null)
            {
                cardTypeFrame.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
                creatureVFrame.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            }
            cardType.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
            creatureValues.text = cardToDisplay.power + "/" + cardToDisplay.hp;
        }
        else
        {
            if (cardTypeFrame != null)
            {
                creatureVFrame.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
            }
            creatureValues.text = "";
            cardType.sprite = ImageHelper.GetCardTypeImage(cardToDisplay.type.FastCardTypeString());
        }

        if (cardToDisplay.isUpgradable)
        {
            upgradeShine.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue);
        }
        else
        {
            upgradeShine.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        }

    }


}

