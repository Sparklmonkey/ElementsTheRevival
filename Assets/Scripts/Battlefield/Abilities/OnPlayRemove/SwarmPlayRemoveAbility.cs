namespace Battlefield.Abilities
{
    class SwarmPlayRemoveAbility : OnPlayRemoveAbility
    {
        public override void OnPlayActivate(ID owner, Card card)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Scarab, owner.owner, 1));
        }

        public override void OnRemoveActivate(ID owner, Card card)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Scarab, owner.owner, -1));
        }
    }
}