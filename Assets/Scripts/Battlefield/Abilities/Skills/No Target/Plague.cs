using System.Collections.Generic;

public class Plague : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        var targetPlayer = DuelManager.GetNotIDOwner(target.id);
        var idList = targetPlayer.playerCreatureField.GetAllValidCardIds();

        foreach (var idCardi in idList)
        {
            if (idCardi.card.innateSkills.Immaterial)
            {
                continue;
            }

            if (idCardi.card.passiveSkills.Burrow)
            {
                continue;
            }

            idCardi.card.Poison += 1;
            idCardi.UpdateCard();
        }

        if (BattleVars.Shared.AbilityOrigin.card.cardType == CardType.Creature)
        {
            BattleVars.Shared.AbilityOrigin.RemoveCard();
        }
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        return new();
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> possibleTargets)
    {
        return null;
    }

    public override TargetPriority GetPriority() => TargetPriority.Any;
}
