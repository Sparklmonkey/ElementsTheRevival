using UnityEngine;

public class PassiveBehaviour : CardTypeBehaviour
{
    protected override void OnCardPlay(OnCardPlayEvent onCardPlayEvent)
    {
        if (!onCardPlayEvent.IdPlayed.Equals(cardPair.id))
        {
            return;
        }
        cardPair.card = onCardPlayEvent.CardPlayed;
        cardPair.stackCount = 1;
        StackCount = cardPair.stackCount;
        
        switch (cardPair.card.iD)
        {
            case "7n8":
            case "5oo":
                cardPair.card.TurnsInPlay = 5;
                break;
            case "61t":
            case "80d":
                cardPair.card.TurnsInPlay = 3;
                break;
        }

        if (cardPair.card.innateSkills.Bones)
        {
            Owner.AddPlayerCounter(PlayerCounters.Bone, 7);
        }
        EventBus<UpdateCardDisplayEvent>.Raise(new UpdateCardDisplayEvent(cardPair.id, cardPair.card, cardPair.stackCount, cardPair.isHidden));
    }

    protected override void OnCardRemove(OnCardRemovedEvent onCardRemovedEvent)
    {
        if (!onCardRemovedEvent.IdRemoved.Equals(cardPair.id))
        {
            return;
        }
        
        AnimationManager.Instance.StartAnimation("CardDeath", transform);
        EventBus<PlaySoundEffectEvent>.Raise(new PlaySoundEffectEvent("RemoveCardFromField"));
        
        cardPair.isHidden = true;
        cardPair.stackCount = 0;
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(cardPair.id, cardPair.stackCount, cardPair.card));
        
        if (cardPair.card.innateSkills.Bones)
        {
            Owner.AddPlayerCounter(PlayerCounters.Bone, -9999);
        }

        switch (cardPair.card.cardType)
        {
            case CardType.Weapon:
                var placeHolder = CardDatabase.Instance.GetPlaceholderCard(1);
                EventBus<OnCardPlayEvent>.Raise(new OnCardPlayEvent(cardPair.id, placeHolder));
                break;
            case CardType.Shield:
                placeHolder = CardDatabase.Instance.GetPlaceholderCard(0);
                EventBus<OnCardPlayEvent>.Raise(new OnCardPlayEvent(cardPair.id, placeHolder));
                break;
        }
    }

    public override void OnTurnStart()
    {
        switch (cardPair.card.iD)
        {
            case "7n8":
            case "5oo":
                cardPair.card.TurnsInPlay--;
                break;
            case "61t":
            case "80d":
                cardPair.card.TurnsInPlay--;
                break;
        }
    }

    protected override void DeathTrigger(OnDeathDTriggerEvent onDeathDTriggerEvent)
    {
        if (!cardPair.HasCard()) return;
        if (cardPair.card.innateSkills.Bones)
        {
            Owner.AddPlayerCounter(PlayerCounters.Bone, 2);
        }
    }

    protected override void OnTurnEnd(OnTurnEndEvent onTurnEndEvent)
    {
        if (!cardPair.HasCard()) return;
        switch (onTurnEndEvent.CardType)
        {
            case CardType.Spell:
            case CardType.Pillar:
            case CardType.Artifact:
            case CardType.Creature:
                return;
        }

        if (onTurnEndEvent.IsPlayer != cardPair.isPlayer) return;
        
        if (onTurnEndEvent.CardType == CardType.Mark && cardPair.card.cardType.Equals(CardType.Mark))
        {
            AnimationManager.Instance.StartAnimation("QuantaGenerate", transform, cardPair.card.costElement);
            if (BattleVars.Shared.EnemyAiData.maxHp >= 150 && cardPair.id.owner == OwnerEnum.Opponent)
            {
                EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(3, cardPair.card.costElement, Owner.isPlayer, true));
            }
            else
            {
                EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, cardPair.card.costElement, Owner.isPlayer, true));
            }
        }

        if (onTurnEndEvent.CardType != CardType.Weapon || !cardPair.card.cardType.Equals(CardType.Weapon)) return;
        var atknow = cardPair.card.AtkNow;

        if (cardPair.card.innateSkills.Regenerate)
        {
            Owner.ModifyHealthLogic(5, false, false);
        }
        if (cardPair.card.innateSkills.Fiery)
        {
            atknow += Mathf.FloorToInt(Owner.GetAllQuantaOfElement(Element.Fire) / 5);
        }
        if (cardPair.card.innateSkills.Hammer)
        {
            if (Owner.playerPassiveManager.GetMark().card.costElement == Element.Earth || Owner.playerPassiveManager.GetMark().card.costElement == Element.Gravity)
            {
                atknow++;
            }
        }
        if (cardPair.card.innateSkills.Dagger)
        {
            if (Owner.playerPassiveManager.GetMark().card.costElement == Element.Death || Owner.playerPassiveManager.GetMark().card.costElement == Element.Darkness)
            {
                atknow++;
            }
        }
        if (cardPair.card.innateSkills.Bow)
        {
            if (Owner.playerPassiveManager.GetMark().card.costElement == Element.Air)
            {
                atknow++;
            }
        }

        if (cardPair.card.Freeze > 0)
        {
            atknow = 0;
            cardPair.card.Freeze--;
        }

        if (cardPair.card.innateSkills.Delay > 0)
        {
            atknow = 0;
            cardPair.card.innateSkills.Delay--;
        }

        if (!cardPair.card.passiveSkills.Momentum)
        {
            Enemy.ManageShield(ref atknow, ref cardPair);
        }

        //Send Damage
        Enemy.ModifyHealthLogic(atknow, true, false);

        if (atknow > 0)
        {
            if (cardPair.card.passiveSkills.Vampire)
            {
                Owner.ModifyHealthLogic(atknow, false, false);
            }
            if (cardPair.card.passiveSkills.Venom)
            {
                Enemy.AddPlayerCounter(PlayerCounters.Poison, 1);
            }
            if (cardPair.card.innateSkills.Scramble)
            {
                Enemy.ScrambleQuanta();
            }
        }
        cardPair.card.AbilityUsed = false;


    }
}