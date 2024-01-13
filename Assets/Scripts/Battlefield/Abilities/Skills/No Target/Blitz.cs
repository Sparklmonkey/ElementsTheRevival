using System.Collections.Generic;
using System.Linq;

public class Blitz : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(75, Element.Air, Owner.Owner, false));
        var validCardList = Owner.playerCreatureField.GetAllValidCardIds();
        foreach (var pair in validCardList.Where(idCardPair => idCardPair.Item2.innateSkills.Airborne))
        {
            EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(pair.id, "Dive", Element.Other));
            pair.Item2.passiveSkills.Dive = true;
            targetCard.AtkModify *= 2;
            targetCard.atk *= 2;
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(pair.Item1, pair.Item2, true));
        }
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy) => new List<(ID, Card)>();

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets) => default;
    public override TargetPriority GetPriority() => TargetPriority.Any;
}
