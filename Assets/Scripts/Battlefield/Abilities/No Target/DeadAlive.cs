using System.Collections.Generic;
using System.Linq;

public class DeadAlive : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        Game_AnimationManager.shared.StartAnimation("DeadAndAlive", target.transform);
        var tempCard = new Card(target.card);
        target.RemoveCard();
        target.PlayCard(tempCard);
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