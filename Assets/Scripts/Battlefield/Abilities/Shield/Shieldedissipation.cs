using UnityEngine;

public class Shieldedissipation : ShieldAbility
{
    public override int ActivateShield(int atkNow, (ID id, Card card) cardPair)
    {
        if (Owner.playerCounters.sanctuary > 0) { return atkNow; }
        var quantaToUse = Mathf.CeilToInt(atkNow / 3);
        var availableEQuanta = Owner.GetAllQuantaOfElement(Element.Entropy);
        if (availableEQuanta >= quantaToUse)
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(quantaToUse, Element.Other, Owner.Owner, false));
            atkNow = 0;
        }
        else
        {
            EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(new ID(Owner.Owner, FieldEnum.Passive, 2)));
        }

        return atkNow;
    }
}
