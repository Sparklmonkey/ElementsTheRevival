using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Drainlife : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.OpHighAtk;

    public override void Activate(ID targetId, Card targetCard)
    {
        var quantaElement = Owner.GetAllQuantaOfElement(Element.Darkness);
        var damageToDeal = 2 + Mathf.FloorToInt(quantaElement / 10) * 2;

        if (targetCard is null)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(damageToDeal, true, true, targetId.owner));
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(damageToDeal, false, true, Owner.Owner));
            return;
        }

        var defPlaceHolder = targetCard.DefNow;
        targetCard.DefDamage += damageToDeal;
        var amountToHeal = targetCard.DefNow > 0 ? damageToDeal : defPlaceHolder;

        EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(amountToHeal, false, false, Owner.Owner));
        if (targetCard.DefNow > 0 && targetCard.innateSkills.Voodoo)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(damageToDeal, true, false, targetId.owner.Not()));
        }

        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(enemy.playerCreatureField.GetAllValidCardIds());
        possibleTargets.Add((enemy.playerID, null));
        possibleTargets.Add((Owner.playerID, null));
        return possibleTargets.Count == 0 ? new() : possibleTargets.FindAll(x => x.IsTargetable());
    }

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets)
    {
        if (possibleTargets.Count == 0) { return default; }

        var opCreatures = possibleTargets.FindAll(x => x.Item1.owner == OwnerEnum.Player && x.HasCard());

        return opCreatures.Count == 0 ? default : opCreatures[Random.Range(0, possibleTargets.Count)];
    }
}