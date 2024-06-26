namespace Battlefield.Abilities
{
    class TowerPlayRemoveAbility : OnPlayRemoveAbility
    {
        public override void OnPlayActivate(ID owner, Card card)
        {
            if (card.CostElement.Equals(Element.Other))
            {
                EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(3, card.CostElement, owner.owner, true));
                return;
            }
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, card.CostElement, owner.owner, true));
        }
    }
}