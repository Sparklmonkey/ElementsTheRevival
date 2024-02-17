namespace Battlefield.Abilities
{
    class PhoenixPlayRemoveAbility : OnPlayRemoveAbility
    {
        public override void OnRemoveActivate(ID owner, Card card)
        {
            var ash = CardDatabase.Instance.GetCardFromId(card.Id.IsUpgraded() ? "7dt" : "5fd");
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(owner, ash, false));
        }
    }
}