using System.Collections.Generic;

public class Ignite : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        target.RemoveCard();

        DuelManager.Instance.GetNotIDOwner(target.id).ModifyHealthLogic(20, true, false);

        var idList = DuelManager.Instance.GetNotIDOwner(target.id).playerCreatureField.GetAllValidCardIds();

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

    public override IDCardPair SelectRandomTarget(List<IDCardPair> possibleTargets)
    {
        return null;
    }
    public override TargetPriority GetPriority() => TargetPriority.Any;
}
