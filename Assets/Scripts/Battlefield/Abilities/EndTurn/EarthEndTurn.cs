namespace Battlefield.Abilities
{
    public class EarthEndTurn : OnEndTurnAbility
    {
        public override void Activate(ID owner, Card card)
        {
            EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(owner, "QuantaGenerate", Element.Earth));
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, Element.Earth, owner.owner, true));
        }
    }
}