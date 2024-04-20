using Core.Helpers;

namespace Battlefield.Abilities
{
    class GraveyardDeathTrigger : DeathTriggerAbility
    {
        public override void Activate(ID owner, Card card)
        {
            var skeleton = CardDatabase.Instance.GetCardFromId("716");
            EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(skeleton, owner.IsOwnedBy(OwnerEnum.Player)));
            EventBus<PlayCreatureOnFieldEvent>.Raise(new PlayCreatureOnFieldEvent(owner.owner, skeleton));
        }
    }
}