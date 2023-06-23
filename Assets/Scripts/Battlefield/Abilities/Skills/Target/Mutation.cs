using System.Collections.Generic;
using UnityEngine;

public class Mutation : AbilityEffect
{
    public override bool NeedsTarget() => true;

    public override void Activate(IDCardPair target)
    {
        Game_AnimationManager.shared.StartAnimation("Mutation", target.transform);
        switch (GetMutationResult())
        {
            case MutationEnum.Kill:
                target.RemoveCard();
                return;
            case MutationEnum.Mutate:
                target.PlayCard(CardDatabase.Instance.GetMutant(target.card.iD.IsUpgraded()));
                break;
            default:
                target.PlayCard(CardDatabase.Instance.GetCardFromId(target.card.iD.IsUpgraded() ? "6tu" : "4ve"));
                break;
        }
    }

    private MutationEnum GetMutationResult()
    {
        int num = Random.Range(0, 100);
        if (num >= 90)
        {
            return MutationEnum.Kill;
        }
        else if (num >= 50)
        {
            return MutationEnum.Mutate;
        }
        else
        {
            return MutationEnum.Abomination;
        }

    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(enemy.playerCreatureField.GetAllValidCardIds());
        return possibleTargets.FindAll(x => x.IsTargetable());
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        if (posibleTargets.Count == 0) { return null; }
        return posibleTargets[Random.Range(0, posibleTargets.Count)];
    }
}