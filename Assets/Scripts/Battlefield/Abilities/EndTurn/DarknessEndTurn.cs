namespace Battlefield.Abilities
{
    public class DarknessEndTurn : OnEndTurnAbility
    {
        public override void Activate(ID owner, Card card)
        {
            EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(owner, "QuantaGenerate", Element.Darkness));
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, Element.Darkness, owner.owner, true));
        }
    }
}