namespace Battlefield.Abilities
{
    class SundialPlayRemoveAbility : OnPlayRemoveAbility
    {
        public override void OnPlayActivate(ID owner, Card card)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Delay, owner.owner, 1));
        }

        public override void OnRemoveActivate(ID owner, Card card)
        {
            return;
        }
    }
}