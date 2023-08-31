using UnityEngine;

public class Shieldedissipation : ShieldAbility
{
    public override void ActivateShield(ref int atkNow, ref IDCardPair cardPair)
    {
        if (Owner.playerCounters.sanctuary > 0) { return; }
        int quantaToUse = Mathf.CeilToInt(atkNow / 3);
        int availableEQuanta = Owner.GetAllQuantaOfElement(Element.Entropy);
        if (availableEQuanta >= quantaToUse)
        {
            Owner.SpendQuantaLogic(Element.Other, quantaToUse);
            atkNow = 0;
        }
        else
        {
            Owner.playerPassiveManager.RemoveShield();
        }
    }
}
