using System.Collections.Generic;

public class Fractal : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.SelfHighAtk;

    public override void Activate(ID targetId, Card targetCard)
    {
        Owner.FillHandWith(CardDatabase.Instance.GetCardFromId(targetCard.iD));
        EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(75, Element.Aether, Owner.Owner, false));
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

        (ID, Card) currentTarget = default;
        var score = 0;

        foreach (var target in possibleTargets)
        {
            var currentScore = 100;
            currentScore -= target.Item2.AtkNow;
            currentScore -= target.Item2.DefNow;

            currentScore -= target.Item2.skill == "" ? 30 : 0;
            currentScore += target.Item2.cost * 10;

            if (currentScore > score)
            {
                score = currentScore;
                currentTarget = target;
            }
        }

        return currentTarget;
    }
}