using System.Collections.Generic;
using System.Linq;

public class Lycanthropy : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        BattleVars.shared.abilityOrigin.card.AtkModify += 5;
        BattleVars.shared.abilityOrigin.card.DefModify += 5;
        BattleVars.shared.abilityOrigin.card.skill = "";
        BattleVars.shared.abilityOrigin.card.desc = "";
        BattleVars.shared.abilityOrigin.UpdateCard();
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
