using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    public bool isTargetFixed { get; }
    public bool IsValidTarget(ID iD);
    public IEnumerator ActivateAbility(ID iD);

}