using System;
using UnityEngine;

public static class CommandManager
{
    public static event EventHandler RequerySuggested;

    private static MonoBehaviour _monoBehaviourInstance;
    private static bool _initialized;

    // Initialize with a MonoBehaviour to use for invoking events on the main thread
    public static void Initialize(MonoBehaviour monoBehaviour)
    {
        if (_initialized) return;
        _monoBehaviourInstance = monoBehaviour;
        _initialized = true;
    }

    public static void InvalidateRequerySuggested()
    {
        if (!_initialized)
        {
            Debug.LogWarning("CommandManager not initialized. Please call Initialize with a MonoBehaviour instance first.");
            return;
        }

        if (_monoBehaviourInstance != null)
        {
            // Ensure we're on the main thread
            if (Application.isPlaying)
            {
                _monoBehaviourInstance.StartCoroutine(ExecuteOnNextFrame());
            }
        }
    }

    private static System.Collections.IEnumerator ExecuteOnNextFrame()
    {
        yield return null;
        RequerySuggested?.Invoke(null, EventArgs.Empty);
    }
}