namespace Battlefield.Abilities
{
    class CloakPlayRemoveAbility : OnPlayRemoveAbility
    {
        public override void OnPlayActivate(ID owner, Card card)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Invisibility, owner.owner, 3));
        }

        public override void OnRemoveActivate(ID owner, Card card)
        {
            var player = DuelManager.Instance.GetIDOwner(owner);
            if (player.playerPermanentManager.GetAllValidCardIds().FindAll(x => x.Item2.Id is "5v2" or "7ti").Count != 1) return;
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Invisibility, owner.owner, -3));
        }
    }
}