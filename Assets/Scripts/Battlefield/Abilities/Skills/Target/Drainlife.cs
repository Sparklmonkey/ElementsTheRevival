using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Drainlife : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.OpHighAtk;

    public override void Activate(IDCardPair target)
    {
        int quantaElement = Owner.GetAllQuantaOfElement(Element.Darkness);
        int damageToDeal = 2 + (Mathf.FloorToInt(quantaElement / 10) * 2);

        if (!target.HasCard())
        {
            DuelManager.GetIDOwner(target.id).ModifyHealthLogic(damageToDeal, true, true);
            Owner.ModifyHealthLogic(damageToDeal, false, false);
            return;
        }

        int defPlaceHolder = target.card.DefNow;
        target.card.DefDamage += damageToDeal;
        var amountToHeal = target.card.DefNow > 0 ? damageToDeal : defPlaceHolder;


        Owner.ModifyHealthLogic(amountToHeal, false, false);
        if (target.card.DefNow > 0 && target.card.innateSkills.Voodoo)
        {
            Owner.ModifyHealthLogic(amountToHeal, true, false);
        }

        target.UpdateCard();
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(enemy.playerCreatureField.GetAllValidCardIds());
        possibleTargets.Add(enemy.playerID);
        possibleTargets.Add(Owner.playerID);
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

        var opCreatures = possibleTargets.FindAll(x => x.id.owner == OwnerEnum.Player && x.HasCard());

        if (opCreatures.Count == 0)
        {
            return possibleTargets.Find(x => x.id.owner == OwnerEnum.Player);
        }
        else
        {
            return opCreatures.Aggregate((i1, i2) => i1.card.AtkNow >= i2.card.AtkNow ? i1 : i2);
        }
    }
}