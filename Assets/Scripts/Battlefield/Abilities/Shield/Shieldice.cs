public class Shieldice : ShieldAbility
{
    public override void ActivateShield(ref int atkNow, ref IDCardPair cardPair)
    {
        cardPair.card.Freeze = UnityEngine.Random.Range(0f, 1f) <= 0.3f ? 3 : 0;
    }
}
