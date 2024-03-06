namespace Battlefield.Abilities
{
    class TowerPlayRemoveAbility : OnPlayRemoveAbility
    {
        public override void OnPlayActivate(ID owner, Card card)
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, card.CostElement, owner.owner, true));
        }
    }
}