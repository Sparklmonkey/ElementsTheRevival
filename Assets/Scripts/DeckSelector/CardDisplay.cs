using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{

    [SerializeField]
    private Image cardImage, elementImage, cardBack, cardType, upgradeShine, rareIndicator;

    [SerializeField]
    private TextMeshProUGUI cardName, cardCost, cardDescription, creatureValues;
    [SerializeField]
    private TMP_FontAsset cardNameRegular, cardNameUpped, creatureValuesFont, spellIconFont;
    private Card _card;

    public void SetupCardView(Card cardToDisplay)
    {
        _card = cardToDisplay;
        if (cardToDisplay.CardName.Contains("Pendulum") && SceneTransitionManager.Instance.GetActiveScene() == "Battlefield")
        {
            var pendulumElement = cardToDisplay.CostElement;
            var markElement = BattleVars.Shared.IsPlayerTurn ? PlayerData.Shared.markElement : BattleVars.Shared.EnemyAiData.mark;
            if (cardToDisplay.CostElement == cardToDisplay.SkillElement)
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
            cardImage.sprite = cardToDisplay.cardImage;
        }

        cardName.text = cardToDisplay.CardName;
        var backGroundString = cardToDisplay.CardElement.ToString();
        cardBack.sprite = ImageHelper.GetCardBackGroundImage(backGroundString);
        cardCost.text = cardToDisplay.Cost.ToString();
        elementImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        elementImage.sprite = ImageHelper.GetElementImage(cardToDisplay.CostElement.FastElementString());
        cardDescription.text = cardToDisplay.Desc;
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
            creatureValues.text = cardToDisplay.AtkNow + "|" + cardToDisplay.DefNow;
            creatureValues.font = creatureValuesFont;
        }
        else if (cardToDisplay.Type.Equals(CardType.Spell))
        {
            creatureValues.text = "~";
            creatureValues.font = spellIconFont;
        }
        else
        {
            creatureValues.text = "";
            cardType.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        }

        if (_card.Id.IsUpgraded())
        {
            upgradeShine.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            cardName.font = cardNameUpped;
            cardName.color = new Color32(byte.MinValue, byte.MinValue, byte.MinValue, byte.MaxValue);
        }
        else
        {
            upgradeShine.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
            cardName.font = cardNameRegular;
            cardName.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        }

        rareIndicator.color = !cardToDisplay.IsRare() ? new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue) : new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
    }
}

