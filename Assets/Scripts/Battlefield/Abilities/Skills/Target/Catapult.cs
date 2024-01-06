using System.Collections.Generic;
using UnityEngine;

public class Catapult : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.HighestHp;

    public override void Activate(ID targetId, Card targetCard)
    {
        var damage = 100 * targetCard.DefNow / (100 + targetCard.DefNow);
        damage += targetCard.Freeze > 0 ? Mathf.FloorToInt(damage * 0.5f) : 0;
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
        
        EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(damage, true, false, targetId.owner.Not()));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerCreatureField.GetAllValidCardIds();
        if (possibleTargets.Count == 0) { return new(); }
        return possibleTargets.FindAll(x => x.IsTargetable());
    }

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets)
    {
        return possibleTargets.Count == 0 ? default : possibleTargets[Random.Range(0, possibleTargets.Count)];
    }
}