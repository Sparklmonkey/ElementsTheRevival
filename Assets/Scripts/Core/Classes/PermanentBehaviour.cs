using System.Collections.Generic;
using UnityEngine;

public class PermanentBehaviour : CardTypeBehaviour
{
    protected override void OnCardPlay(OnCardPlayEvent onCardPlayEvent)
    {
        if (!onCardPlayEvent.IdPlayed.Equals(cardPair.id))
        {
            return;
        }
        
        cardPair.card = onCardPlayEvent.CardPlayed;
        if (cardPair.card.cardType == CardType.Pillar)
        {
            cardPair.stackCount++;
        }
        else
        {
            cardPair.stackCount = 1;
        }
        StackCount = cardPair.stackCount;
        
        switch (cardPair.card.iD)
        {
            case "7q9":
            case "5rp":
                cardPair.card.TurnsInPlay = 2;
                Owner.AddPlayerCounter(PlayerCounters.Delay, 1);
                DuelManager.Instance.GetNotIDOwner(cardPair.id).AddPlayerCounter(PlayerCounters.Delay, 1);
                break;
            case "5v2":
            case "7ti":
                Owner.AddPlayerCounter(PlayerCounters.Invisibility, 3);
                cardPair.card.TurnsInPlay = 3;
                Owner.ActivateCloakEffect(cardPair);
                break;
            case "5j2":
            case "7hi":
                Owner.AddPlayerCounter(PlayerCounters.Patience, 1);
                break;
            case "5uq":
            case "7ta":
                DuelManager.Instance.UpdateNightFallEclipse(true, cardPair.card.iD);
                break;
            case "5pa":
            case "7nq":
                Owner.AddPlayerCounter(PlayerCounters.Freedom, 1);
                break;
            case "5ih":
            case "7h1":
                DuelManager.Instance.AddFloodCount(1);
                break;
        }

        if (cardPair.card.innateSkills.Sanctuary)
        {
            Owner.AddPlayerCounter(PlayerCounters.Sanctuary, 1);
        }
        EventBus<UpdateCardDisplayEvent>.Raise(new UpdateCardDisplayEvent(onCardPlayEvent.IdPlayed, cardPair.card, StackCount, cardPair.isHidden));
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
        
        switch (cardPair.card.iD)
        {
            case "5v2":
            case "7ti":
                Owner.ResetCloakPermParent(cardPair);
                if (Owner.playerPermanentManager.GetAllValidCardIds().FindAll(x => x.card.iD is "5v2" or "7ti").Count == 1)
                {
                    Owner.DeactivateCloakEffect();
                    Owner.AddPlayerCounter(PlayerCounters.Invisibility, -3);
                }
                break;
            case "5j2":
            case "7hi":
                Owner.AddPlayerCounter(PlayerCounters.Patience, -1);
                break;
            case "5uq":
            case "7ta":
                DuelManager.Instance.UpdateNightFallEclipse(false, cardPair.card.iD);
                break;
            case "5pa":
            case "7nq":
                Owner.AddPlayerCounter(PlayerCounters.Freedom, -1);
                break;
            case "5ih":
            case "7h1":
                DuelManager.Instance.AddFloodCount(-1);
                break;
        }

        if (cardPair.card.innateSkills.Sanctuary)
        {
            Owner.AddPlayerCounter(PlayerCounters.Sanctuary, -1);
        }
        if(cardPair.stackCount == 0)
        {
            cardPair.card = null;
        }
    }

    public override void OnTurnStart()
    {
        List<string> permanentsWithCountdown = new() { "7q9", "5rp", "5v2", "7ti" };
        cardPair.card.AbilityUsed = false;
        if (permanentsWithCountdown.Contains(cardPair.card.iD))
        {
            cardPair.card.TurnsInPlay--;

            if (cardPair.card.TurnsInPlay == 0)
            {
                EventBus<OnCardRemovedEvent>.Raise(new OnCardRemovedEvent(cardPair.id));
                return;
            }
        }
        cardPair.UpdateCard();
    }

