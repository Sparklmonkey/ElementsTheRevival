using System.Collections.Generic;
using Core.Helpers;
using UnityEngine;

public static class IdCardTupleExtensions
{
    public static void CardInnateEffects(this (ID id, Card card) cardPair, ref int atkNow)
    {
        var owner = DuelManager.Instance.GetIDOwner(cardPair.id);
        if (cardPair.card.innateSkills.Regenerate)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(5, false, false, owner.Owner));
        }
        if (cardPair.card.innateSkills.Fiery)
        {
            atkNow += Mathf.FloorToInt(owner.GetAllQuantaOfElement(Element.Fire) / 5);
        }
        if (cardPair.card.innateSkills.Hammer)
        {
            if (owner.playerPassiveManager.GetMark().Item2.costElement == Element.Earth || owner.playerPassiveManager.GetMark().Item2.costElement == Element.Gravity)
            {
                atkNow++;
            }
        }
        if (cardPair.card.innateSkills.Dagger)
        {
            if (owner.playerPassiveManager.GetMark().Item2.costElement == Element.Death || owner.playerPassiveManager.GetMark().Item2.costElement == Element.Darkness)
            {
                atkNow++;
            }
        }

        if (!cardPair.card.innateSkills.Bow) return;
        if (owner.playerPassiveManager.GetMark().Item2.costElement == Element.Air)
        {
            atkNow++;
        }
    }

    public static bool IsTargetable(this Card card)
    {
        return !card.innateSkills.Immaterial && !card.passiveSkills.Burrow;
    }
    
    public static bool IsTargetable(this (ID, Card) tuple)
    {
        if (tuple.Item1.IsPlayerField())
        {
            return true;
        }
        return !tuple.Item2.innateSkills.Immaterial && !tuple.Item2.passiveSkills.Burrow;
    }
    
    public static bool HasCard(this (ID id, Card card) tuple)
    {
        return tuple.card != null && tuple.card.iD != "4t2" && tuple.card.iD != "4t1";
    }
    
    public static void EndTurnPassiveEffect(this (ID id, Card card) cardPair)
    {
        var owner = DuelManager.Instance.GetIDOwner(cardPair.id);
        var enemy = DuelManager.Instance.GetNotIDOwner(cardPair.id);
        if (cardPair.card.innateSkills.Delay == 0)
        {
            if (cardPair.card.passiveSkills.Air)
            {;
                EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(cardPair.id, "QuantaGenerate", Element.Air));
                EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, Element.Air, owner.Owner, true));
            }
            if (cardPair.card.passiveSkills.Earth)
            {
                EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(cardPair.id, "QuantaGenerate", Element.Earth));
                EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, Element.Earth, owner.Owner, true));
            }
            if (cardPair.card.passiveSkills.Fire)
            {
                EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(cardPair.id, "QuantaGenerate", Element.Fire));
                EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, Element.Fire, owner.Owner, true));
            }
            if (cardPair.card.passiveSkills.Light)
            {
                EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(cardPair.id, "QuantaGenerate", Element.Light));
                EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, Element.Light, owner.Owner, true));
            }
            if (cardPair.card.innateSkills.Devourer)
            {
                if (enemy.GetAllQuantaOfElement(Element.Other) > 0 && enemy.playerCounters.sanctuary == 0)
                {
                    EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, Element.Other, enemy.Owner, false));
                }
                EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(cardPair.id, "QuantaGenerate", Element.Darkness));
                EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, Element.Darkness, owner.Owner, true));
            }
            if (cardPair.card.passiveSkills.Overdrive)
            {
                cardPair.card.AtkModify += 3;
                cardPair.card.DefModify -= 1;
            }
            if (cardPair.card.passiveSkills.Acceleration)
            {
                cardPair.card.AtkModify += 2;
                cardPair.card.DefModify -= 1;
            }
            if (cardPair.card.passiveSkills.Infest)
            {
                var card = CardDatabase.Instance.GetCardFromId("4t8");
                
                EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(card, cardPair.id.IsOwnedBy(OwnerEnum.Player)));
                EventBus<PlayCreatureOnFieldEvent>.Raise(new PlayCreatureOnFieldEvent(cardPair.id.owner, card));
            }

        }

        if (!cardPair.card.innateSkills.Swarm) return;
        cardPair.card.def = owner.playerCounters.scarab;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(cardPair.id, cardPair.card, true));
    }
    
    public static void CreatureTurnDownTick(this (ID id, Card card) cardPair)
    {
        if (cardPair.card.passiveSkills.Dive)
        {
            cardPair.card.passiveSkills.Dive = false;
            cardPair.card.atk /= 2;
            cardPair.card.AtkModify /= 2;
        }

        if (cardPair.card.Freeze > 0)
        {
            cardPair.card.Freeze--;
        }

        if (cardPair.card.Charge > 0)
        {
            cardPair.card.Charge--;
            cardPair.card.AtkModify--;

        }
        if (cardPair.card.innateSkills.Delay > 0)
        {
            cardPair.card.innateSkills.Delay--;
        }

        var healthChange = cardPair.card.Poison;
        cardPair.card.DefDamage += healthChange;
    }
    
    public static void SingularityEffect(this (ID id, Card card) cardPair)
    {
        if (!cardPair.card.innateSkills.Singularity) { return; }
        List<string> singuEffects = new() { "Chaos", "Copy", "Nova" };
        if (cardPair.card.AtkNow > 0)
        {
            cardPair.card.atk = -Mathf.Abs(cardPair.card.atk);
            cardPair.card.AtkModify = -Mathf.Abs(cardPair.card.AtkModify);
        }

        if (!cardPair.card.innateSkills.Immaterial)
        {
            singuEffects.Add("Immaterial");
        }
        if (!cardPair.card.passiveSkills.Adrenaline)
        {
            singuEffects.Add("Adrenaline");
        }
        if (!cardPair.card.passiveSkills.Vampire)
        {
            singuEffects.Add("Vampire");
        }

        switch (singuEffects[Random.Range(0, singuEffects.Count)])
        {
            case "Immaterial":
                cardPair.card.innateSkills.Immaterial = true;
                break;
            case "Vampire":
                cardPair.card.passiveSkills.Vampire = true;
                break;
            case "Chaos":
                var chaos = Random.Range(1, 6);
                cardPair.card.AtkModify += chaos;
                cardPair.card.DefModify += chaos;
                break;
            case "Nova":
                for (var y = 0; y < 12; y++)
                {
                    EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, (Element)y, cardPair.id.owner, true));
                }
                break;
            case "Adrenaline":
                cardPair.card.passiveSkills.Adrenaline = true;
                break;
            default:
                Card duplicate = new(cardPair.card);
                EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(cardPair.id, "ParallelUniverse", Element.Air));
                EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(duplicate, cardPair.id.IsOwnedBy(OwnerEnum.Player)));
                EventBus<PlayCreatureOnFieldEvent>.Raise(new PlayCreatureOnFieldEvent(cardPair.id.owner, duplicate));
                break;
        }
    }
}