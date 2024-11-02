using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Play Pillar", story: "Play a Pillar card from hand", category: "Action", id: "6c17f85fcf04b898e88b26893b694639")]
public partial class PlayPillarAction : Action
{

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
        Debug.Log("Pillar Played");
    }
}

