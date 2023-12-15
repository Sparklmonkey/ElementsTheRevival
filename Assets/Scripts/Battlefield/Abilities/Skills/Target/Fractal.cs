using System.Collections.Generic;

public class Fractal : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.SelfHighAtk;

    public override void Activate(IDCardPair target)
    {
        Owner.FillHandWith(CardDatabase.Instance.GetCardFromId(target.card.iD));
        EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(75, Element.Aether, Owner.isPlayer, false));
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(enemy.playerCreatureField.GetAllValidCardIds());
        if (possibleTargets.Count == 0)
        {
            return new();
        }

        return possibleTargets.FindAll(x => x.IsTargetable());
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> possibleTargets)
    {
        if (possibleTargets.Count == 0)
        {
            return null;
        }

        IDCardPair currentTarget = null;
        var score = 0;

        foreach (var target in possibleTargets)
        {
            var currentScore = 100;
            currentScore -= target.card.AtkNow;
            currentScore -= target.card.DefNow;

            currentScore -= target.card.skill == "" ? 30 : 0;
            currentScore += target.card.cost * 10;

            if (currentScore > score)
            {
                score = currentScore;
                currentTarget = target;
            }
        }

        return currentTarget;
    }
}