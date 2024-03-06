namespace Battlefield.Abilities
{
    public class DissipationSkill : ShieldSkill
    {
        public override int ActivateSkill(int atkNow, (ID id, Card card) cardPair)
        {
            var owner = DuelManager.Instance.GetNotIDOwner(cardPair.id);
            if (owner.playerCounters.sanctuary > 0) { return atkNow; }
            var allQuanta = owner.GetAllQuantaOfElement(Element.Other);
            if (allQuanta >= atkNow)
            {
                EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(atkNow, Element.Other, cardPair.id.owner.Not(), false));
                atkNow = 0;
            }
            else
            {
                EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(new ID(cardPair.id.owner.Not(), FieldEnum.Passive, 2)));
            }

            return atkNow;
        }
    }
}