namespace Battlefield.Abilities
{
    class ESoulCatcherDeathTrigger : DeathTriggerAbility
    {
        public override void Activate(ID owner, Card card)
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(3, Element.Death, owner.owner, true));
        }
    }
}