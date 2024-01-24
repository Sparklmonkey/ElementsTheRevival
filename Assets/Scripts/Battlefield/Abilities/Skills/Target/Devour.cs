using System.Collections.Generic;
using UnityEngine;

public class Devour : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.OpHighAtk;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        BattleVars.Shared.AbilityCardOrigin.AtkModify++;
        BattleVars.Shared.AbilityCardOrigin.DefModify++;
        if (targetCard.innateSkills.Poisonous)
        {
            BattleVars.Shared.AbilityCardOrigin.Poison++;
        }
        EventBus<UpdateCreatureCardEvent>.Raise( new UpdateCreatureCardEvent(BattleVars.Shared.AbilityIDOrigin, BattleVars.Shared.AbilityCardOrigin, true));
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = enemy.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(Owner.playerCreatureField.GetAllValidCardIds());
        if (possibleTargets.Count == 0) { return new(); }
        if (!possibleTargets.Exists(x => x.IsTargetable() && x.Item2.DefNow <= Origin.DefNow)) { return new(); }
        return possibleTargets.FindAll(x => x.IsTargetable() && x.Item2.DefNow < Origin.DefNow);
    }
    
    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.cardType.Equals(CardType.Creature) && card.DefNow <= Origin.DefNow && card.IsTargetable();
    }
    
    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets)
    {
        if (possibleTargets.Count == 0) { return default; }

        var opCreatures = possibleTargets.FindAll(x => x.Item1.owner == OwnerEnum.Player && x.HasCard());

        return opCreatures.Count == 0 ? default : opCreatures[Random.Range(0, possibleTargets.Count)];
    }
}
