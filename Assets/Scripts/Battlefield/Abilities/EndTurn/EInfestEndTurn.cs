using Core.Helpers;

namespace Battlefield.Abilities
{
    public class EInfestEndTurn : OnEndTurnAbility
    {
        public override void Activate(ID owner, Card card)
        {
            var cell = CardDatabase.Instance.GetCardFromId("6ro");
                
            EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(card, owner.IsOwnedBy(OwnerEnum.Player)));
            EventBus<PlayCreatureOnFieldEvent>.Raise(new PlayCreatureOnFieldEvent(owner.owner, card));
        }
    }
}