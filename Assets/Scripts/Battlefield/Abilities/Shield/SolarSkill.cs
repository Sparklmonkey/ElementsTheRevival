namespace Battlefield.Abilities
{
    public class SolarSkill : ShieldSkill
    {
        public override int ActivateSkill(int atkNow, (ID id, Card card) cardPair)
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, Element.Light, cardPair.id.owner.Not(), true));
            return atkNow;
        }
    }
}