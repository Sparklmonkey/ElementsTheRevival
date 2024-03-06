namespace Battlefield.Abilities
{
    public class SanctuaryEndTurn : OnEndTurnAbility
    {
        public override void Activate(ID owner, Card card)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(4, false, false, owner.owner));
        }
    }
}