using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AbilityEffect
{
    public string EffectName { get; set; }
    public PlayerManager Owner { get; set; }
    public abstract bool NeedsTarget();
    public abstract IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets);
    public abstract List<IDCardPair> GetPossibleTargets(PlayerManager enemy);
    public abstract void Activate(IDCardPair target);

}