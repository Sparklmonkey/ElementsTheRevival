using System.Collections.Generic;
using System.Linq;

public class Plague : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        var targetPlayer = DuelManager.GetNotIDOwner(target.id);
        var idList = targetPlayer.playerCreatureField.GetAllValidCardIds();

        foreach (var idCardi in idList)
        {
            idCardi.card.Poison += 1;
            idCardi.UpdateCard();
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
