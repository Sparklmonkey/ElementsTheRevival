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

        owner.text = elementAction.Owner;
        action.text = elementAction.Action;

        if (elementAction.OriginImage is null)
        {
            originImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue);
        }
        else
        {
            originImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            originImage.sprite = elementAction.OriginImage;
        }

        if (elementAction.TargetImage is null)
        {
            targetImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue);
        }
        else
        {
            targetImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            targetImage.sprite = elementAction.TargetImage;
        }

        if (elementAction.ShouldShowArrow)
        {
            actionArrow.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        }
        else
        {
            actionArrow.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue);
        }

    }

}
