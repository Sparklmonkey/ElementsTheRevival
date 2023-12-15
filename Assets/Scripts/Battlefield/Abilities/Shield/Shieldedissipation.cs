using UnityEngine;

public class Shieldedissipation : ShieldAbility
{
    public override void ActivateShield(ref int atkNow, ref IDCardPair cardPair)
    {
        if (Owner.playerCounters.sanctuary > 0) { return; }
        var quantaToUse = Mathf.CeilToInt(atkNow / 3);
        var availableEQuanta = Owner.GetAllQuantaOfElement(Element.Entropy);
        if (availableEQuanta >= quantaToUse)
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(quantaToUse, Element.Other, Owner.isPlayer, false));
            atkNow = 0;
        }
        else
        {
            Owner.playerPassiveManager.RemoveShield();
        }
    }
}
