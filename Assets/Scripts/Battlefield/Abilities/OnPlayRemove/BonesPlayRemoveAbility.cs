namespace Battlefield.Abilities
{
    class BonesPlayRemoveAbility : OnPlayRemoveAbility
    {
        public override void OnPlayActivate(ID owner, Card card)
        {
            if (BattleVars.Shared.AbilityCardOrigin is not null)
            {
                if (BattleVars.Shared.AbilityCardOrigin.Skill is Steal)
                {
                    EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Bone, owner.owner, 1));
                    return;
                }
            }
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Bone, owner.owner, 7));
        }

        public override void OnRemoveActivate(ID owner, Card card)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Bone, owner.owner, -9999));
        }
    }
}