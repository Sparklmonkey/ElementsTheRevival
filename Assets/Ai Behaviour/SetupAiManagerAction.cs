using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetupAiManager", story: "Setup Player Manager for Testing", category: "Action", id: "41534a16336381e319a2a589bb95b5f5")]
public partial class SetupAiManagerAction : Action
{

    protected override Status OnStart()
    {
        BattleVars.Shared.IsPlayerTurn = false;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

