using UnityEngine;

public class ShieldEdissipation : ShieldAbility
{
    public override int ActivateShield(int atkNow, (ID id, Card card) cardPair)
    {
        var owner = DuelManager.Instance.GetNotIDOwner(cardPair.id);
        if (owner.playerCounters.sanctuary > 0) { return atkNow; }
        var quantaToUse = Mathf.CeilToInt(atkNow / 3);
        var availableEQuanta = owner.GetAllQuantaOfElement(Element.Entropy);
        if (availableEQuanta >= quantaToUse)
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(quantaToUse, Element.Other, cardPair.id.owner.Not(), false));
            atkNow = 0;
        }
        else
        {
            EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(new ID(cardPair.id.owner.Not(), FieldEnum.Passive, 2)));
        }

        return atkNow;
    }
}
