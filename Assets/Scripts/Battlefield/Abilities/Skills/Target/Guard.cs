using System.Collections.Generic;
using UnityEngine;

public class Guard : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.OpHighAtk;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.innateSkills.Delay++;
        BattleVars.Shared.AbilityCardOrigin.innateSkills.Delay++;
        if (!targetCard.innateSkills.Airborne)
        {
            targetCard.DefDamage += BattleVars.Shared.AbilityCardOrigin.AtkNow;
        }
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(BattleVars.Shared.AbilityIDOrigin, BattleVars.Shared.AbilityCardOrigin, true));
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
        if (possibleTargets.Count == 0) { return default; }

        var opCreatures = possibleTargets.FindAll(x => x.Item1.owner == OwnerEnum.Player && x.HasCard());

        return opCreatures.Count == 0 ? default : opCreatures[Random.Range(0, possibleTargets.Count)];
    }
}