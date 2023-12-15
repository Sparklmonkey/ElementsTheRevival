using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Firebolt : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.OpHighAtk;

    public override void Activate(IDCardPair target)
    {
        var quantaElement = Owner.GetAllQuantaOfElement(Element.Fire);
        var damageToDeal = 3 + Mathf.FloorToInt(quantaElement / 10) * 3;

        if (!target.HasCard())
        {
            DuelManager.Instance.GetIDOwner(target.id).ModifyHealthLogic(damageToDeal, true, true);
            return;
        }

        target.card.DefDamage += damageToDeal;
        if (target.card.DefNow > 0 && target.card.innateSkills.Voodoo)
        {
            DuelManager.Instance.GetNotIDOwner(target.id).ModifyHealthLogic(target.card.DefNow < damageToDeal ? target.card.DefNow : damageToDeal, true, false);
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