using System.Collections.Generic;
using System.Linq;

public class Acceleration : AbilityEffect
{
    public override bool NeedsTarget() => true;

    public override TargetPriority GetPriority() => TargetPriority.HighestHp;

    public override void Activate(IDCardPair target)
    {
        target.card.desc = "Acceleration: \n Gain +2 /-1 per turn";
        target.card.skill = "";
        target.card.passiveSkills.Acceleration = true;
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
            int currentScore = target.id.owner == Owner.playerID.id.owner ? 75 : 50;
            currentScore += target.card.AtkNow;
            currentScore += target.card.DefNow;

            currentScore += target.card.skill == "" ? 15 : 0;

            if (currentScore > score)
            {
                score = currentScore;
                currentTarget = target;
            }
        }

        return currentTarget;
    }
}