using Core.Helpers;

namespace Battlefield.Abilities
{
    public class InfestEndTurn : OnEndTurnAbility
    {
        public override void Activate(ID owner, Card card)
        {
            var cell = CardDatabase.Instance.GetCardFromId("4t8");
                
            EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(card, owner.IsOwnedBy(OwnerEnum.Player)));
            EventBus<PlayCreatureOnFieldEvent>.Raise(new PlayCreatureOnFieldEvent(owner.owner, card));
        }
    }
}