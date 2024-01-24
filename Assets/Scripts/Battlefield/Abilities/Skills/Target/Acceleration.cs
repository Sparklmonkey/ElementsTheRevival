using System.Collections.Generic;

public class Acceleration : AbilityEffect
{
    public override bool NeedsTarget() => true;

    public override TargetPriority GetPriority() => TargetPriority.HighestHp;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.desc = "Acceleration: \n Gain +2 /-1 per turn";
        targetCard.skill = "";
        targetCard.passiveSkills.Acceleration = true;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.cardType.Equals(CardType.Creature) && card.IsTargetable();
    }
    
    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(enemy.playerCreatureField.GetAllValidCardIds());
        return possibleTargets.Count == 0 ? new() : possibleTargets.FindAll(x => x.IsTargetable());
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
            var currentScore = target.Item1.owner == Owner.playerID.owner ? 75 : 50;
            currentScore += target.Item2.AtkNow;
            currentScore += target.Item2.DefNow;

            currentScore += target.Item2.skill == "" ? 15 : 0;

            if (currentScore > score)
            {
                score = currentScore;
                currentTarget = target;
            }
        }

        return currentTarget;
    }
}