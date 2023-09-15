using System.Collections.Generic;
using UnityEngine;

public class Devour : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.OpHighAtk;

    public override void Activate(IDCardPair target)
    {
        BattleVars.Shared.AbilityOrigin.card.AtkModify++;
        BattleVars.Shared.AbilityOrigin.card.DefModify++;
        if (target.card.innateSkills.Poisonous)
        {
            BattleVars.Shared.AbilityOrigin.card.Poison++;
        }
        BattleVars.Shared.AbilityOrigin.UpdateCard();
        target.RemoveCard();
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = enemy.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(Owner.playerCreatureField.GetAllValidCardIds());
        if (possibleTargets.Count == 0) { return new(); }
        if (!possibleTargets.Exists(x => x.IsTargetable() && x.card.DefNow <= Origin.card.DefNow)) { return new(); }
        return possibleTargets.FindAll(x => x.IsTargetable() && x.card.DefNow < Origin.card.DefNow);
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> possibleTargets)
    {
        if (possibleTargets.Count == 0) { return null; }

        var opCreatures = possibleTargets.FindAll(x => x.id.owner == OwnerEnum.Player && x.HasCard());

        if (opCreatures.Count == 0)
        {
            return null;
        }
        else
        {
            return opCreatures[Random.Range(0, possibleTargets.Count)];
        }
    }
}
