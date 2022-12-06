using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActivateAbility
{
    public bool ShouldSelectTarget { get; }
    public int AbilityCost { get; }
    public Element AbilityElement { get; }

    public IEnumerator ActivateAbility(ID target);
    public bool IsValidTarget(ID target);
}
