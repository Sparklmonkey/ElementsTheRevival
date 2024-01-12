using System.Collections.Generic;

public class Accretion : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.Permanent;

    public override void Activate(ID targetId, Card targetCard)
    {
        BattleVars.Shared.AbilityCardOrigin.DefModify += 15;
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
        if (BattleVars.Shared.AbilityCardOrigin.DefNow >= 45)
        {
            var cardToAdd = BattleVars.Shared.AbilityCardOrigin.iD.IsUpgraded()
                ? CardDatabase.Instance.GetCardFromId("74f")
                : CardDatabase.Instance.GetCardFromId("55v");
            
            EventBus<AddCardToHandEvent>.Raise(new AddCardToHandEvent(Owner.Owner, new(cardToAdd)));
            EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(BattleVars.Shared.AbilityIDOrigin));
        }
        else
        {
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(BattleVars.Shared.AbilityIDOrigin, BattleVars.Shared.AbilityCardOrigin, true));
        }
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = enemy.playerPermanentManager.GetAllValidCardIds();
        possibleTargets.AddRange(Owner.playerPermanentManager.GetAllValidCardIds());
        if (enemy.playerPassiveManager.GetWeapon().Item2.iD != "4t2")
        {
            possibleTargets.Add(enemy.playerPassiveManager.GetWeapon());
        }

        if (Owner.playerPassiveManager.GetWeapon().Item2.iD != "4t2")
        {
            possibleTargets.Add(Owner.playerPassiveManager.GetWeapon());
        }

        if (enemy.playerPassiveManager.GetShield().Item2.iD != "4t1")
        {
            possibleTargets.Add(enemy.playerPassiveManager.GetShield());
        }

        if (Owner.playerPassiveManager.GetShield().Item2.iD != "4t1")
        {
            possibleTargets.Add(Owner.playerPassiveManager.GetShield());
        }

        return possibleTargets.Count == 0 ? new() : possibleTargets.FindAll(x => x.IsTargetable());
    }

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets)
    {
        if (possibleTargets.Count == 0)
        {
            return default;
        }

        (ID, Card) currentTarget = default;
        var score = 0;

        foreach (var target in possibleTargets)
        {
            var currentScore = target.Item1.owner == Owner.playerID.owner ? 0 : 100;
            currentScore += target.Item2.AtkNow;
            currentScore += target.Item2.DefNow;

            currentScore += target.Item2.skill == "" ? 30 : 0;
            currentScore += (int)target.Item2.cardType * 20;

            if (currentScore <= score) continue;
            score = currentScore;
            currentTarget = target;
        }

        return currentTarget;
    }
}