using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mutation : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(targetId, "Mutation", Element.Other));
        switch (GetMutationResult())
        {
            case MutationEnum.Kill:
                EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
                return;
            case MutationEnum.Mutate:
                EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, CardDatabase.Instance.GetMutant(targetCard.iD.IsUpgraded()), false));
                break;
            default:
                EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, CardDatabase.Instance.GetCardFromId(targetCard.iD.IsUpgraded() ? "6tu" : "4ve"), false));
                break;
        }
    }

    private MutationEnum GetMutationResult()
    {
        var num = Random.Range(0, 100);
        return num switch
        {
            >= 90 => MutationEnum.Kill,
            >= 50 => MutationEnum.Mutate,
            _ => MutationEnum.Abomination
        };
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.cardType.Equals(CardType.Creature) && card.IsTargetable();
    }
}