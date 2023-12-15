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
        if (cardToDisplay.cardName.Contains("Pendulum") && SceneTransitionManager.Instance.GetActiveScene() == "Battlefield")
        {
            var pendulumElement = cardToDisplay.costElement;
            var markElement = BattleVars.Shared.IsPlayerTurn ? PlayerData.Shared.markElement : BattleVars.Shared.EnemyAiData.mark;
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

        cardImage.sprite = ImageHelper.GetCardImage(cardToDisplay.imageID);

        cardName.text = cardToDisplay.cardName;
        var backGroundString = cardToDisplay.cardName == "Animate Weapon" ? "Air" :
                                cardToDisplay.cardName == "Luciferin" || cardToDisplay.cardName == "Luciferase" ? "Light" :
                                cardToDisplay.costElement.ToString();
        cardBack.sprite = ImageHelper.GetCardBackGroundImage(backGroundString);
        cardCost.text = cardToDisplay.cost.ToString();
        elementImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        elementImage.sprite = ImageHelper.GetElementImage(cardToDisplay.costElement.FastElementString());
        cardDescription.text = cardToDisplay.desc;
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
            creatureValues.text = cardToDisplay.AtkNow + "|" + cardToDisplay.DefNow;
            creatureValues.font = creatureValuesFont;
        }
        else if (cardToDisplay.cardType.Equals(CardType.Spell))
        {
            creatureValues.text = "~";
            creatureValues.font = spellIconFont;
        }
        else
        {
            creatureValues.text = "";
            cardType.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        }

        if (_card.iD.IsUpgraded())
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

