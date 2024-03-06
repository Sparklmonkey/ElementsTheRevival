namespace Battlefield.Abilities
{
    public class FireEndTurn : OnEndTurnAbility
    {
        public override void Activate(ID owner, Card card)
        {
            EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(owner, "QuantaGenerate", Element.Fire));
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, Element.Fire, owner.owner, true));
        }
    }
}