using System;
using System.Collections.Generic;

[Serializable]
public class CreatureManager : FieldManager
{
    private readonly List<int> _creatureCardOrder = new() { 11, 13, 9, 10, 12, 14, 8, 16, 18, 20, 22, 0, 2, 4, 6, 15, 17, 19, 21, 1, 3, 5, 7 };
    private readonly List<int> _safeZones = new() { 11, 13, 10, 12, 14 };

    private bool _isPlayer;
        
    private EventBinding<PlayCardOnFieldEvent> _playCardOnFieldBinding;
    
    public void OnDisable() {
        EventBus<PlayCardOnFieldEvent>.Unregister(_playCardOnFieldBinding);
    }

    public void SetupManager(bool isPlayer)
    {
        _isPlayer = isPlayer;
        _playCardOnFieldBinding = new EventBinding<PlayCardOnFieldEvent>(PlayCreature);
        EventBus<PlayCardOnFieldEvent>.Register(_playCardOnFieldBinding);
    }
    
    public List<IDCardPair> GetCreaturesWithGravity()
    {
        var idCardList = GetAllValidCardIds();
        if (idCardList.Count == 0) { return new List<IDCardPair>(); }
        return idCardList.FindAll(x => x.card.passiveSkills.GravityPull);
    }

    public void CreatureTurnDown()
    {
        foreach (var idCard in PairList)
        {
            if (idCard.HasCard())
            {
                idCard.cardBehaviour.OnTurnStart();
                idCard.UpdateCard();
            }
        }

    }

    public void PlayCreature(PlayCardOnFieldEvent playCardOnFieldEvent)
    {
        if (!playCardOnFieldEvent.CardToPlay.cardType.Equals(CardType.Creature))
        {
            return;
        }
        if (playCardOnFieldEvent.IsPlayer != _isPlayer)
        {
            return;
        }
        var index = 0;
        if (DuelManager.FloodCount > 0 && !playCardOnFieldEvent.CardToPlay.costElement.Equals(Element.Other) && !playCardOnFieldEvent.CardToPlay.costElement.Equals(Element.Water))
        {
            for (var i = 0; i < _safeZones.Count; i++)
            {
                var orderIndex = _safeZones[i];
                if (PairList[orderIndex].HasCard()) {continue;}
                EventBus<OnCardPlayEvent>.Raise(new OnCardPlayEvent(PairList[orderIndex].id, playCardOnFieldEvent.CardToPlay));
                index = orderIndex;
                break;
            }
        }
        else
        {
            foreach (var orderIndex in _creatureCardOrder)
            {
                if (!PairList[orderIndex].HasCard())
                {
                    EventBus<OnCardPlayEvent>.Raise(new OnCardPlayEvent(PairList[orderIndex].id, playCardOnFieldEvent.CardToPlay));
                    index = orderIndex;
                    break;
                }

            }
        }
    }

    internal void ClearField()
    {
        foreach (var pair in PairList)
        {
            pair.card = null;
        }
        
    }
}
