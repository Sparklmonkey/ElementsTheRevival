using System.Collections.Generic;
using System.Linq;

public class Blackhole : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        var victim = Owner.isPlayer ? DuelManager.Instance.enemy : DuelManager.Instance.player;
        int hpToRestore = 0;

        for (int i = 0; i < 12; i++)
        {
            if (victim.HasSufficientQuanta((Element)i, 3))
            {
                victim.SpendQuantaLogic((Element)i, 3);
                hpToRestore += 3;
            }
            else if (victim.HasSufficientQuanta((Element)i, 2))
            {
                victim.SpendQuantaLogic((Element)i, 2);
                hpToRestore += 2;
            }
            else if (victim.HasSufficientQuanta((Element)i, 1))
            {
                victim.SpendQuantaLogic((Element)i, 1);
                hpToRestore++;
            }
        }
        Owner.ModifyHealthLogic(hpToRestore, false, false);
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
