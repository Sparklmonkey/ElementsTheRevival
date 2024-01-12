using System.Collections.Generic;
using System.Linq;

public class Plague : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        var targetPlayer = DuelManager.Instance.GetNotIDOwner(targetId);
        var idList = targetPlayer.playerCreatureField.GetAllValidCardIds();

        foreach (var pair in idList.Where(pair => !pair.Item2.innateSkills.Immaterial).Where(pair => !pair.Item2.passiveSkills.Burrow))
        {
            pair.Item2.Poison += 1;
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
        }
        
        if (targetCard.cardType.Equals(CardType.Creature))
        {
            EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
        }
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy) => new List<(ID, Card)>();

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets) => default;

    public override TargetPriority GetPriority() => TargetPriority.Any;
}
