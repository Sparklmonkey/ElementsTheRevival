using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public static class EventUtility
{
    public delegate IEnumerator ActivateEffectHandler(MonoBehaviour sender, ActiveCardArgs args);

    public delegate IEnumerator CreatureEventHandler(MonoBehaviour sender, ActiveCardArgs args);

    public delegate IEnumerator DeathTriggerHandler(MonoBehaviour sender, ActiveCardArgs args);

    public delegate IEnumerator EndTurnHandler(MonoBehaviour sender, ActiveCardArgs args);

    public class ActiveCardArgs : EventArgs
    {
        public PlayerManager enemy;
        public bool isSkeletonDeath;
    }

    public static IEnumerator Occured
        (this CreatureEventHandler e, MonoBehaviour sender, ActiveCardArgs args)
    {
        if (e == null) yield break;
        foreach (var handler in e.GetInvocationList().Cast<CreatureEventHandler>())
            yield return handler(sender, args);
    }

    public static IEnumerator Occured
        (this DeathTriggerHandler e, MonoBehaviour sender, ActiveCardArgs args)
    {
        if (e == null) yield break;
        foreach (var handler in e.GetInvocationList().Cast<DeathTriggerHandler>())
            yield return handler(sender, args);
    }

}
