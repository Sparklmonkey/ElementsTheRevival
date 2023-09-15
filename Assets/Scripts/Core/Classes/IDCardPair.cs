using System;
using Elements.Duel.Visual;
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
    public CardTypeBehaviour cardBehaviour;

    public bool IsActive() => transform.parent.gameObject.activeSelf;

    public event Action<Card, int, bool> OnCardChanged;
    public event Action<Card, int> OnCardRemoved;

    public event Action<IDCardPair> OnClickObject;

    public event Action<bool> OnBeingTargeted;
    public event Action<bool> OnBeingPlayable;

    void Start()
    {
        cardBehaviour = GetComponent<CardTypeBehaviour>();
        cardBehaviour.cardPair = this;
        cardBehaviour.Owner = isPlayer ? DuelManager.Instance.player : DuelManager.Instance.enemy;
        cardBehaviour.Enemy = isPlayer ? DuelManager.Instance.enemy : DuelManager.Instance.player;
        var parentName = transform.parent.gameObject.name;

        if (parentName != "EnemySide" && parentName != "PlayerSide")
        {
            int index = int.Parse(parentName.Split("_")[1]) - 1;
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
            if (effectDisplay != null)
            {
                OnCardChanged += effectDisplay.UpdateEffectDisplay;
            }

            var cardDisplayer = GetComponent<CardDisplayer>();
            OnCardChanged += cardDisplayer.DisplayCard;
            OnCardRemoved += cardDisplayer.HideCard;
            OnBeingTargeted += cardDisplayer.ShouldShowTarget;
            OnBeingPlayable += cardDisplayer.ShouldShowUsableGlow;
        }
        else if (parentName == "EnemySide" || parentName == "PlayerSide")
        {
            var cardDisplayer = GetComponent<PlayerDisplayer>();
            OnBeingTargeted += cardDisplayer.ShouldShowTarget;
        }

        OnClickObject += DuelManager.Instance.IdCardTapped;

        if (parentName != "EnemySide" && parentName != "PlayerSide" && !parentName.Contains("Passive"))
        {
            transform.parent.gameObject.SetActive(false);
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

    public void PlayCard(Card card)
    {
        this.card = card;
        if (card.cardType == CardType.Pillar && id.field == FieldEnum.Permanent)
        {
            stackCount++;
        }
        else
        {
            stackCount = 1;
        }
        cardBehaviour.StackCount = stackCount;
        cardBehaviour.OnCardPlay();
        OnCardChanged?.Invoke(card, stackCount, isHidden);
    }

    public void UpdateCard()
    {
        if (!HasCard()) { return; }
        if (card.cardType.Equals(CardType.Creature))
        {
            if (card.DefDamage < 0) { card.DefDamage = 0; }
            if (card.DefNow <= 0)
            {
                RemoveCard();
                return;
            }
        }
        OnCardChanged?.Invoke(card, stackCount, isHidden);
    }

    public void RemoveCard()
    {
        if (id.field != FieldEnum.Hand)
        {
            AnimationManager.Instance.StartAnimation("CardDeath", transform);
            SoundManager.Instance.PlayAudioClip("RemoveCardFromField");
        }
        isHidden = true;
        stackCount--;
        if(stackCount < 0) { stackCount = 0; }
        cardBehaviour.OnCardRemove();
        OnCardRemoved?.Invoke(card, stackCount);
    }

    public Card GetCard() => card;

    internal bool HasCard()
    {
        if (card == null && id.field != FieldEnum.Player) { return false; }
        return card != null && card.iD != "4t2" && card.iD != "4t1" && card.cardName != "" && card.cardType != CardType.Mark;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ToolTipCanvas.Instance.HideToolTip();
        if (!HasCard() && id.field != FieldEnum.Player) { return; }
        OnClickObject?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!HasCard()) { return; }
        if (id.field == FieldEnum.Hand && id.owner == OwnerEnum.Opponent) { return; }
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 objectSize = new(rectTransform.rect.height, rectTransform.rect.width);
        ToolTipCanvas.Instance.SetupToolTip(new Vector2(transform.position.x, transform.position.y), objectSize, card, id.index + 1, id.field == FieldEnum.Creature);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipCanvas.Instance.HideToolTip();
    }

    public bool IsFromHand()
    {
        return id.field.Equals(FieldEnum.Hand);
    }

    public void HandCardUpdate(Card card)
    {
        if (card == null)
        {
            OnCardRemoved?.Invoke(card, 0);
            return;
        }
        this.card = card;
        OnCardChanged?.Invoke(card, stackCount, isHidden);
    }

    public void IsTargeted(bool shouldShowTarget)
    {
        OnBeingTargeted?.Invoke(shouldShowTarget);
    }

    public void IsPlayable(bool shouldShowGlow)
    {
        OnBeingPlayable?.Invoke(shouldShowGlow);
    }

}
