using System.Collections.Generic;
using System.Linq;

public class Endow : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.Permanent;

    public override void Activate(ID targetId, Card targetCard)
    {
        BattleVars.Shared.AbilityCardOrigin.skill = targetCard.skill;
        BattleVars.Shared.AbilityCardOrigin.skillCost = targetCard.skillCost;
        BattleVars.Shared.AbilityCardOrigin.skillElement = targetCard.skillElement;
        BattleVars.Shared.AbilityCardOrigin.AtkModify += targetCard.AtkNow;
        BattleVars.Shared.AbilityCardOrigin.DefModify = targetCard.DefNow;
        BattleVars.Shared.AbilityCardOrigin.innateSkills = targetCard.innateSkills;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(BattleVars.Shared.AbilityIDOrigin, BattleVars.Shared.AbilityCardOrigin));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = enemy.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(Owner.playerCreatureField.GetAllValidCardIds());
        if (enemy.playerPassiveManager.GetWeapon().HasCard())
        {
            possibleTargets.Add(enemy.playerPassiveManager.GetWeapon());
        }

        if (Owner.playerPassiveManager.GetWeapon().HasCard())
        {
            possibleTargets.Add(Owner.playerPassiveManager.GetWeapon());
        }

        return possibleTargets.Count == 0 ? new() : possibleTargets.FindAll(x => x.IsTargetable() && CardDatabase.Instance.WeaponIdList.Contains(x.Item2.iD));
    }

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets)
    {
        if (possibleTargets.Count == 0)
        {
            return default;
        }

        return possibleTargets.Aggregate((i1, i2) => i1.Item2.AtkNow >= i2.Item2.AtkNow ? i1 : i2);
    }
}
