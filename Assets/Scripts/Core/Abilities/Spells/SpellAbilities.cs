using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//MARK: TODO
public class SpellShardIntegrity : ISkill
{

    public bool isTargetFixed => true;

    public IEnumerator ActivateAbility(ID iD)
    {
        List<int> atkModifier = new List<int> { 2, 2, 0, 1, 2, 3, 2, 2, 2, 2, 2, 2, 1 };
        List<int> hpModifier = new List<int> { 2, 2, 6, 4, 2, 0, 2, 2, 2, 2, 2, 2, 1 };
        PlayerManager player = DuelManager.GetIDOwner(BattleVars.shared.originId);
        List<Card> cardsInHand = player.GetHandCards();

        int golemAtk = 1;
        int golemHp = 4;
        Element lastElement = Element.Other;
        List<QuantaObject> shardElementCounter = new List<QuantaObject>();
        foreach (Card handCard in cardsInHand)
        {
            if (handCard.cardName.Contains("Shard of"))
            {
                int? index = shardElementCounter.ContainsElement(handCard.costElement);
                if (index != null)
                {
                    shardElementCounter[(int)index].count++;
                }
                else
                {
                    shardElementCounter.Add(new QuantaObject(handCard.costElement, 1));
                }
                lastElement = handCard.costElement;
                golemAtk += atkModifier[(int)handCard.costElement];
                golemHp += hpModifier[(int)handCard.costElement];
            }
            yield return null;
        }
        Card golem = CardDatabase.GetCardFromId("597");
        if (lastElement == Element.Other)
        {
            player.PlayCardOnFieldLogic(golem);
            yield break;
        }
        QuantaObject lastQuantaObject = new QuantaObject(Element.Other, 1);
        foreach (QuantaObject item in shardElementCounter)
        {
            if (item.element == lastElement)
            {
                lastQuantaObject = item;
            }
            yield return null;
        }
        player.PlayCardOnFieldLogic(CardDatabase.GetGolemAbility(lastQuantaObject, golem));
        yield return null;

    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature);
    }
}
public class SpellEliteShardIntegrity : ISkill
{
    public bool isTargetFixed => true;

    public IEnumerator ActivateAbility(ID iD)
    {
        List<int> atkModifier = new List<int> { 2, 2, 0, 1, 2, 3, 2, 2, 2, 2, 2, 2, 1 };
        List<int> hpModifier = new List<int> { 2, 2, 6, 4, 2, 0, 2, 2, 2, 2, 2, 2, 1 };
        PlayerManager player = DuelManager.GetIDOwner(BattleVars.shared.originId);
        List<Card> cardsInHand = player.GetHandCards();

        int golemAtk = 2;
        int golemHp = 5;
        Element lastElement = Element.Other;
        List<QuantaObject> shardElementCounter = new List<QuantaObject>();
        foreach (Card handCard in cardsInHand)
        {
            if (handCard.cardName.Contains("Shard of"))
            {
                int? index = shardElementCounter.ContainsElement(handCard.costElement);
                if (index != null)
                {
                    shardElementCounter[(int)index].count++;
                }
                else
                {
                    shardElementCounter.Add(new QuantaObject(handCard.costElement, 1));
                }
                lastElement = handCard.costElement;
                golemAtk += atkModifier[(int)handCard.costElement];
                golemHp += hpModifier[(int)handCard.costElement];
            }
            yield return null;
        }
        Card golem = CardDatabase.GetCardFromId("597");
        if (lastElement == Element.Other)
        {
            player.PlayCardOnFieldLogic(golem);
            yield break;
        }
        QuantaObject lastQuantaObject = new QuantaObject(Element.Other, 1);
        foreach (QuantaObject item in shardElementCounter)
        {
            if (item.element == lastElement)
            {
                lastQuantaObject = item;
            }
            yield return null;
        }
        player.PlayCardOnFieldLogic(CardDatabase.GetGolemAbility(lastQuantaObject, golem));

        yield return null;
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature);
    }
}