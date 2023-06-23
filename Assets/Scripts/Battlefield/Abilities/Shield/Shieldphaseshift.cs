using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shieldphaseshift : ShieldAbility
{
    public override void ActivateShield(ref int atkNow, ref IDCardPair cardPair)
    {
        atkNow = 0;
    }
}
