public class Shieldspines : ShieldAbility
{
    public override void ActivateShield(ref int atkNow, ref IDCardPair cardPair)
    {
        if (UnityEngine.Random.Range(0f, 1f) <= 0.75f)
        {
            cardPair.card.Poison++;
        }
    }
}
