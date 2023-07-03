using System.Collections.Generic;
using System.Linq;

public class Endow : AbilityEffect
{
    public override bool NeedsTarget() => true;

    public override void Activate(IDCardPair target)
    {
        BattleVars.shared.abilityOrigin.card.skill = target.card.skill;
        BattleVars.shared.abilityOrigin.card.skillCost = target.card.skillCost;
        BattleVars.shared.abilityOrigin.card.skillElement = target.card.skillElement;
        BattleVars.shared.abilityOrigin.card.AtkModify += target.card.AtkNow;
        BattleVars.shared.abilityOrigin.card.DefModify = target.card.DefNow;
        BattleVars.shared.abilityOrigin.UpdateCard();
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
        if (possibleTargets.Count == 0) { return new(); }
        return possibleTargets.FindAll(x => x.IsTargetable() && CardDatabase.Instance.weaponIdList.Contains(x.card.iD));
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        if (posibleTargets.Count == 0) { return null; }
        return posibleTargets.Aggregate((i1, i2) => i1.card.AtkNow >= i2.card.AtkNow ? i1 : i2);
    }
}
