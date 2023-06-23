public class Shielddissipation : ShieldAbility
{
    public override void ActivateShield(ref int atkNow, ref IDCardPair cardPair)
    {
        if (Owner.playerCounters.sanctuary > 0) { return; }
        int allQuanta = Owner.GetAllQuantaOfElement(Element.Other);
        if (allQuanta >= atkNow)
        {
            Owner.SpendQuantaLogic(Element.Other, atkNow);
            atkNow = 0;
        }
        else
        {
            Owner.playerPassiveManager.RemoveShield();
        }
    }
}
