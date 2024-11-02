using System;
using Unity.Behavior.GraphFramework;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/TurnChange")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "TurnChange", message: "Ownership of turn has changed", category: "Events", id: "d220c36a1f36b97bcb287907b338cded")]
public partial class TurnChange : EventChannelBase
{
    public delegate void TurnChangeEventHandler();
    public event TurnChangeEventHandler Event; 

    public void SendEventMessage()
    {
        Event?.Invoke();
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        Event?.Invoke();
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        TurnChangeEventHandler del = () =>
        {
            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as TurnChangeEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as TurnChangeEventHandler;
    }
}

