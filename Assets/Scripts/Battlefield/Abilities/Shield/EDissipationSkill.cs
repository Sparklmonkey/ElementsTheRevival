namespace Battlefield.Abilities
{
    public class EDissipationSkill : ShieldSkill
    {
        public override int ActivateSkill(int atkNow, (ID id, Card card) cardPair)
        {
            var owner = DuelManager.Instance.GetNotIDOwner(cardPair.id);
            if (owner.playerCounters.sanctuary > 0) { return atkNow; }

            var entropy = owner.GetAllQuantaOfElement(Element.Entropy);
            while (atkNow > 0)
            {
                if (entropy <= 0)
                {
                    EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(new(cardPair.id.owner.Not(), FieldEnum.Passive, 2)));
                    return atkNow;
                }
                EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, Element.Entropy, cardPair.id.owner.Not(), false));
                entropy -= 1;
                atkNow -= 3;
            }

            return atkNow;
        }
    }
}