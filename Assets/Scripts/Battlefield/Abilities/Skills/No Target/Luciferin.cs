using System.Collections.Generic;

public class Luciferin : AbilityEffect
{
    public override bool NeedsTarget() => false;
    public override bool IsCardValid(ID id, Card card) => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        var cardList = Owner.playerCreatureField.GetAllValidCardIds();

        foreach (var pair in cardList)
        {
            if (pair.Item2.skill != "") continue;
            pair.Item2.passiveSkills.Light = true;
            pair.Item2.desc = "Bioluminescence : \n Each turn <sprite=3> is generated";
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(pair.Item1, pair.Item2, true));
        }
        EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(10, false, true, Owner.Owner));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy) => new List<(ID, Card)>();

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets) => default;
    public override TargetPriority GetPriority() => TargetPriority.Any;
}
