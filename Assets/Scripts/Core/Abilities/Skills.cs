using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class SkillBase
{
    public readonly PlayerManager player = DuelManager.Instance.player;
    public readonly PlayerManager enemy = DuelManager.Instance.enemy;
    public virtual List<ID> SetupValidTargets()
    {
        return new();
    }

    public virtual IEnumerator ActivateAbility(bool targetIsPlayer, Card targetCard, ID targetId)
    {
        yield break;
    }
}


public class Duality : SkillBase
{
    public override IEnumerator ActivateAbility(bool targetIsPlayer, Card targetCard, ID targetId)
    {
        if (targetIsPlayer)
        {
            Card cardToAdd = enemy.deckManager.GetTopCard();
            if (cardToAdd == null) { yield break; }
            player.playerHand.AddCardToHand(new(cardToAdd));
        }
        else
        {
            Card cardToAdd = player.deckManager.GetTopCard();
            if (cardToAdd == null) { yield break; }
            enemy.playerHand.AddCardToHand(new(cardToAdd));
        }
    }
}


public class Nightmare : SkillBase
{
    public override IEnumerator ActivateAbility(bool targetIsPlayer, Card targetCard, ID targetId)
    {
        Card creature = CardDatabase.Instance.GetCardFromId(targetCard.iD);
        if (targetIsPlayer)
        {
            int damage = 7 - enemy.GetHandCards().Count;
            enemy.FillHandWith(creature);
            enemy.ModifyHealthLogic(damage * 2, true, true);
            player.ModifyHealthLogic(damage * 2, false, true);
        }
        else
        {
            int damage = 7 - player.GetHandCards().Count;
            player.FillHandWith(creature);
            enemy.ModifyHealthLogic(damage * 2, false, true);
            player.ModifyHealthLogic(damage * 2, true, true);
        }
    }
    public override List<ID> SetupValidTargets()
    {
        List<ID> permIds = player.playerCreatureField.GetAllIds();
        List<Card> permCards = player.playerCreatureField.GetAllCards();

        List<ID> returnList = new();

        if (player.playerCounters.invisibility == 0)
        {
            for (int i = 0; i < permCards.Count; i++)
            {
                if (!permCards[i].innate.Contains("immaterial") && !permCards[i].innate.Contains("burrow"))
                {
                    player.creatureDisplayers[permIds[i].Index].ShouldShowTarget(true);
                    returnList.Add(permIds[i]);
                }
            }
        }
        permIds = enemy.playerCreatureField.GetAllIds();
        permCards = enemy.playerCreatureField.GetAllCards();

        if (enemy.playerCounters.invisibility == 0)
        {
            for (int i = 0; i < permCards.Count; i++)
            {
                if (!permCards[i].innate.Contains("immaterial") && !permCards[i].innate.Contains("burrow"))
                {
                    enemy.creatureDisplayers[permIds[i].Index].ShouldShowTarget(true);
                    returnList.Add(permIds[i]);
                }
            }
        }

        return returnList;
    }

}