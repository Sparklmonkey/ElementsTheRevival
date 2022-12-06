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
                if (creature.passive.Contains("gravity pull"))
                {
                    listToReturn.Add(iD);
                }
            }
        }

        return listToReturn;
    }

    public Card DamageCreature(int amount, ID target)
    {
        Card creature = GetCardWithID(target);
        if(creature == null) { return null; }
        creature.DefDamage -= amount;
        if(creature.DefDamage < 0) { creature.DefDamage = 0; }
        if(creature.DefNow > 0)
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
                    if(amount > 0)
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
                creature.DefModify += amount;
                creature.AtkModify += amount;
                break;
            default:
                break;
        }
    }

    public void ModifyPowerHP(int modifyPower, int modifyHP, ID target, bool isModifyPerm)
    {
        Card creature = GetCardWithID(target);
        creature.AtkModify += modifyPower;
        creature.DefModify += modifyHP;
        if (isModifyPerm)
        {
            creature.def += modifyHP;
            creature.atk += modifyPower;
        }
    }
}





