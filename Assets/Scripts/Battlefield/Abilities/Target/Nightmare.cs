using System.Collections.Generic;
using UnityEngine;

public class Nightmare : AbilityEffect
{
    public override bool NeedsTarget() => true;

    public override void Activate(IDCardPair target)
    {
        PlayerManager opponent = DuelManager.GetNotIDOwner(Owner.playerID.id);
        Card creature = CardDatabase.Instance.GetCardFromId(target.card.iD);

        int damage = 7 - opponent.GetHandCards().Count;
        opponent.FillHandWith(creature);
        opponent.ModifyHealthLogic(damage * 2, true, true);
        Owner.ModifyHealthLogic(damage * 2, false, true);
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(enemy.playerCreatureField.GetAllValidCardIds());
        return possibleTargets.FindAll(x => x.card.IsTargetable());
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        if (posibleTargets.Count == 0) { return null; }
        return posibleTargets[Random.Range(0, posibleTargets.Count)];
    }
}