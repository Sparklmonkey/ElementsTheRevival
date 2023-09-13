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
            if (idCardi.card.innateSkills.Immaterial) { continue; }
            if (idCardi.card.passiveSkills.Burrow) { continue; }
            idCardi.card.Poison += 1;
            idCardi.UpdateCard();
        }

        if (BattleVars.shared.abilityOrigin.card.cardType == CardType.Creature)
        {
            BattleVars.shared.abilityOrigin.RemoveCard();
        }
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        return new();
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        return null;
    }
}
