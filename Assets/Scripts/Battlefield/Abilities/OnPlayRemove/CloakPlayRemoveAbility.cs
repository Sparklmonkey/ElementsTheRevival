namespace Battlefield.Abilities
{
    class CloakPlayRemoveAbility : OnPlayRemoveAbility
    {
        public override void OnPlayActivate(ID owner, Card card)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Invisibility, owner.owner, 3));
            // DuelManager.Instance.GetIDOwner(owner).ActivateCloakEffect(transform);
        }

        public override void OnRemoveActivate(ID owner, Card card)
        {
            var player = DuelManager.Instance.GetIDOwner(owner);
            player.ResetCloakPermParent((owner, card));
            if (player.playerPermanentManager.GetAllValidCardIds().FindAll(x => x.Item2.Id is "5v2" or "7ti").Count != 1) return;
            player.DeactivateCloakEffect();
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Invisibility, owner.owner, -3));
        }
    }
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