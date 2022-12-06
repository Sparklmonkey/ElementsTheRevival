using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplayDetail : MonoBehaviour
{

    [SerializeField]
    private Image cardImage, elementImage, cardBack, cardType, upgradeShine;

    [SerializeField]
    private TextMeshProUGUI cardName, cardCost, cardDescription, creatureValues;
    [SerializeField]
    private TMP_FontAsset underLayBlack, underLayWhite;
    public Card card;

    public void SetupCardView(Card cardToDisplay, bool belongsToPlayer, bool isBattlefield)
    {
        if(cardToDisplay == null) { gameObject.SetActive(false); return; }
        card = cardToDisplay;

        if (cardToDisplay.cardName.Contains("Pendulum") && isBattlefield)
        {
            Element pendulumElement = cardToDisplay.costElement;
            Element markElement = belongsToPlayer ? PlayerData.shared.markElement : BattleVars.shared.enemyAiData.mark;
            if (cardToDisplay.costElement == cardToDisplay.skillElement)
            {
                cardImage.sprite = ImageHelper.GetPendulumImage(pendulumElement.FastElementString(), markElement.FastElementString());
            }
            else
            {
                cardImage.sprite = ImageHelper.GetPendulumImage(markElement.FastElementString(), pendulumElement.FastElementString());
            }
        }
        else
        {
            cardImage.sprite = ImageHelper.GetCardImage(cardToDisplay.imageID);
        }
        
        cardName.text = cardToDisplay.cardName;
        string backGroundString = cardToDisplay.cardName == "Animate Weapon" ? "Air" :
                                cardToDisplay.cardName == "Luciferin" || cardToDisplay.cardName == "Luciferase" ? "Light" :
                                cardToDisplay.costElement.ToString();
        cardBack.sprite = ImageHelper.GetCardBackGroundImage(backGroundString);
        cardCost.text = cardToDisplay.cost.ToString();
        elementImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        elementImage.sprite = ImageHelper.GetElementImage(cardToDisplay.costElement.FastElementString());
        cardDescription.text = cardToDisplay.desc;


        if (cardToDisplay.iD.IsUpgraded())
        {
            cardName.color = new Color32(byte.MinValue, byte.MinValue, byte.MinValue, byte.MaxValue);
            cardName.font = underLayWhite;
            upgradeShine.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        }
        else
        {
            cardName.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            cardName.font = underLayBlack;
            upgradeShine.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue);
        }

        if (cardToDisplay.cost == 0)
        {
            cardCost.text = "";
            elementImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
        }

        if (cardToDisplay.costElement.Equals(Element.Other))
        {
            elementImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
        }

        if (cardToDisplay.cardType.Equals(CardType.Creature))
        {
            cardType.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
            creatureValues.text = cardToDisplay.atk + "/" + cardToDisplay.def;
        }
        else
        {
            creatureValues.text = "";
            cardType.sprite = ImageHelper.GetCardTypeImage(cardToDisplay.cardType.FastCardTypeString());
        }
    }
}

