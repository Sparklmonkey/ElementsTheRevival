using System.Collections.Generic;
using System.Linq;

public class Endow : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        BattleVars.Shared.AbilityCardOrigin.skill = targetCard.skill;
        BattleVars.Shared.AbilityCardOrigin.skillCost = targetCard.skillCost;
        BattleVars.Shared.AbilityCardOrigin.skillElement = targetCard.skillElement;
        BattleVars.Shared.AbilityCardOrigin.AtkModify += targetCard.AtkNow;
        BattleVars.Shared.AbilityCardOrigin.DefModify = targetCard.DefNow;
        BattleVars.Shared.AbilityCardOrigin.innateSkills = targetCard.innateSkills;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(BattleVars.Shared.AbilityIDOrigin, BattleVars.Shared.AbilityCardOrigin, true));
    }

    
    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return CardDatabase.Instance.WeaponIdList.Contains(card.iD) && card.IsTargetable();
    }
}
