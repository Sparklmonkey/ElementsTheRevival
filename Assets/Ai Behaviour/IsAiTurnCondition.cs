using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsAiTurn", story: "The player has passed the turn to the AI", category: "Conditions", id: "f2c09c2e3999eaf94c2129caa53edfeb")]
public partial class IsAiTurnCondition : Condition
{

    public override bool IsTrue()
    {
        return !BattleVars.Shared.IsPlayerTurn;
    }

    public override void OnStart()
    {
        Debug.Log("Check Condition");
    }

    public override void OnEnd()
    {
        Debug.Log("Condition Change");
        BattleVars.Shared.IsPlayerTurn = false;
    }
}
