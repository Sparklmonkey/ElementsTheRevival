using System.Collections.Generic;
using UnityEngine;

public class Steal : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.Permanent;

    public override void Activate(ID targetId, Card targetCard)
    {
        EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(new(targetCard), targetId.owner.Equals(OwnerEnum.Player)));
        
        switch (targetCard.cardType)
        {
            case CardType.Artifact:
            case CardType.Pillar:
                EventBus<PlayPermanentOnFieldEvent>.Raise(new PlayPermanentOnFieldEvent(Owner.Owner, new(targetCard)));
                break;
            case CardType.Weapon:
            case CardType.Shield:
            case CardType.Mark:
                EventBus<PlayPassiveOnFieldEvent>.Raise(new PlayPassiveOnFieldEvent(Owner.Owner, new(targetCard)));
                break;
        }
        // AnimationManager.Instance.StartAnimation("Steal", target.transform);
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = enemy.playerPermanentManager.GetAllValidCardIds();
        if (enemy.playerPassiveManager.GetWeapon().HasCard())
        {
            possibleTargets.Add(enemy.playerPassiveManager.GetWeapon());
        }

        if (enemy.playerPassiveManager.GetShield().HasCard())
        {
            possibleTargets.Add(enemy.playerPassiveManager.GetShield());
        }

        if (possibleTargets.Count == 0)
        {
            return new();
        }

        return possibleTargets.FindAll(x => x.IsTargetable());
    }

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets)
    {
        if (possibleTargets.Count == 0)
        {
            return default;
        }

        return possibleTargets[Random.Range(0, possibleTargets.Count)];
    }
}