namespace Battlefield.Abilities
{
    public class OverdriveEndTurn : OnEndTurnAbility
    {
        public override void Activate(ID owner, Card card)
        {
            card.AtkModify += 3;
            card.DefModify--;
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(owner, card, true));
        }
    }
}