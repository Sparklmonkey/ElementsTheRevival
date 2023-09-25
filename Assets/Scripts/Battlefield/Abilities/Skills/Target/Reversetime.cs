using System.Collections.Generic;
using UnityEngine;

public class Reversetime : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.HighestCost;

    public override void Activate(IDCardPair target)
    {
        if (target.card.innateSkills.Mummy)
        {
            target.PlayCard(CardDatabase.Instance.GetCardFromId(target.card.iD.IsUpgraded() ? "7qc" : "5rs"));
        }
        else if (target.card.innateSkills.Undead)
        {
            target.PlayCard(CardDatabase.Instance.GetRandomCard(CardType.Creature, target.card.iD.IsUpgraded(), true));
        }
        else
        {
            Card baseCreature = CardDatabase.Instance.GetCardFromId(target.card.iD);
            DuelManager.Instance.GetIDOwner(target.id).AddCardToDeck(baseCreature);
            target.RemoveCard();
        }
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

        return possibleTargets[Random.Range(0, possibleTargets.Count)];
    }
}