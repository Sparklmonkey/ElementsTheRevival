﻿public class Shieldicetwo : ShieldAbility
{
    public override void ActivateShield(ref int atkNow, ref IDCardPair cardPair)
    {
        atkNow = atkNow > 2 ? atkNow - 2 : 0;
        cardPair.card.Freeze = UnityEngine.Random.Range(0f, 1f) <= 0.3f ? 3 : 0;
    }
}