namespace Battlefield.Abilities
{
    public class AccelerateEndTurn : OnEndTurnAbility
    {
        public override void Activate(ID owner, Card card)
        {
            card.AtkModify += 2;
            card.DefModify--;
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(owner, card, true));
        }
    }
}