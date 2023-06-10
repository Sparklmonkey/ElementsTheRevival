using System.Collections.Generic;
using UnityEngine;

public abstract class FieldManager
{
    public List<IDCardPair> pairList;
    public List<int> stackCountList = new List<int>();

    public List<ID> GetAllIds()
    {
        List<ID> toReturn = new List<ID>();
        foreach (IDCardPair item in pairList)
        {
            if (item.card != null)
            {
                if (!item.card.cardType.Equals(CardType.Mark) && item.card.cardName != "Shield_" && item.card.cardName != "Weapon")
                {
                    toReturn.Add(item.id);
                }
            }
        }
        return toReturn;
    }

    public List<IDCardPair> GetAllValidCardIds()
    {
        return new List<IDCardPair>(pairList.FindAll(x => x.HasCard()));
    }

    public void PlayCardAtLocation(Card card, ID location)
    {
        pairList[location.Index].PlayCard(card);
    }

    private readonly List<int> creatureCardOrder = new() { 11, 13, 9, 10, 12, 14, 8, 16, 18, 20, 22, 0, 2, 4, 6, 15, 17, 19, 21, 1, 3, 5, 7 };
    private readonly List<int> safeZones = new() { 11, 13, 10, 12, 14 };
    private readonly List<int> permanentCardOrder = new() { 1, 3, 5, 7, 0, 2, 4, 6, 9, 11, 13, 8, 10, 12, 14 };

    public ID PlayCardAtRandomLocation(Card card)
    {
        ID canStack = CanStack(card);
        if (canStack != null)
        {
            stackCountList[canStack.Index]++;
            return canStack;
        }


        if (card.cardType.Equals(CardType.Creature) && DuelManager.floodCount > 0 && !card.costElement.Equals(Element.Other) && !card.costElement.Equals(Element.Water))
        {

            for (int i = 0; i < safeZones.Count; i++)
            {
                if (!pairList[safeZones[i]].HasCard())
                {
                    pairList[safeZones[i]].PlayCard(card);
                    return pairList[safeZones[i]].id;
                }
            }
        }

        if (card.cardType.Equals(CardType.Creature))
        {
            foreach (int orderIndex in creatureCardOrder)
            {
                if (!pairList[safeZones[orderIndex]].HasCard())
                {
                    pairList[safeZones[orderIndex]].PlayCard(card);
                    return pairList[orderIndex].id;
                }
            }
        }

        if (card.cardType.Equals(CardType.Pillar) || card.cardType.Equals(CardType.Artifact))
        {
            foreach (int orderIndex in permanentCardOrder)
            {
                if (!pairList[safeZones[orderIndex]].HasCard())
                {
                    if (stackCountList.Count > 0)
                    {
                        stackCountList[orderIndex]++;
                    }
                    pairList[safeZones[orderIndex]].PlayCard(card);
                    return pairList[orderIndex].id;
                }
            }
        }

        int loopBreak = 0;
        while (loopBreak < pairList.Count)
        {
            int rndIndex = Random.Range(0, pairList.Count);
            if (pairList[rndIndex].card == null)
            {
                pairList[rndIndex].card = card;
                return pairList[rndIndex].id;
            }
            else
            {
                loopBreak++;
            }

        }
        return null;
    }

    private ID CanStack(Card card)
    {
        foreach (IDCardPair item in pairList)
        {
            if (item.card != null)
            {
                if (item.card.cardName.Equals(card.cardName) && item.card.cardType.Equals(CardType.Pillar))
                {
                    return item.id;
                }
            }
        }
        return null;
    }

    public List<Card> GetAllCards()
    {
        List<Card> toReturn = new List<Card>();
        foreach (IDCardPair item in pairList)
        {
            if (item.card != null)
            {
                toReturn.Add(item.card);
            }
        }
        return toReturn;
    }

