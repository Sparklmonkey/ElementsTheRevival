using System.Collections.Generic;
using System.Linq;

public class Endow : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.Permanent;

    public override void Activate(IDCardPair target)
    {
        BattleVars.Shared.AbilityOrigin.card.skill = target.card.skill;
        BattleVars.Shared.AbilityOrigin.card.skillCost = target.card.skillCost;
        BattleVars.Shared.AbilityOrigin.card.skillElement = target.card.skillElement;
        BattleVars.Shared.AbilityOrigin.card.AtkModify += target.card.AtkNow;
        BattleVars.Shared.AbilityOrigin.card.DefModify = target.card.DefNow;
        BattleVars.Shared.AbilityOrigin.UpdateCard();
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
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

        if (possibleTargets.Count == 0)
        {
            return new();
        }

        return possibleTargets.FindAll(x => x.IsTargetable() && CardDatabase.Instance.WeaponIdList.Contains(x.card.iD));
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> possibleTargets)
    {
        if (possibleTargets.Count == 0)
        {
            return null;
        }

        return possibleTargets.Aggregate((i1, i2) => i1.card.AtkNow >= i2.card.AtkNow ? i1 : i2);
    }
}
