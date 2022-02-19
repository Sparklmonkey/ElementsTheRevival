using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class CreatureManager : FieldManager
{
    public CreatureManager(List<ID> idList)
    {
        pairList = new List<IDCardPair>();
        for (int i = 0; i < idList.Count; i++)
        {
            pairList.Add(new IDCardPair(idList[i], null));
        }
    }

    public List<ID> GetCreaturesWithGravity()
    {
        List<ID> cards = GetAllIds();
        List<ID> listToReturn = new List<ID>();
        if(cards.Count > 0)
        {
            foreach (ID iD in cards)
            {
                Card creature = pairList[iD.Index].card;
                if (creature.cardPassives.hasGravity)
                {
                    listToReturn.Add(iD);
                }
            }
        }

        return listToReturn;
    }

    public (Card, int) DamageCreature(int amount, ID target)
    {
        Card creature = GetCardWithID(target);
        creature.hp -= amount;

        if(creature.hp > creature.maxHP) { creature.hp = creature.maxHP; }

        if(creature.hp > 0)
        {
            return (creature, -1);
        }
        return (null, Math.Abs(creature.hp));
    }

    public void ApplyCounter(CounterEnum counter, int amount, ID target)
    {
        Card creature = GetCardWithID(target);
        switch (counter)
        {
            case CounterEnum.Freeze:
                creature.cardCounters.freeze += amount;
                break;
            case CounterEnum.Poison:
                creature.cardCounters.poison += amount;
                break;
            case CounterEnum.Delay:
                creature.cardCounters.delay += amount;
                break;
            case CounterEnum.Purify:
                creature.cardCounters.purity += amount;
                break;
            case CounterEnum.Invisible:
                creature.cardCounters.invisibility += amount;
                break;
            case CounterEnum.Charge:
                creature.cardCounters.invisibility += amount;
                creature.power += amount;
                break;
            default:
                break;
        }
    }

    public void ModifyPowerHP(int modifyPower, int modifyHP, ID target)
    {
        Card creature = GetCardWithID(target);
        creature.power += modifyPower;
        creature.hp += modifyHP;
        creature.maxHP += modifyHP;
    }
}