    public void ClearAllCards()
    {
        foreach (IDCardPair item in pairList)
        {
            item.RemoveCard();
        }
    }

    public Card GetCardWithID(ID id) => pairList[id.Index].card;

    public void DestroyCard(int index)
    {
        if(stackCountList.Count > 0)
        {
            stackCountList[index]--;
            if (stackCountList[index] > 0) { return; }
        }

        pairList[index].card = null;
    }

    //public void ChangePassiveAbility(PassiveEnum passiveToChange, bool newValue, ID target)
    //{
    //    Card creature = GetCardWithID(target);
    //    switch (passiveToChange)
    //    {
    //        case PassiveEnum.IsMaterial:
    //            creature.cardPassives.isImmaterial = newValue;
    //            break;
    //        case PassiveEnum.IsBurrowed:
    //            creature.cardPassives.isBurrowed = newValue;
    //            if (newValue)
    //            {
    //                creature.basePower = creature.power;
    //                creature.power = Mathf.FloorToInt(creature.power / 2);
    //            }
    //            else
    //            {
    //                creature.power = creature.basePower;
    //            }
    //            break;
    //        case PassiveEnum.WillSkeleton:
    //            creature.cardPassives.willSkeleton = newValue;
    //            break;
    //        case PassiveEnum.IsAirborne:
    //            creature.cardPassives.isAirborne = newValue;
    //            break;
    //        case PassiveEnum.HasReach:
    //            creature.cardPassives.hasReach = newValue;
    //            break;
    //        case PassiveEnum.HasMomentum:
    //            creature.cardPassives.hasMomentum = newValue;
    //            break;
    //        case PassiveEnum.IsAntimatter:
    //            creature.power = Mathf.Abs(creature.power);
    //            creature.power = newValue ? -creature.power : creature.power;
    //            creature.basePower = creature.power;
    //            creature.cardPassives.isAntimatter = newValue;
    //            break;
    //        case PassiveEnum.IsUndead:
    //            creature.cardPassives.isUndead = newValue;
    //            break;
    //        case PassiveEnum.IsMummy:
    //            creature.cardPassives.isMummy = newValue;
    //            break;
    //        case PassiveEnum.HasAflatoxin:
    //            creature.cardPassives.hasAflatoxin = newValue;
    //            break;
    //        case PassiveEnum.IsVenemous:
    //            creature.cardPassives.isVenemous = newValue;
    //            break;
    //        case PassiveEnum.IsPoisonous:
    //            creature.cardPassives.isPoisonous = newValue;
    //            break;
    //        case PassiveEnum.HasGravity:
    //            creature.cardPassives.hasGravity = newValue;
    //            break;
    //        case PassiveEnum.IsReflect:
    //            creature.cardPassives.isReflect = newValue;
    //            break;
    //        case PassiveEnum.IsMutant:
    //            creature.cardPassives.isMutant = newValue;
    //            break;
    //        case PassiveEnum.IsDiving:
    //            creature.cardPassives.isDiving = newValue;
    //            break;
    //        case PassiveEnum.IsVampire:
    //            creature.cardPassives.isVampire = newValue;
    //            break;
    //        case PassiveEnum.IsVoodoo:
    //            creature.cardPassives.isVoodoo = newValue;
    //            break;
    //        case PassiveEnum.IsPsion:
    //            creature.cardPassives.isPsion = newValue;
    //            break;
    //        case PassiveEnum.IsPheonix:
    //            creature.cardPassives.isPhoenix = newValue;
    //            break;
    //        case PassiveEnum.IsDeadlyVenemous:
    //            creature.cardPassives.isDeadlyVenemous = newValue;
    //            break;
    //        case PassiveEnum.hasAdrenaline:
    //            creature.cardPassives.hasAdrenaline = newValue;
    //            break;
    //        case PassiveEnum.isReady:
    //            creature.firstTurn = newValue;
    //            break;
    //        default:
    //            break;
    //    }
    //}
}
