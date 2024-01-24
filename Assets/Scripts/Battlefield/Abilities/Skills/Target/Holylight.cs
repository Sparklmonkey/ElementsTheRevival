using System.Collections.Generic;
using UnityEngine;

public class Holylight : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.LowestHp;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        if (targetCard is null)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(10, false, false, targetId.owner));
            return;
        }

        var damage = targetCard.costElement.Equals(Element.Death) || targetCard.costElement.Equals(Element.Darkness)
            ? -10
            : 10;
        targetCard.DefDamage -= damage;
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

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null)
        {
            return id.field.Equals(FieldEnum.Player);
        }
        return card.cardType.Equals(CardType.Creature) && card.IsTargetable();
    }

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets)
    {
        return possibleTargets.Count == 0 ? default : possibleTargets[Random.Range(0, possibleTargets.Count)];
    }
}