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
        if (cardToDisplay is null) { gameObject.SetActive(false); return; }
        card = cardToDisplay;

        if (cardToDisplay.CardName.Contains("Pendulum"))
        {
            var pendulumElement = cardToDisplay.CostElement;
            var markElement = belongsToPlayer ? PlayerData.Shared.markElement : BattleVars.Shared.EnemyAiData.mark;
            cardImage.sprite = cardToDisplay.IsPendulumTurn ? 
                ImageHelper.GetPendulumImage(pendulumElement.FastElementString(), markElement.FastElementString()) : 
                ImageHelper.GetPendulumImage(markElement.FastElementString(), pendulumElement.FastElementString());
        }
        else
        {
            cardImage.sprite = cardToDisplay.cardImage;
        }

        cardName.text = cardToDisplay.CardName;
        var backGroundString = cardToDisplay.CardElement.ToString();
        cardBack.sprite = ImageHelper.GetCardBackGroundImage(backGroundString);
        cardCost.text = cardToDisplay.Cost.ToString();
        elementImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        elementImage.sprite = ImageHelper.GetElementImage(cardToDisplay.CostElement.FastElementString());
        cardDescription.text = cardToDisplay.Desc;


        if (cardToDisplay.Id.IsUpgraded())
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

        if (cardToDisplay.Cost == 0)
        {
            cardCost.text = "";
            elementImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
        }

        if (cardToDisplay.CostElement.Equals(Element.Other))
        {
            elementImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
        }

        if (cardToDisplay.Type.Equals(CardType.Creature))
        {
            cardType.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
            creatureValues.text = cardToDisplay.Atk + "/" + cardToDisplay.Def;
        }
        else
        {
            creatureValues.text = "";
            cardType.sprite = ImageHelper.GetCardTypeImage(cardToDisplay.Type.FastCardTypeString());
        }
    }
}

