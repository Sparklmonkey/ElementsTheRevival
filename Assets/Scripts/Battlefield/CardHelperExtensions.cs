using System.Collections.Generic;
using UnityEngine;

public static class CardHelperExtensions 
{
    public static void CardInnateEffects(this IDCardPair cardPair, ref int atkNow)
    {
        var owner = DuelManager.Instance.GetIDOwner(cardPair.id);
        if (cardPair.card.innateSkills.Regenerate)
        {
            owner.ModifyHealthLogic(5, false, false);
        }
        if (cardPair.card.innateSkills.Fiery)
        {
            atkNow += Mathf.FloorToInt(owner.GetAllQuantaOfElement(Element.Fire) / 5);
        }
        if (cardPair.card.innateSkills.Hammer)
        {
            if (owner.playerPassiveManager.GetMark().card.costElement == Element.Earth || owner.playerPassiveManager.GetMark().card.costElement == Element.Gravity)
            {
                atkNow++;
            }
        }
        if (cardPair.card.innateSkills.Dagger)
        {
            if (owner.playerPassiveManager.GetMark().card.costElement == Element.Death || owner.playerPassiveManager.GetMark().card.costElement == Element.Darkness)
            {
                atkNow++;
            }
        }

        if (!cardPair.card.innateSkills.Bow) return;
        if (owner.playerPassiveManager.GetMark().card.costElement == Element.Air)
        {
            atkNow++;
        }
    }
    
    public static void EndTurnPassiveEffect(this IDCardPair cardPair)
    {
        var owner = DuelManager.Instance.GetIDOwner(cardPair.id);
        var enemy = DuelManager.Instance.GetNotIDOwner(cardPair.id);
        if (cardPair.card.innateSkills.Delay == 0)
        {
            if (cardPair.card.passiveSkills.Air)
            {
                AnimationManager.Instance.StartAnimation("QuantaGenerate", cardPair.transform, Element.Air);
                EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, Element.Air, owner.isPlayer, true));
            }
            if (cardPair.card.passiveSkills.Earth)
            {
                AnimationManager.Instance.StartAnimation("QuantaGenerate", cardPair.transform, Element.Earth);
                EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, Element.Earth, owner.isPlayer, true));
            }
            if (cardPair.card.passiveSkills.Fire)
            {
                AnimationManager.Instance.StartAnimation("QuantaGenerate", cardPair.transform, Element.Fire);
                EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, Element.Fire, owner.isPlayer, true));
            }
            if (cardPair.card.passiveSkills.Light)
            {
                AnimationManager.Instance.StartAnimation("QuantaGenerate", cardPair.transform, Element.Light);
                EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, Element.Light, owner.isPlayer, true));
            }
            if (cardPair.card.innateSkills.Devourer)
            {
                if (enemy.GetAllQuantaOfElement(Element.Other) > 0 && enemy.playerCounters.sanctuary == 0)
                {
                    EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, Element.Other, enemy.isPlayer, false));
                }
                AnimationManager.Instance.StartAnimation("QuantaGenerate", cardPair.transform, Element.Darkness);
                EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, Element.Darkness, owner.isPlayer, true));
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
                owner.PlayCardOnField(CardDatabase.Instance.GetCardFromId("4t8"));
            }

        }

        if (cardPair.card.innateSkills.Swarm)
        {
            cardPair.card.def = owner.playerCounters.scarab;
            cardPair.UpdateCard();
        }
    }
    
    public static void CreatureTurnDownTick(this IDCardPair cardPair)
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
            cardPair.card.DefModify--;

        }
        if (cardPair.card.innateSkills.Delay > 0)
        {
            cardPair.card.innateSkills.Delay--;
        }

        var healthChange = cardPair.card.Poison;
        cardPair.card.DefDamage += healthChange;
    }
    
    public static void SingularityEffect(this IDCardPair cardPair)
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
                    EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, (Element)y, cardPair.id.owner == OwnerEnum.Player, true));
                }
                break;
            case "Adrenaline":
                cardPair.card.passiveSkills.Adrenaline = true;
                break;
            default:
                Card duplicate = new(cardPair.card);
                AnimationManager.Instance.StartAnimation("ParallelUniverse", cardPair.transform);
                DuelManager.Instance.GetIDOwner(cardPair.id).PlayCardOnField(duplicate);
                break;
        }
    }
}
