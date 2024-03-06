namespace Battlefield.Abilities
{
    public class LightEndTurn : OnEndTurnAbility
    {
        public override void Activate(ID owner, Card card)
        {
            EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(owner, "QuantaGenerate", Element.Light));
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, Element.Light, owner.owner, true));
        }
    }
}