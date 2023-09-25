using System.Collections.Generic;
using UnityEngine;

public class Shockwave : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.IsFrozen;

    public override void Activate(IDCardPair target)
    {
        if (target.card.Freeze > 0)
        {
            target.RemoveCard();
            return;
        }

        target.card.DefDamage += 4;
        if (target.card.DefNow > 0 && target.card.innateSkills.Voodoo)
        {
            DuelManager.Instance.GetNotIDOwner(target.id).ModifyHealthLogic(4, true, false);
        }

        target.UpdateCard();
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(enemy.playerCreatureField.GetAllValidCardIds());
        if (possibleTargets.Count == 0)
        {
            return new();
        }

        return possibleTargets.FindAll(x => x.IsTargetable());
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> possibleTargets)
    {
        if (possibleTargets.Count == 0)
        {
            return null;
        }

        return possibleTargets[Random.Range(0, possibleTargets.Count)];
    }
}