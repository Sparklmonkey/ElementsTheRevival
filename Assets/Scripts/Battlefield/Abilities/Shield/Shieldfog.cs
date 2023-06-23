public class Shieldfog : ShieldAbility
{
    public override void ActivateShield(ref int atkNow, ref IDCardPair cardPair)
    {
        atkNow = UnityEngine.Random.Range(0f, 1f) <= 0.4f ? 0 : atkNow;
    }
}
