using System;
using System.Collections.Generic;
using Elements.Duel.Manager;
using Elements.Duel.Visual;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class IDCardPair : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ID id;
    public Card card;
    public int stackCount;
    public bool isPlayer;
    public CardTypeBehaviour cardBehaviour;

    public bool IsActive() => transform.parent.gameObject.activeSelf;

    public event Action<Card, int> OnCardChanged;
    public event Action<Card, int> OnCardRemoved;

    public event Action<IDCardPair, bool> OnHoverObject;
    public event Action<IDCardPair> OnClickObject;

    public event Action<int> OnCreatureAttack;

    public event Action<bool> OnBeingTargeted;
    public event Action<bool> OnBeingPlayable;

    public event Action<IDCardPair, PlayerManager, PlayerManager> OnTurnEnd;

    void Start()
    {
        cardBehaviour = GetComponent<CardTypeBehaviour>();
        cardBehaviour.CardPair = this;
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
        var isCardTargetable = HasCard() ? !card.innate.Contains("immaterial") && !card.passive.Contains("burrow") : false;
        if (id.Field == FieldEnum.Player)
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
        if (card.cardType == CardType.Pillar && id.Field == FieldEnum.Permanent)
        {
            stackCount++;
        }
        else
        {
            stackCount = 1;
        }
        cardBehaviour.StackCount = stackCount;
        cardBehaviour.OnCardPlay();
        OnCardChanged?.Invoke(card, stackCount);
    }

    public void UpdateCard()
    {
        if (card.cardType.Equals(CardType.Creature))
        {
            if (card.DefDamage < 0) { card.DefDamage = 0; }
            if (card.DefNow <= 0)
            {
                RemoveCard();
                return;
            }
        }
        OnCardChanged?.Invoke(card, stackCount);
    }

    public void RemoveCard()
    {
        if(id.Field != FieldEnum.Hand)
        {
            StartCoroutine(Game_AnimationManager.shared.PlayAnimation("CardDeath", transform));
            Game_SoundManager.shared.PlayAudioClip("RemoveCardFromField");
        }
        stackCount--;
        cardBehaviour.OnCardRemove();
        OnCardRemoved?.Invoke(card, stackCount);
        if (stackCount == 0)
        {
            card = null;
        }
    }

    public Card GetCard() => card;

    internal bool HasCard()
    {
        return card != null && card.iD != "4t2" && card.iD != "4t1" && card.cardName != "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!HasCard() && id.Field != FieldEnum.Player) { return; }
        OnClickObject?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!HasCard()) { return; }
        if (id.Field == FieldEnum.Hand && id.Owner == OwnerEnum.Opponent) { return; }
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 objectSize = new(rectTransform.rect.height, rectTransform.rect.width);
        ToolTipCanvas.Instance.SetupToolTip(new Vector2(transform.position.x, transform.position.y), objectSize, card, id.Index + 1, id.Field == FieldEnum.Creature);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipCanvas.Instance.HideToolTip();
    }

    public bool IsFromHand()
    {
        return id.Field.Equals(FieldEnum.Hand);
    }

    public void HandCardUpdate(Card card)
    {
        if(card == null)
        {
            OnCardRemoved?.Invoke(card, 0);
            return;
        }
        this.card = card;
        OnCardChanged?.Invoke(card, stackCount);
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
