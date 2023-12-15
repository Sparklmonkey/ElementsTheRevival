using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AiTurnCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI aiTurnCounterLabel;
    private int _turnCount;

    private EventBinding<AddAiTurnCountEvent> _addAiTurnCountBinding;
    private EventBinding<ResetAiTurnCountEvent> _resetAiTurnCountBinding;
    
    private void OnDisable() {
        EventBus<AddAiTurnCountEvent>.Unregister(_addAiTurnCountBinding);
        EventBus<ResetAiTurnCountEvent>.Unregister(_resetAiTurnCountBinding);
    }

    private void OnEnable()
    {
        _addAiTurnCountBinding = new EventBinding<AddAiTurnCountEvent>(AddTurnCount);
        EventBus<AddAiTurnCountEvent>.Register(_addAiTurnCountBinding);
        _resetAiTurnCountBinding = new EventBinding<ResetAiTurnCountEvent>(ResetTurnCount);
        EventBus<ResetAiTurnCountEvent>.Register(_resetAiTurnCountBinding);
    }

    private void AddTurnCount()
    {
        _turnCount++;
        aiTurnCounterLabel.text = _turnCount.ToString();
    }
    
    private void ResetTurnCount()
    {
        _turnCount = 0;
        aiTurnCounterLabel.text = _turnCount.ToString();
    }
}

public class AddAiTurnCountEvent : IEvent
{
}
public class ResetAiTurnCountEvent : IEvent
{
}
