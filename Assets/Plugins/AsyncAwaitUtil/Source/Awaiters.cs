using System;
using UnityEngine;

// TODO: Remove the allocs here, use a static memory pool?
public static class Awaiters
{
    public static WaitForUpdate NextFrame { get; } = new();

    public static WaitForFixedUpdate FixedUpdate { get; } = new();

    public static WaitForEndOfFrame EndOfFrame { get; } = new();

    public static WaitForSeconds Seconds(float seconds)
    {
        return new WaitForSeconds(seconds);
    }

    public static WaitForSecondsRealtime SecondsRealtime(float seconds)
    {
        return new WaitForSecondsRealtime(seconds);
    }

    public static WaitUntil Until(Func<bool> predicate)
    {
        return new WaitUntil(predicate);
    }

    public static WaitWhile While(Func<bool> predicate)
    {
        return new WaitWhile(predicate);
    }
}
