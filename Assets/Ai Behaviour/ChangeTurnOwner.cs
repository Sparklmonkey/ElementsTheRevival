using System;
using Unity.Behavior.GraphFramework;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/ChangeTurnOwner")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "ChangeTurnOwner", message: "[Player] hands turn over to [Opponent]", category: "Events", id: "2d4f869a178874ffe9fb075a68d8a62b")]
public partial class ChangeTurnOwner : EventChannelBase
{
    public delegate void ChangeTurnOwnerEventHandler(GameObject Player, GameObject Opponent);
    public event ChangeTurnOwnerEventHandler Event; 

    public void SendEventMessage(GameObject Player, GameObject Opponent)
    {
        Event?.Invoke(Player, Opponent);
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        BlackboardVariable<GameObject> PlayerBlackboardVariable = messageData[0] as BlackboardVariable<GameObject>;
        var Player = PlayerBlackboardVariable != null ? PlayerBlackboardVariable.Value : default(GameObject);

        BlackboardVariable<GameObject> OpponentBlackboardVariable = messageData[1] as BlackboardVariable<GameObject>;
        var Opponent = OpponentBlackboardVariable != null ? OpponentBlackboardVariable.Value : default(GameObject);

        Event?.Invoke(Player, Opponent);
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        ChangeTurnOwnerEventHandler del = (Player, Opponent) =>
        {
            BlackboardVariable<GameObject> var0 = vars[0] as BlackboardVariable<GameObject>;
            if(var0 != null)
                var0.Value = Player;

            BlackboardVariable<GameObject> var1 = vars[1] as BlackboardVariable<GameObject>;
            if(var1 != null)
                var1.Value = Opponent;

            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as ChangeTurnOwnerEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as ChangeTurnOwnerEventHandler;
    }
}

