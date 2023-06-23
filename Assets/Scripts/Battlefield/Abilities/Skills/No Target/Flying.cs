using System.Collections.Generic;
using System.Linq;

public class Flying : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        Card weapon = new(Owner.playerPassiveManager.GetWeapon().card);
        if (weapon.iD == "4t2") { return; }
        weapon.cardType = CardType.Creature;
        Owner.PlayCardOnFieldLogic(weapon);
        Owner.playerPassiveManager.RemoveWeapon();
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
