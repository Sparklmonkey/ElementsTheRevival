public class Shieldsolar : ShieldAbility
{
    public override void ActivateShield(ref int atkNow, ref IDCardPair cardPair)
    {
        Owner.GenerateQuantaLogic(Element.Light, 1);
    }
}
