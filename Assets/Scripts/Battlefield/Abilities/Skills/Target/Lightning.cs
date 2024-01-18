using System.Collections.Generic;
using System.Linq;

public class Lightning : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.OpHighAtk;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (targetCard is null)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(5, true, true, targetId.owner));
            return;
        }

        targetCard.DefDamage += 5;
        if (targetCard.DefNow > 0 && targetCard.innateSkills.Voodoo)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(5, true, false, targetId.owner.Not()));
        }

        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = enemy.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(Owner.playerCreatureField.GetAllValidCardIds());
        possibleTargets.Add((enemy.playerID, null));
        possibleTargets.Add((Owner.playerID, null));

        if (possibleTargets.Count == 0)
        {
            return new();
        }

        return possibleTargets.FindAll(x => x.IsTargetable());
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
        if (possibleTargets.Count == 0)
        {
            return default;
        }

        var opCreatures = possibleTargets.FindAll(x => x.Item1.owner == OwnerEnum.Player && x.HasCard());

        return opCreatures.Count == 0 ? possibleTargets.Find(x => x.Item1.owner == OwnerEnum.Player) : opCreatures.Aggregate((i1, i2) => i1.Item2.AtkNow >= i2.Item2.AtkNow ? i1 : i2);
    }
}