namespace Battlefield.Abilities
{
    class BonesDeathTrigger : DeathTriggerAbility
    {
        public override void Activate(ID owner)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Bone, owner.owner, 2));
        }
    }
}