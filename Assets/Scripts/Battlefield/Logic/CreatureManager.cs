using System;
using System.Collections.Generic;
using System.Linq;
using Elements.Duel.Visual;
using UnityEngine;

[Serializable]
public class CreatureManager : FieldManager
{
    private readonly List<int> creatureCardOrder = new (){ 11, 13, 9, 10, 12, 14, 8, 16, 18, 20, 22, 0, 2, 4, 6, 15, 17, 19, 21, 1, 3, 5, 7 };
    private readonly List<int> safeZones = new (){ 11, 13, 10, 12, 14 };

    public List<ID> GetCreaturesWithGravity()
    {
        List<ID> cards = GetAllIds();
        List<ID> listToReturn = new ();
        if (cards.Count > 0)
        {
            foreach (ID iD in cards)
            {
                Card creature = pairList[iD.Index].card;
                if (creature.passive.Contains("gravity pull"))
                {
                    listToReturn.Add(iD);
                }
            }
        }

        return listToReturn;
    }

    public void CreatureTurnDown()
    {
        foreach (var idCard in pairList)
        {
            if (idCard.HasCard())
            {
                idCard.card.AbilityUsed = false;
                idCard.UpdateCard();
            }
        }

    }

    public ID PlayCreature(Card card)
    {
        if (DuelManager.floodCount > 0 && !card.costElement.Equals(Element.Other) && !card.costElement.Equals(Element.Water))
        {

            foreach (int orderIndex in safeZones)
            {
                if (!pairList[orderIndex].HasCard())
                {
                    pairList[orderIndex].PlayCard(card);
                    return pairList[orderIndex].id;
                }
            }
        }

        foreach (int orderIndex in creatureCardOrder)
        {
            if (!pairList[orderIndex].HasCard())
            {
                pairList[orderIndex].PlayCard(card);
                return pairList[orderIndex].id;
            }

        }
        return null;
    }


    public Card DamageCreature(int amount, ID target)
    {
        Card creature = GetCardWithID(target);
        if (creature == null) { return null; }
        creature.DefDamage += amount;
        if (creature.DefDamage < 0) { creature.DefDamage = 0; }
        if (creature.DefNow > 0)
        {
            return creature;
        }
        return null;
    }

    public void ApplyCounter(CounterEnum counter, int amount, ID target)
    {
        Card creature = GetCardWithID(target);
        switch (counter)
        {
            case CounterEnum.Freeze:
                creature.Freeze += amount;
                break;
            case CounterEnum.Poison:
                if (amount == -1)
                {
                    if (creature.Poison > 0) { creature.Poison = 0; }
                    creature.Poison -= 2;
                    creature.IsAflatoxin = false;
                }
                else
                {
                    creature.Poison += amount;
                }
                break;
            case CounterEnum.Delay:
                for (int i = 0; i < amount; i++)
                {
                    if (amount > 0)
                    {
                        creature.innate.Add("delay");
                    }
                    else
                    {
                        creature.innate.Remove("delay");
                    }
                }
                break;
            case CounterEnum.Invisible:
                //creature.cardCounters.invisibility += amount;
                break;
            case CounterEnum.Charge:
                creature.Charge += amount;
                creature.AtkModify += amount;
                break;
            default:
                break;
        }
    }

    public void ModifyPowerHP(int modifyPower, int modifyHP, ID target, bool isModifyPerm)
    {
        Card creature = GetCardWithID(target);
        if (isModifyPerm)
        {
            creature.def += modifyHP;
            creature.atk += modifyPower;
            return;
        }
        creature.AtkModify += modifyPower;
        creature.DefModify += modifyHP;
    }
}
