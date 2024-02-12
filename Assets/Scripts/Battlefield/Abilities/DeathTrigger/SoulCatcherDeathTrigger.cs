namespace Battlefield.Abilities
{
    class SoulCatcherDeathTrigger : DeathTriggerAbility
    {
        public override void Activate(ID owner)
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(2, Element.Death, owner.owner, true));
        }
    }
}