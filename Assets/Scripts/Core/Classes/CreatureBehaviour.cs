using UnityEngine;

public class CreatureBehaviour : CardTypeBehaviour
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
        
        if (cardPair.card.innateSkills.Swarm)
        {
            Owner.AddPlayerCounter(PlayerCounters.Scarab, 1);
        }

        if (cardPair.card.innateSkills.Integrity)
        {
            var shardList = Owner.playerHand.GetAllValidCardIds().FindAll(x => x.card.cardName.Contains("Shard of"));
            cardPair.card = CardDatabase.Instance.GetGolemAbility(shardList);
        }

        if (cardPair.card.innateSkills.Chimera)
        {
            var creatureList = Owner.playerCreatureField.GetAllValidCardIds();
            var chimeraPwrHp = (0, 0);

            if (creatureList.Count > 0)
            {
                foreach (var creature in creatureList)
                {
                    if (creature.id == cardPair.id) continue;
                    chimeraPwrHp.Item1 += creature.card.AtkNow;
                    chimeraPwrHp.Item2 += creature.card.DefNow;
                    EventBus<OnCardRemovedEvent>.Raise(new OnCardRemovedEvent(creature.id));
                }
            }

            cardPair.card.atk = chimeraPwrHp.Item1;
            cardPair.card.def = chimeraPwrHp.Item2;
        }
        if (cardPair.card.costElement.Equals(Element.Darkness) 
            || cardPair.card.costElement.Equals(Element.Death))
        {
            if (DuelManager.Instance.GetCardCount(new() { "7ta" }) > 0)
            {
                cardPair.card.DefModify += 1;
                cardPair.card.AtkModify += 2;
            }
            else if (DuelManager.Instance.GetCardCount(new() { "5uq" }) > 0)
            {
                cardPair.card.DefModify += 1;
                cardPair.card.AtkModify += 1;
            }
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
        
        Owner.AddPlayerCounter(PlayerCounters.Scarab, cardPair.card.innateSkills.Swarm ? -1 : 0);
        var shouldDeath = ShouldActivateDeathTriggers();
        if (cardPair.card.IsAflatoxin)
        {
            var card = CardDatabase.Instance.GetCardFromId("6ro");
            EventBus<OnCardPlayEvent>.Raise(new OnCardPlayEvent(cardPair.id, card));
        } 
        else if (cardPair.card.passiveSkills.Phoenix && !shouldDeath)
        {
            var card = CardDatabase.Instance.GetCardFromId(cardPair.card.iD.IsUpgraded() ? "7dt" : "5fd");
            EventBus<OnCardPlayEvent>.Raise(new OnCardPlayEvent(cardPair.id, card));
        }
        else
        {
            cardPair.card = null;
        }
        
        if (shouldDeath)
        {
            EventBus<OnDeathDTriggerEvent>.Raise(new OnDeathDTriggerEvent());
        }
    }

    private bool ShouldActivateDeathTriggers()
    {
        if (BattleVars.Shared.AbilityOrigin is not null)
        {
            return BattleVars.Shared.AbilityOrigin.card.skill is not "reversetime";
        }
        return cardPair.card.cardName.Contains("Skeleton");
    }

    public override void OnTurnStart()
    {
        return;
    }

    protected override void DeathTrigger(OnDeathDTriggerEvent onDeathTriggerEvent)
    {
        if (!cardPair.HasCard()) return;
        if (!cardPair.card.passiveSkills.Scavenger) return;
        cardPair.card.AtkModify += 1;
        cardPair.card.DefModify += 1;
        cardPair.UpdateCard();
    }

    protected override void OnTurnEnd(OnTurnEndEvent onTurnEndEvent)
    {
        if (!onTurnEndEvent.CardType.Equals(CardType.Creature)) return;
        if (onTurnEndEvent.IsPlayer != cardPair.isPlayer) return;
        if (!cardPair.HasCard()) return;
        
        var adrenalineIndex = 0;
        var hasAdrenaline = cardPair.card.passiveSkills.Adrenaline;
        var isFirstAttack = true;
        var atkNow = cardPair.card.AtkNow;

        var shouldSkip = DuelManager.Instance.GetCardCount(new() { "5rp", "7q9" }) > 0 || 
                         Owner.playerCounters.patience > 0 || 
                         cardPair.card.Freeze > 0 || 
                         cardPair.card.innateSkills.Delay > 0;
        cardPair.card.AbilityUsed = false;

        while (isFirstAttack || hasAdrenaline)
        {
            isFirstAttack = false;
            if (!shouldSkip)
            {
                cardPair.CardInnateEffects(ref atkNow);
                var isFreedomEffect = Random.Range(0, 100) < 25 * Owner.playerCounters.freedom && cardPair.card.costElement.Equals(Element.Air);
                atkNow = Mathf.FloorToInt(isFreedomEffect ? atkNow * 1.5f : atkNow);
                EventBus<PlaySoundEffectEvent>.Raise(new PlaySoundEffectEvent("CreatureDamage"));
                if (atkNow > 0)
                {
                    if (Enemy.HasGravityCreatures())
                    {
                        Enemy.ManageGravityCreatures(ref atkNow);
                    }

                    if (!cardPair.card.passiveSkills.Momentum)
                    {
                        if (Enemy.ManageShield(ref atkNow, ref cardPair))
                        {
                            if (cardPair.card.DefNow <= 0)
                            {
                                EventBus<OnCardRemovedEvent>.Raise(new OnCardRemovedEvent(cardPair.id));
                                return;
                            }
                        }
                    }
                }

                if (adrenalineIndex < 2)
                {
                    cardPair.EndTurnPassiveEffect();

                    if (atkNow > 0)
                    {
                        if (cardPair.card.passiveSkills.Venom)
                        {
                            Enemy.AddPlayerCounter(PlayerCounters.Poison, 1);
                        }

                        if (cardPair.card.passiveSkills.DeadlyVenom)
                        {
                            Enemy.AddPlayerCounter(PlayerCounters.Poison, 2);
                        }

                        if (cardPair.card.passiveSkills.Neurotoxin)
                        {
                            Enemy.AddPlayerCounter(PlayerCounters.Neurotoxin, 1);
                        }
                    }

                    if (atkNow != 0)
                    {
                        if (cardPair.card.passiveSkills.Vampire)
                        {
                            Owner.ModifyHealthLogic(atkNow, false, false);
                        }

                        Enemy.ModifyHealthLogic(atkNow, true, cardPair.card.passiveSkills.Psion);
                    }
                }

                cardPair.SingularityEffect();
                cardPair.UpdateCard();
                if (!cardPair.HasCard()) { return; }
                if (cardPair.card.AtkNow != 0 && hasAdrenaline)
                {
                    adrenalineIndex++;
                    if (DuelManager.AdrenalineDamageList[Mathf.Abs(cardPair.card.AtkNow) - 1].Count <= adrenalineIndex)
                    {
                        hasAdrenaline = false;
                    }
                    else
                    {
                        atkNow = DuelManager.AdrenalineDamageList[Mathf.Abs(cardPair.card.AtkNow) - 1][adrenalineIndex];
                        if (cardPair.card.passiveSkills.Antimatter)
                        {
                            atkNow = -atkNow;
                        }
                    }
                }
            }
            cardPair.CreatureTurnDownTick();
            cardPair.UpdateCard();
        }
    }
}
