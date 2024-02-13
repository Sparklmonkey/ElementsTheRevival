namespace Battlefield.Abilities
{
    class BonesDeathTrigger : DeathTriggerAbility
    {
        public override void Activate(ID owner, Card card)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Bone, owner.owner, 2));
        }
    }
}