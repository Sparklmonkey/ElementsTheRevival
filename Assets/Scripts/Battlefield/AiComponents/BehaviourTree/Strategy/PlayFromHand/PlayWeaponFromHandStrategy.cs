using System.Collections.Generic;
using System.Linq;

public class PlayWeaponFromHandStrategy : IStrategy
{
    private readonly PlayerManager _aiOwner;

    public PlayWeaponFromHandStrategy(PlayerManager aiOwner)
    {
        _aiOwner = aiOwner;
    }
    public Node.Status Process((Card card, ID id) cardId)
    {
        if (_aiOwner.playerPassiveManager.GetWeapon().HasCard()) return Node.Status.Failure;
        if(!_aiOwner.IsCardPlayable(cardId.card)) return Node.Status.Failure;
        EventBus<PlayCardFromHandEvent>.Raise(new PlayCardFromHandEvent(cardId.card, cardId.id));
        return Node.Status.Success;
    }

    public void Reset()
    {
        return;
    } 
}