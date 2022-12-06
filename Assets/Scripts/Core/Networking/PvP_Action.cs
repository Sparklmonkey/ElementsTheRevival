using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PseudoPvPAction
{
    public ActionType actionType;
    public PseudoID originId;
    public PseudoID targetId;
}
[Serializable]
public class PvP_Action 
{
    public ActionType ActionType;
    public ID OriginId;
    public ID TargetId;

    public PvP_Action(ActionType actionType, ID originId, ID targetId)
    {
        ActionType = actionType;
        OriginId = originId;
        TargetId = targetId;
    }
}


[Serializable]
public enum ActionType
{
    PlayCardToField,
    PlaySpell,
    CardOnFieldAbility,
    DiscardCard,
    EndTurn
}