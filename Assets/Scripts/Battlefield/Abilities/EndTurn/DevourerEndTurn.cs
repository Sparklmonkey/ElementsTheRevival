namespace Battlefield.Abilities
{
    public class DevourerEndTurn : OnEndTurnAbility
    {
        public override void Activate(ID owner, Card card)
        {
            var enemy = DuelManager.Instance.GetNotIDOwner(owner);
            if (enemy.GetAllQuantaOfElement(Element.Other) > 0 && enemy.playerCounters.sanctuary == 0)
            {
                EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, Element.Other, enemy.owner, false));
            }
            EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(owner, "QuantaGenerate", Element.Darkness));
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, Element.Darkness, owner.owner, true));
        }
    }
}