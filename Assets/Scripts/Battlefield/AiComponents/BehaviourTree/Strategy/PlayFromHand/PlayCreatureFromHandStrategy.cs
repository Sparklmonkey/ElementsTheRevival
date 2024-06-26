using System.Collections.Generic;
using System.Linq;
using Battlefield.Abilities;

public class PlayCreatureFromHandStrategy : IStrategy
{
    private readonly PlayerManager _aiOwner;

    public PlayCreatureFromHandStrategy(PlayerManager aiOwner)
    {
        _aiOwner = aiOwner;
    }
    
    public Node.Status Process((Card card, ID id) cardId)
    {
        if(!_aiOwner.IsCardPlayable(cardId.card)) return Node.Status.Failure;
        if (cardId.card.PlayRemoveAbility is ChimeraPlayRemoveAbility && _aiOwner.playerCreatureField.GetAllValidCards().Count < 2)
        {
            return Node.Status.Failure;
        }
        EventBus<PlayCardFromHandEvent>.Raise(new PlayCardFromHandEvent(cardId.card, cardId.id));
        return Node.Status.Success;
    }

    public void Reset()
    {
        return;
    } 
}