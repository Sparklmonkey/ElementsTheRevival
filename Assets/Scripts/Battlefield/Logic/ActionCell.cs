using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI owner, action;
    [SerializeField]
    private Image originImage, targetImage, actionArrow;

    public void SetupFromElementAction(ElementAction elementAction)
    {

        owner.text = elementAction.owner;
        action.text = elementAction.action;

        if (elementAction.originImage == "")
        {
            originImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue);
        }
        else
        {
            originImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            originImage.sprite = ImageHelper.GetCardImage(elementAction.originImage);
        }

        if (elementAction.targetImage == "")
        {
            targetImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue);
        }
        else
        {
            targetImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            targetImage.sprite = ImageHelper.GetCardImage(elementAction.targetImage);
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
