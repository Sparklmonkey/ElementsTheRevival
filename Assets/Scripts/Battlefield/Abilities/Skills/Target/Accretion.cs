using System.Collections.Generic;
using UnityEngine;

public class Accretion : AbilityEffect
{
    public override bool NeedsTarget() => true;

    public override void Activate(IDCardPair target)
    {
        BattleVars.shared.abilityOrigin.card.DefModify += 15;
        target.RemoveCard();
        if (BattleVars.shared.abilityOrigin.card.DefNow >= 45)
        {
            Owner.playerHand.AddCardToHand(BattleVars.shared.abilityOrigin.card.iD.IsUpgraded() ? CardDatabase.Instance.GetCardFromId("74f") : CardDatabase.Instance.GetCardFromId("55v"));
            BattleVars.shared.abilityOrigin.RemoveCard();
        }
        else
        {
            BattleVars.shared.abilityOrigin.UpdateCard();
        }
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = enemy.playerPermanentManager.GetAllValidCardIds();
        possibleTargets.AddRange(Owner.playerPermanentManager.GetAllValidCardIds());
        if (enemy.playerPassiveManager.GetWeapon().HasCard())
        {
            possibleTargets.Add(enemy.playerPassiveManager.GetWeapon());
        }
        if (Owner.playerPassiveManager.GetWeapon().HasCard())
        {
            possibleTargets.Add(Owner.playerPassiveManager.GetWeapon());
        }
        if (enemy.playerPassiveManager.GetShield().HasCard())
        {
            possibleTargets.Add(enemy.playerPassiveManager.GetShield());
        }
        if (Owner.playerPassiveManager.GetShield().HasCard())
        {
            possibleTargets.Add(Owner.playerPassiveManager.GetShield());
        }
        if (possibleTargets.Count == 0) { return new(); }

        return possibleTargets.FindAll(x => x.IsTargetable());
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        if (posibleTargets.Count == 0) { return null; }

        var opCreatures = posibleTargets.FindAll(x => x.id.Owner == OwnerEnum.Player && x.HasCard());

        if (opCreatures.Count == 0)
        {
            return null;
        }
        else
        {
            return opCreatures[Random.Range(0, posibleTargets.Count)];
        }
    }
}