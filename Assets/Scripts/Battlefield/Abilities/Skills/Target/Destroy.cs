using System.Collections.Generic;
using UnityEngine;

public class Destroy : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.Permanent;

    public override void Activate(IDCardPair target)
    {
        EventBus<OnCardRemovedEvent>.Raise(new OnCardRemovedEvent(target.id));
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = enemy.playerPermanentManager.GetAllValidCardIds();
        possibleTargets.AddRange(Owner.playerPermanentManager.GetAllValidCardIds());
        if (enemy.playerPassiveManager.GetWeapon().HasCard())
        {
            possibleTargets.Add(enemy.playerPassiveManager.GetWeapon());
        }
        if (Owner.playerPassiveManager.GetWeapon().HasCard())
        {
            possibleTargets.Add(Owner.playerPassiveManager.GetWeapon());
        }
        if (enemy.playerPassiveManager.GetShield().HasCard())
        {
            possibleTargets.Add(enemy.playerPassiveManager.GetShield());
        }
        if (Owner.playerPassiveManager.GetShield().HasCard())
        {
            possibleTargets.Add(Owner.playerPassiveManager.GetShield());
        }

        if (possibleTargets.Count == 0) { return new(); }
        return possibleTargets.FindAll(x => x.IsTargetable());
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