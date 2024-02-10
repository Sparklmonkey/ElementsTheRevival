namespace Battlefield.Abilities
{
    public class EDissipationSkill : ShieldSkill
    {
        public override int ActivateSkill(int atkNow, (ID id, Card card) cardPair)
        {
            var owner = DuelManager.Instance.GetNotIDOwner(cardPair.id);
            if (owner.GetAllQuantaOfElement(Element.Entropy) <= 0)
            {
                EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(new(cardPair.id.owner.Not(), FieldEnum.Passive, 2)));
                return atkNow;
            }
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, Element.Entropy, cardPair.id.owner.Not(), false));
            return atkNow > 3 ? atkNow -= 3 : 0;
        }
    }
}