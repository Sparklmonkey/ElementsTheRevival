namespace Battlefield.Abilities
{
    public class VoidEndTurn : OnEndTurnAbility
    {
        public override void Activate(ID owner)
        {
            var amount =
                DuelManager.Instance.GetIDOwner(owner).playerPassiveManager.GetMark().card.costElement is Element
                    .Darkness
                    ? 3
                    : 2;
            EventBus<ModifyPlayerHealthLogicEvent>.Raise(new ModifyPlayerHealthLogicEvent(amount, owner.owner, true));
        }
    }
}