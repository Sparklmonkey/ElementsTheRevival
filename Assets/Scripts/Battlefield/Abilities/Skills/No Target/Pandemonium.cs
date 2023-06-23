using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pandemonium : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        var idList = DuelManager.Instance.player.playerCreatureField.GetAllValidCardIds();

        foreach (var idCardi in idList)
        {
            var chaos = new Chaos { Owner = Owner };
            chaos.Activate(idCardi);
        }

        idList = DuelManager.Instance.enemy.playerCreatureField.GetAllValidCardIds();

        foreach (var idCardi in idList)
        {
            var chaos = new Chaos { Owner = Owner };
            chaos.Activate(idCardi);
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
