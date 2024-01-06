using System.Collections.Generic;
using UnityEngine;

public class Beserk : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.LowestHp;

    public override void Activate(ID targetId, Card targetCard)
    {
        targetCard.AtkModify += 6;
        targetCard.DefModify -= 6;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(enemy.playerCreatureField.GetAllValidCardIds());
        if (possibleTargets.Count == 0) { return new(); }
        return possibleTargets.FindAll(x => x.IsTargetable());
    }

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets)
    {
        if (possibleTargets.Count == 0) { return default; }

        var opCreatures = possibleTargets.FindAll(x => x.Item1.owner == OwnerEnum.Opponent && x.HasCard());

        
        return opCreatures.Count == 0 ? default : opCreatures[Random.Range(0, possibleTargets.Count)];
    }
}