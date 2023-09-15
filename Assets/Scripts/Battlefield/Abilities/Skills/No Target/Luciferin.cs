using System.Collections.Generic;

public class Luciferin : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        var idList = Owner.playerCreatureField.GetAllValidCardIds();

        foreach (var idCardi in idList)
        {
            if (idCardi.card.skill == "")
            {
                idCardi.card.passiveSkills.Light = true;
                idCardi.card.desc = "Bioluminescence : \n Each turn <sprite=3> is generated";
                idCardi.UpdateCard();
            }
        }
        Owner.ModifyHealthLogic(10, false, false);
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
