using System.Collections.Generic;
using Battlefield.Abilities;
using Core.Helpers;
using UnityEngine;

public static class IdCardTupleExtensions
{

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
        return tuple.card != null && tuple.card.Id != "4t2" && tuple.card.Id != "4t1";
    }
    
    public static void CreatureTurnDownTick(this (ID id, Card card) cardPair)
    {
        if (cardPair.card.passiveSkills.Dive)
        {
            cardPair.card.passiveSkills.Dive = false;
            cardPair.card.Atk /= 2;
            cardPair.card.AtkModify /= 2;
        }

        if (cardPair.card.Counters.Freeze > 0)
        {
            cardPair.card.Counters.Freeze--;
        }

        if (cardPair.card.Counters.Charge > 0)
        {
            cardPair.card.Counters.Charge--;
            cardPair.card.AtkModify--;

        }
        if (cardPair.card.Counters.Delay > 0)
        {
            cardPair.card.Counters.Delay--;
        }

        var healthChange = cardPair.card.Counters.Poison;
        cardPair.card.DefDamage += healthChange;
    }
}