using Core.Helpers;

namespace Battlefield.Abilities
{
    class GraveyardDeathTrigger : DeathTriggerAbility
    {
        public override void Activate(ID owner)
        {
            var card = CardDatabase.Instance.GetCardFromId("716");
            EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(card, owner.IsOwnedBy(OwnerEnum.Player)));
            EventBus<PlayCreatureOnFieldEvent>.Raise(new PlayCreatureOnFieldEvent(owner.owner, card));
        }
    }
}