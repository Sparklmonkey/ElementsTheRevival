using System.Collections.Generic;
using UnityEngine;

public class Catapult : AbilityEffect
{
    public override bool NeedsTarget() => true;

    public override void Activate(IDCardPair target)
    {
        int damage = 100 * target.card.DefNow / (100 + target.card.DefNow);
        damage += target.card.Freeze > 0 ? Mathf.FloorToInt(damage * 0.5f) : 0;
        target.RemoveCard();

        DuelManager.GetNotIDOwner(target.id).ModifyHealthLogic(damage, true, false);
        return;
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerCreatureField.GetAllValidCardIds();
        return possibleTargets.FindAll(x => x.IsTargetable());
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        if (posibleTargets.Count == 0) { return null; }
        return posibleTargets[Random.Range(0, posibleTargets.Count)];
    }
}