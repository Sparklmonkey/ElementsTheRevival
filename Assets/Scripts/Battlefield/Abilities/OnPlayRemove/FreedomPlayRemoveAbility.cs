namespace Battlefield.Abilities
{
    class FreedomPlayRemoveAbility : OnPlayRemoveAbility
    {
        public override void OnPlayActivate(ID owner, Card card)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Freedom, owner.owner, 1));
        }

        public override void OnRemoveActivate(ID owner, Card card)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Freedom, owner.owner, -1));
        }
    }
}