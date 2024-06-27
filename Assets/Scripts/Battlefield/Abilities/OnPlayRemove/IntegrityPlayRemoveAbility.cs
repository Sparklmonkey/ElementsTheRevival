namespace Battlefield.Abilities
{
    class IntegrityPlayRemoveAbility : OnPlayRemoveAbility
    {
        public override void OnPlayActivate(ID owner, Card card)
        {
            var shardList = DuelManager.Instance.GetIDOwner(owner).playerHand.GetAllValidCardIds().FindAll(x => x.card.CardName.Contains("Shard of"));
            var golem = CardDatabase.Instance.GetGolem(shardList);
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(owner, golem, true));
        }
    }
}