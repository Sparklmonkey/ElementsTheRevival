using System.Collections.Generic;
using System.Linq;

public class Bravery : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        int cardToDraw = Owner.playerPassiveManager.GetMark().card.costElement.Equals(Element.Fire) ? 3 : 2;
        for (int i = 0; i < cardToDraw; i++)
        {
            DuelManager.Instance.player.DrawCardFromDeckLogic();
            DuelManager.Instance.enemy.DrawCardFromDeckLogic();
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
