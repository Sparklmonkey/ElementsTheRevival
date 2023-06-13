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
            if (idCardi.card.innate.Contains("airborne"))
            {
                Game_AnimationManager.shared.StartAnimation("Dive", target.transform);
                idCardi.card.passive.Add("dive");
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
