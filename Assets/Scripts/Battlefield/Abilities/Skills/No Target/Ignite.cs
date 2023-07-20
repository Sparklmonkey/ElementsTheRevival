using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ignite : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        target.RemoveCard();

        DuelManager.GetNotIDOwner(target.id).ModifyHealthLogic(20, true, false);

        var idList = DuelManager.GetNotIDOwner(target.id).playerCreatureField.GetAllValidCardIds();

        foreach (var idCardi in idList)
        {
            if (idCardi.card.innateSkills.Immaterial) { continue; }
            if (idCardi.card.passiveSkills.Burrow) { continue; }
            idCardi.card.DefDamage += 1;
            idCardi.UpdateCard();
        }

        idList = Owner.playerCreatureField.GetAllValidCardIds();

        foreach (var idCardi in idList)
        {
            if (idCardi.card.innateSkills.Immaterial) { continue; }
            if (idCardi.card.passiveSkills.Burrow) { continue; }
            idCardi.card.DefDamage += 1;
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
