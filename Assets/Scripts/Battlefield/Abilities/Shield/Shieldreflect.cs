using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shieldreflect : ShieldAbility
{
    public override void ActivateShield(ref int atkNow, ref IDCardPair cardPair)
    {
        if (cardPair.card.passive.Contains("psion"))
        {
            Enemy.ModifyHealthLogic(atkNow, true, false);
            atkNow = 0;
        }

    }
}
