namespace Battlefield.Abilities
{
    class BonesDeathTrigger : DeathTriggerAbility
    {
        public override void Activate(ID owner, Card card)
        {
            if (BattleVars.Shared.WasSkeleton) return;
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Bone, owner.owner, 2));
        }
    }
}