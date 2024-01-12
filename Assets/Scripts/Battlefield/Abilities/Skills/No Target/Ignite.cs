using System.Collections.Generic;
using System.Linq;

public class Ignite : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));

        EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(20, true, false, targetId.owner.Not()));

        var cardList = DuelManager.Instance.GetNotIDOwner(targetId).playerCreatureField.GetAllValidCardIds();

        foreach (var pair in cardList.Where(pair => !pair.Item2.innateSkills.Immaterial).Where(pair => !pair.Item2.passiveSkills.Burrow))
        {
            pair.Item2.DefDamage += 1;
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(pair.Item1, pair.Item2, true));
        }

        cardList = Owner.playerCreatureField.GetAllValidCardIds();

        foreach (var pair in cardList.Where(pair => !pair.Item2.innateSkills.Immaterial).Where(pair => !pair.Item2.passiveSkills.Burrow))
        {
            pair.Item2.DefDamage += 1;
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(pair.Item1, pair.Item2, true));
        }
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy) => new List<(ID, Card)>();

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets) => default;
    public override TargetPriority GetPriority() => TargetPriority.Any;
}
