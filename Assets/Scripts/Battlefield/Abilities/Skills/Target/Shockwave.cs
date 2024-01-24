using System.Collections.Generic;
using UnityEngine;

public class Shockwave : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.IsFrozen;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        if (targetCard.Freeze > 0)
        {
            EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
            return;
        }

        targetCard.DefDamage += 4;
        if (targetCard.DefNow > 0 && targetCard.innateSkills.Voodoo)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(4, true, false, targetId.owner.Not()));
        }

        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(enemy.playerCreatureField.GetAllValidCardIds());
        if (possibleTargets.Count == 0)
        {
            return new();
        }

        return possibleTargets.FindAll(x => x.IsTargetable());
    }


    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.cardType.Equals(CardType.Creature) && card.IsTargetable();
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