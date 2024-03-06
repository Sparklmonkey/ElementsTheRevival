using System.Collections.Generic;
using Core.Helpers;

public class Accretion : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        BattleVars.Shared.AbilityCardOrigin.DefModify += 15;
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
        if (BattleVars.Shared.AbilityCardOrigin.DefNow >= 45)
        {
            var cardToAdd = BattleVars.Shared.AbilityCardOrigin.Id.IsUpgraded()
                ? CardDatabase.Instance.GetCardFromId("74f")
                : CardDatabase.Instance.GetCardFromId("55v");
            
            EventBus<AddCardToHandEvent>.Raise(new AddCardToHandEvent(BattleVars.Shared.AbilityIDOrigin.owner, cardToAdd.Clone()));
            EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(BattleVars.Shared.AbilityIDOrigin));
        }
        else
        {
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(BattleVars.Shared.AbilityIDOrigin, BattleVars.Shared.AbilityCardOrigin, true));
        }
    }
    
    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        if (id.IsPermanentField() && card.IsTargetable())
        {
            return true;
        }
        if (card.Id is "4t1" or "4t2") return false;
        if (card.Type.Equals(CardType.Mark)) return false;
        return id.field.Equals(FieldEnum.Passive) && card.Type is CardType.Shield or CardType.Weapon && card.IsTargetable();
    }
    
    public override AiTargetType GetTargetType()
    {
        return new AiTargetType(false, false, false, TargetType.Permanent, -10, 0, 0);
    }
}