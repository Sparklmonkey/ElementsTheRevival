using System;
using UnityEngine.Serialization;

[Serializable]
public class PseudoPvPAction
{
    public ActionType actionType;
    public PseudoID originId;
    public PseudoID targetId;
}
[Serializable]
public class PvPAction
{
    [FormerlySerializedAs("ActionType")] public ActionType actionType;
    [FormerlySerializedAs("OriginId")] public ID originId;
    [FormerlySerializedAs("TargetId")] public ID targetId;

    public PvPAction(ActionType actionType, ID originId, ID targetId)
    {
        this.actionType = actionType;
        this.originId = originId;
        this.targetId = targetId;
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