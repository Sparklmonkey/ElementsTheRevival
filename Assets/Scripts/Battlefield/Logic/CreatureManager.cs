using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class CreatureManager : MonoBehaviour
{
    [SerializeField] private GameObject creaturePrefab;
    [SerializeField] private List<Transform> creaturePositions;

    private OwnerEnum _owner;

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

    public void SetOwner(OwnerEnum owner) => _owner = owner;
    
    public List<(ID, Card)> GetCreaturesWithGravity()
    {
        var idCardList = GetAllValidCardIds();
        return idCardList.Count == 0 ? new List<(ID, Card)>() : idCardList.FindAll(x => x.Item2.passiveSkills.GravityPull);
    }

    public List<Card> GetAllValidCards()
    {
        var firstList = creaturePositions.FindAll(p => p.childCount > 0);
        return firstList.Select(permanent => permanent.GetComponentInChildren<CreatureCardDisplay>().Card)
            .ToList();
    }

    public List<(ID id, Card card)> GetAllValidCardIds()
    {
        var firstList = creaturePositions.FindAll(p => p.childCount > 0);
        var list = firstList.Select(permanent => permanent.GetComponentInChildren<CreatureCardDisplay>()).ToList();

        return list.Select(item => (item.Id, item.Card)).ToList();
    }
    public void CreatureTurnDown()
    {

    }

    public void PlayCreature(PlayCreatureOnFieldEvent playCardOnFieldEvent)
    {
        if (!playCardOnFieldEvent.Owner.Equals(_owner)) return;

        if (FloodCheck(playCardOnFieldEvent.CardToPlay)) return;
        foreach (var orderIndex in _creatureCardOrder.Where(orderIndex => creaturePositions[orderIndex].childCount <= 0))
        {
            InstantiateCreature(orderIndex, playCardOnFieldEvent.CardToPlay);
            return;
        }
    }

    private bool FloodCheck(Card card)
    {
        if (DuelManager.FloodCount <= 0) return false;

        foreach (var index in _safeZones)
        {
            if (creaturePositions[index].childCount > 0) continue;
            InstantiateCreature(index, card);
            return true;
        }
        return false;
    }

    private void InstantiateCreature(int index, Card card)
    {
        var id = new ID(_owner, FieldEnum.Creature, index);
        var creatureCardObject = Instantiate(creaturePrefab, creaturePositions[index]);
        creatureCardObject.GetComponent<CreatureCardDisplay>().SetupId(id);

        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(id, card));
    }
}
