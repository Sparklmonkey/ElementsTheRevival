using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public abstract class CardDisplayer : MonoBehaviour
{
    public ID displayerId;
    public bool isPlayer;
    [FormerlySerializedAs("MaskImage")] public GameObject maskImage;
    public TMP_FontAsset underlayBlack, underlayWhite;
    [SerializeField]
    private Image validTargetGlow;
    [SerializeField]
    private Image isUsableGlow;
    private Element _element;
    
    private void Awake()
    {
        var parentName = transform.parent.gameObject.name;
        ShouldShowTarget(new ShouldShowTargetableEvent(false, displayerId));
        var index = int.Parse(parentName.Split("_")[1]) - 1;

        if (parentName is "EnemySide" or "PlayerSide") return;
        if (parentName.Contains("Creature"))
        {
            displayerId = new(isPlayer ? OwnerEnum.Player : OwnerEnum.Opponent, FieldEnum.Creature, index);
        }
        else if (parentName.Contains("Permanent"))
        {
            displayerId = new(isPlayer ? OwnerEnum.Player : OwnerEnum.Opponent, FieldEnum.Permanent, index);
        }
        else if (parentName.Contains("Hand"))
        {
            displayerId = new(isPlayer ? OwnerEnum.Player : OwnerEnum.Opponent, FieldEnum.Hand, index);
        }
        else if (parentName.Contains("Passive"))
        {
            displayerId = new(isPlayer ? OwnerEnum.Player : OwnerEnum.Opponent, FieldEnum.Passive, index);
        }
    }

    private void Start()
    {
        if (!displayerId.field.Equals(FieldEnum.Passive))
        {
            ClearDisplay();
        }
    }

    public void ShouldShowTarget(ShouldShowTargetableEvent shouldShowTargetableEvent)
    {
        if (!shouldShowTargetableEvent.DisplayerId.Equals(displayerId))
        {
            return;
        }
        validTargetGlow.color = shouldShowTargetableEvent.ShouldShow ? new Color(15, 255, 0, 255) : new Color(0, 0, 0, 0);
    }

    public void ShouldShowUsableGlow(ShouldShowUsableEvent shouldShowUsableEvent)
    {
        if (!shouldShowUsableEvent.DisplayerId.Equals(displayerId))
        {
            return;
        }
        
        if (gameObject.name.Contains("Hand"))
        {
            validTargetGlow.color = shouldShowUsableEvent.ShouldShow ? new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue) : new Color(0, 0, 0, 0);
        }
        else
        {
            isUsableGlow.gameObject.SetActive(shouldShowUsableEvent.ShouldShow);
        }
    }

    public void ClearDisplay()
    {
        maskImage.gameObject.SetActive(false);
    }
}
