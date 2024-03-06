using System.Collections.Generic;
using System.Linq;

public class Endow : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        BattleVars.Shared.AbilityCardOrigin.Skill = targetCard.Skill;
        BattleVars.Shared.AbilityCardOrigin.WeaponPassive = targetCard.WeaponPassive;
        BattleVars.Shared.AbilityCardOrigin.SkillCost = targetCard.SkillCost;
        BattleVars.Shared.AbilityCardOrigin.SkillElement = targetCard.SkillElement;
        BattleVars.Shared.AbilityCardOrigin.AtkModify += targetCard.AtkNow;
        BattleVars.Shared.AbilityCardOrigin.DefModify += 2;
        BattleVars.Shared.AbilityCardOrigin.innateSkills = targetCard.innateSkills;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(BattleVars.Shared.AbilityIDOrigin, BattleVars.Shared.AbilityCardOrigin, true));
    }
    
    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return CardDatabase.Instance.WeaponIdList.Contains(card.Id) && card.IsTargetable();
    }
    
    public override AiTargetType GetTargetType()
    {
        return new AiTargetType(false, false, false, TargetType.Weapon, 1, 0, 0);
    }
}
