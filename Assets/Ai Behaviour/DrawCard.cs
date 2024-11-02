using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Draw Card", story: "[Ai] Draws their card/s for turn", category: "Action", id: "3f99feca83a47218c64e9b4a890aa6f8")]
public partial class DrawCardAction : Action
{
    [SerializeReference] public BlackboardVariable<PlayerManager> Ai;
    protected override Status OnStart()
    {
        EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(Ai.Value.owner));
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

