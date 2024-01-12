using System;
using System.Collections.Generic;
using System.Linq;
using Battlefield.Abstract;

[Serializable]
public class CreatureManager : CardManager
{
    private EventBinding<PlayCreatureOnFieldEvent> _playCreatureBinding;

    private void OnDisable()
    {
        EventBus<PlayCreatureOnFieldEvent>.Unregister(_playCreatureBinding);
    }

    private void OnEnable()
    {
        _playCreatureBinding = new EventBinding<PlayCreatureOnFieldEvent>(PlayCreature);
        EventBus<PlayCreatureOnFieldEvent>.Register(_playCreatureBinding);
    }
    
    private readonly List<int> _creatureCardOrder = new() { 11, 13, 9, 10, 12, 14, 8, 16, 18, 20, 22, 0, 2, 4, 6, 15, 17, 19, 21, 1, 3, 5, 7 };
    private readonly List<int> _safeZones = new() { 11, 13, 10, 12, 14 };
    
    public void CreatureTurnDown()
    {

    }

    public void PlayCreature(PlayCreatureOnFieldEvent playCardOnFieldEvent)
    {
        if (!playCardOnFieldEvent.Owner.Equals(owner)) return;

        if (FloodCheck(playCardOnFieldEvent.CardToPlay)) return;
        foreach (var orderIndex in _creatureCardOrder.Where(orderIndex => cardPositions[orderIndex].childCount <= 0))
        {
            var id = new ID(owner, field, orderIndex);
            InstantiateCardObject(id);
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(id, playCardOnFieldEvent.CardToPlay, false));
            return;
        }
    }

    private bool FloodCheck(Card card)
    {
        if (DuelManager.FloodCount <= 0) return false;

        foreach (var index in _safeZones)
        {
            if (cardPositions[index].childCount > 0) continue;
            var id = new ID(owner, field, index);
            InstantiateCardObject(id);
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(id, card, false));
            return true;
        }
        return false;
    }

    // private void InstantiateCreature(int index, Card card)
    // {
    //     var id = new ID(_owner, FieldEnum.Creature, index);
    //     var creatureCardObject = Instantiate(creaturePrefab, creaturePositions[index]);
    //     creatureCardObject.GetComponent<CreatureCardDisplay>().SetupId(id);
    //
    //     EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(id, card, false));
    // }
}