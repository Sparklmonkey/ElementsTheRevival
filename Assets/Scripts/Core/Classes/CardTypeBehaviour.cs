using System;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class CardTypeBehaviour : MonoBehaviour
{
    private EventBinding<OnDeathDTriggerEvent> _onDeathTriggerBinding;
    private EventBinding<OnTurnEndEvent> _onTurnEndBinding;
    private EventBinding<OnCardRemovedEvent> _onCardRemovedBinding;
    private EventBinding<OnCardPlayEvent> _onCardPlayBinding;

    private void OnEnable()
    {
        _onDeathTriggerBinding = new EventBinding<OnDeathDTriggerEvent>(DeathTrigger);
        EventBus<OnDeathDTriggerEvent>.Register(_onDeathTriggerBinding);

        _onTurnEndBinding = new EventBinding<OnTurnEndEvent>(OnTurnEnd);
        EventBus<OnTurnEndEvent>.Register(_onTurnEndBinding);
        
        _onCardRemovedBinding = new EventBinding<OnCardRemovedEvent>(OnCardRemove);
        EventBus<OnCardRemovedEvent>.Register(_onCardRemovedBinding);
        
        _onCardPlayBinding = new EventBinding<OnCardPlayEvent>(OnCardPlay);
        EventBus<OnCardPlayEvent>.Register(_onCardPlayBinding);
    }

    public IDCardPair cardPair;
    public PlayerManager Owner { get; set; }
    public PlayerManager Enemy { get; set; }
    public int StackCount { get; set; }

    protected virtual void OnTurnEnd(OnTurnEndEvent onTurnEndEvent)
    {
        Debug.Log($"Not Overriden {name}");
    }

    public abstract void OnTurnStart();

    protected virtual void OnCardPlay(OnCardPlayEvent onCardPlayEvent)
    {
        Debug.Log($"Not Overriden {name}");
    }

    protected virtual void OnCardRemove(OnCardRemovedEvent onCardRemovedEvent)
    {
        Debug.Log($"Not Overriden {name}");
    }

    protected virtual void DeathTrigger(OnDeathDTriggerEvent onDeathTriggerEvent)
    {
        Debug.Log($"Not Overriden {name}");
    }
}
