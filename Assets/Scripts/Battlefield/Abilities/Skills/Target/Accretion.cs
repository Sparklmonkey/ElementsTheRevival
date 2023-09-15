using System.Collections.Generic;
using UnityEngine;

public class Accretion : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.Permanent;

    public override void Activate(IDCardPair target)
    {
        BattleVars.Shared.AbilityOrigin.card.DefModify += 15;
        target.RemoveCard();
        if (BattleVars.Shared.AbilityOrigin.card.DefNow >= 45)
        {
            Owner.playerHand.AddCardToHand(BattleVars.Shared.AbilityOrigin.card.iD.IsUpgraded()
                ? CardDatabase.Instance.GetCardFromId("74f")
                : CardDatabase.Instance.GetCardFromId("55v"));
            BattleVars.Shared.AbilityOrigin.RemoveCard();
        }
        else
        {
            BattleVars.Shared.AbilityOrigin.UpdateCard();
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

        if (possibleTargets.Count == 0)
        {
            return new();
        }

        return possibleTargets.FindAll(x => x.IsTargetable());
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> possibleTargets)
    {
        if (possibleTargets.Count == 0)
        {
            return null;
        }

        IDCardPair currentTarget = null;
        var score = 0;

        foreach (var target in possibleTargets)
        {
            int currentScore = target.id.owner == Owner.playerID.id.owner ? 0 : 100;
            currentScore += target.card.AtkNow;
            currentScore += target.card.DefNow;

            currentScore += target.card.skill == "" ? 30 : 0;
            currentScore += (int)target.card.cardType * 20;

            if (currentScore > score)
            {
                score = currentScore;
                currentTarget = target;
            }
        }

        return currentTarget;
    }
}