﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EventBus<T> where T : IEvent
{
    private static readonly HashSet<IEventBinding<T>> bindings = new();

    public static void Register(IEventBinding<T> eventBinding) => bindings.Add(eventBinding);
    public static void Unregister(IEventBinding<T> eventBinding) => bindings.Remove(eventBinding);

    public static void Raise(T @event)
    {
        foreach (var binding in bindings.ToList())
        {
            binding.Event.Invoke(@event);
            binding.EventNoArgs.Invoke();
        }
    }
    
    public static IEnumerator RaiseCoroutine(T @event)
    {
        foreach (var binding in bindings)
        {
            binding.Event.Invoke(@event);
            binding.EventNoArgs.Invoke();
            yield return new WaitForFrames(1);
        }
    }

    private static void Clear()
    {
        Debug.Log($"Clearing {typeof(T).Name} bindings");
        bindings.Clear();
    }
}