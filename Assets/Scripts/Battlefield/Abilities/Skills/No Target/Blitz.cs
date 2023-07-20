using System.Collections.Generic;
using System.Linq;

public class Blitz : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        Owner.SpendQuantaLogic(Element.Air, 75);
        var idCardList = Owner.playerCreatureField.GetAllValidCardIds();
        foreach (var idCardi in idCardList)
        {
            if (idCardi.card.innateSkills.Airborne)
            {
                Game_AnimationManager.shared.StartAnimation("Dive", target.transform);
                idCardi.card.passiveSkills.Dive = true;
                target.card.AtkModify *= 2;
                target.card.atk *= 2;
                idCardi.UpdateCard();
            }
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
