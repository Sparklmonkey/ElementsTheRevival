using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpellAbility
{
    public bool isTargetFixed { get; }
    public bool IsValidTarget(ID iD);
    public void ActivateAbility(ID iD);

}