    protected override void DeathTrigger(OnDeathDTriggerEvent onDeathTriggerEvent)
    {
        if (!cardPair.HasCard())
        {
            return;
        }
        if (cardPair.card.innateSkills.SoulCatch)
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(cardPair.card.iD.IsUpgraded() ? 3 : 2, Element.Death, Owner.isPlayer, true));
        }
        if (cardPair.card.innateSkills.Boneyard)
        {
            Owner.PlayCardOnField(CardDatabase.Instance.GetCardFromId(cardPair.card.iD.IsUpgraded() ? "716" : "52m"));
        }
    }

    private void PillarEndTurnAction()
    {
        if (cardPair.card.cardName.Contains("Pendulum"))
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(StackCount, cardPair.card.skillElement, cardPair.id.owner == OwnerEnum.Player, true));
            if (Owner.isPlayer || Owner.playerCounters.invisibility <= 0)
            {
                AnimationManager.Instance.StartAnimation("QuantaGenerate", transform, cardPair.card.costElement);
            }

            cardPair.card.skillElement = cardPair.card.skillElement == cardPair.card.costElement ? DuelManager.Instance.GetIDOwner(cardPair.id).playerPassiveManager.GetMark().card.costElement : cardPair.card.costElement;
        }
        else
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(cardPair.card.costElement == Element.Other ? 3 * StackCount : 1 * StackCount, cardPair.card.costElement, cardPair.id.owner == OwnerEnum.Player, true));
            
            if (Owner.isPlayer || Owner.playerCounters.invisibility <= 0)
            {
                AnimationManager.Instance.StartAnimation("QuantaGenerate", transform, cardPair.card.costElement);
            }
        }
    }

    private void ArtifactEndTurnAction()
    {
        if (cardPair.card.innateSkills.Empathy)
        {
            var creatureCount = Owner.playerCreatureField.GetAllValidCardIds().Count;
            Owner.ModifyHealthLogic(creatureCount, false, false);
        }
        if (cardPair.card.innateSkills.Gratitude)
        {
            var healthAmount = Owner.playerPassiveManager.GetMark().card.costElement == Element.Life ? 5 : 3;
            Owner.ModifyHealthLogic(healthAmount, false, false);
        }

        List<int> floodList = new() { 11, 13, 9, 10, 12 };
        switch (cardPair.card.iD)
        {
            case "5j2":
            case "7hi":
                var creatureList = Owner.playerCreatureField.GetAllValidCardIds();
                foreach (var creature in creatureList)
                {
                    var statModifier = DuelManager.Instance.GetCardCount(new() { "5ih", "7h1" }) > 0 && floodList.Contains(cardPair.id.index) ? 5 : 2;
                    creature.card.AtkModify += statModifier;
                    creature.card.AtkModify += statModifier;
                    cardPair.UpdateCard();
                }
                break;
            case "5ih":
            case "7h1":
                DuelManager.Instance.enemy.ClearFloodedArea(floodList);
                DuelManager.Instance.player.ClearFloodedArea(floodList);
                break;
        }

        if (cardPair.card.innateSkills.Sanctuary)
        {
            Owner.ModifyHealthLogic(4, false, false);
        }

        if (cardPair.card.innateSkills.Void)
        {
            var healthChange = Owner.playerPassiveManager.GetMark().card.costElement == Element.Darkness ? 3 : 2;
            EventBus<ModifyPlayerHealthLogicEvent>.Raise(new ModifyPlayerHealthLogicEvent(-healthChange, !Owner.isPlayer, true));
        }
    }

    protected override void OnTurnEnd(OnTurnEndEvent onTurnEndEvent)
    {
        if (!onTurnEndEvent.CardType.Equals(CardType.Artifact) && !onTurnEndEvent.CardType.Equals(CardType.Pillar))
        {
            return;
        }
        
        if (!cardPair.HasCard() || onTurnEndEvent.IsPlayer != cardPair.isPlayer)
        {
            return;
        }
        
        if (cardPair.card.cardType == onTurnEndEvent.CardType && onTurnEndEvent.CardType == CardType.Artifact)
        {
            ArtifactEndTurnAction();
        }

        if (cardPair.card.cardType == onTurnEndEvent.CardType && onTurnEndEvent.CardType == CardType.Pillar)
        {
            PillarEndTurnAction();
        }

    }
}