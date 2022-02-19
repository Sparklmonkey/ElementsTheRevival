using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ownerName, ownerCard, targetOwner, targetCard;
    [SerializeField]
    private Image playerSprite, enemySprite, actionArrow;

    public void SetupFromElementAction(ElementAction elementAction)
    {

        ownerName.text = elementAction.playerName;
        ownerCard.text = elementAction.playerCard;
        targetOwner.text = elementAction.enemyName;
        targetCard.text = elementAction.enemyCard;

        if(elementAction.playerSprite == null)
        {
            playerSprite.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue);
        }
        else
        {
            playerSprite.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            playerSprite.sprite = ImageHelper.GetCardImage(elementAction.playerSprite);
        }

        if (elementAction.targetSprite == null)
        {
            enemySprite.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue);
        }
        else
        {
            enemySprite.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            enemySprite.sprite = ImageHelper.GetCardImage(elementAction.targetSprite);
        }

        if (elementAction.shouldShowArrow)
        {
            actionArrow.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        }
        else
        {
            actionArrow.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue);
        }

    }

}
