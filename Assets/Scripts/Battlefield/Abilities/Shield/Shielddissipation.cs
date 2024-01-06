public class Shielddissipation : ShieldAbility
{
    public override int ActivateShield(int atkNow, (ID id, Card card) cardPair)
    {
        if (Owner.playerCounters.sanctuary > 0) { return atkNow; }
        var allQuanta = Owner.GetAllQuantaOfElement(Element.Other);
        if (allQuanta >= atkNow)
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(atkNow, Element.Other, Owner.Owner, false));
            atkNow = 0;
        }
        else
        {
            Owner.playerPassiveManager.RemoveShield();
        }

        return atkNow;
    }
}
