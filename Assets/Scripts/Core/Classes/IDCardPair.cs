using System;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class IDCardPair : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ID id;
    public Card card;
    public int stackCount = 0;
    public bool isPlayer;
    public bool isHidden = true;

    public bool IsActive() => transform.parent.gameObject.activeSelf;

    private void Start()
    {
        var parentName = transform.parent.gameObject.name;

        if (parentName != "EnemySide" && parentName != "PlayerSide")
        {
            var index = int.Parse(parentName.Split("_")[1]) - 1;
            if (parentName.Contains("Creature"))
            {
                id = new(isPlayer ? OwnerEnum.Player : OwnerEnum.Opponent, FieldEnum.Creature, index);
            }
            else if (parentName.Contains("Permanent"))
            {
                id = new(isPlayer ? OwnerEnum.Player : OwnerEnum.Opponent, FieldEnum.Permanent, index);
            }
            else if (parentName.Contains("Hand"))
            {
                id = new(isPlayer ? OwnerEnum.Player : OwnerEnum.Opponent, FieldEnum.Hand, index);
            }
            var effectDisplay = GetComponent<EffectDisplayManager>();
            if (effectDisplay is not null)
            {
                // OnCardChanged += effectDisplay.UpdateEffectDisplay;
            }
        }
    }

    public bool IsTargetable()
    {
        var isCardTargetable = HasCard() && !card.innateSkills.Immaterial && !card.passiveSkills.Burrow;
        if (id.field == FieldEnum.Player)
        {
            isCardTargetable = true;
        }
        return isCardTargetable;
    }

    public IDCardPair(ID id, Card card)
    {
        this.id = id;
        this.card = card;
    }

    public void UpdateCard()
    {
        if (!HasCard()) { return; }
        if (card.Type.Equals(CardType.Creature))
        {
            if (card.DefNow <= 0)
            {
                EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(id));
                return;
            }
        }
        EventBus<UpdateCardDisplayEvent>.Raise(new UpdateCardDisplayEvent(id, card, stackCount, isHidden));
    }

    public Card GetCard() => card;

    internal bool HasCard()
    {
        if (card == null && id.field != FieldEnum.Player) { return false; }
        return card != null && card.Id != "4t2" && card.Id != "4t1" && card.CardName != "" && card.Type != CardType.Mark;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventBus<DisplayCardToolTipEvent>.Raise(new DisplayCardToolTipEvent(id, card, true));
        if (!HasCard() && id.field != FieldEnum.Player) { return; }
        // EventBus<CardTappedEvent>.Raise(new CardTappedEvent(this));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!HasCard()) { return; }
        if (id.field == FieldEnum.Hand && id.owner == OwnerEnum.Opponent) { return; }
        var rectTransform = GetComponent<RectTransform>();
        Vector2 objectSize = new(rectTransform.rect.height, rectTransform.rect.width);
        EventBus<DisplayCardToolTipEvent>.Raise(new DisplayCardToolTipEvent(id, card, false));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventBus<DisplayCardToolTipEvent>.Raise(new DisplayCardToolTipEvent(id, card, true));
    }

}
