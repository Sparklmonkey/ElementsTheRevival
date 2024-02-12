using Core.Helpers;

namespace Battlefield.Abilities
{
    class BoneyardDeathTrigger : DeathTriggerAbility
    {
        public override void Activate(ID owner)
        {
            var card = CardDatabase.Instance.GetCardFromId("52m");
            EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(card, owner.IsOwnedBy(OwnerEnum.Player)));
            EventBus<PlayCreatureOnFieldEvent>.Raise(new PlayCreatureOnFieldEvent(owner.owner, card));
        }
    }
}