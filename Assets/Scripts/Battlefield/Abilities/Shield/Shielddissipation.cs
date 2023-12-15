public class Shielddissipation : ShieldAbility
{
    public override void ActivateShield(ref int atkNow, ref IDCardPair cardPair)
    {
        if (Owner.playerCounters.sanctuary > 0) { return; }
        var allQuanta = Owner.GetAllQuantaOfElement(Element.Other);
        if (allQuanta >= atkNow)
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(atkNow, Element.Other, Owner.isPlayer, false));
            atkNow = 0;
        }
        else
        {
            Owner.playerPassiveManager.RemoveShield();
        }
    }
}
