using System.Collections.Generic;
using System.Linq;

public class PlayWeaponFromHandStrategy : IStrategy
{
    private readonly List<(Card card, ID id)> _cardsInHand;
    private readonly PlayerManager _aiOwner;

    public PlayWeaponFromHandStrategy(List<(Card card, ID id)> cardsInHand, PlayerManager aiOwner)
    {
        _cardsInHand = cardsInHand;
        _aiOwner = aiOwner;
    }
    public Node.Status Process()
    {
        if (_aiOwner.playerPassiveManager.GetWeapon().HasCard()) return Node.Status.Failure;
        foreach (var tuple in _cardsInHand.Where(tuple => tuple.card.Type.Equals(CardType.Weapon)))
        {
            EventBus<PlayCardFromHandEvent>.Raise(new PlayCardFromHandEvent(tuple.card, tuple.id));
            return Node.Status.Success;
        }

        return Node.Status.Failure;
    }

    public void Reset()
    {
        return;
    } 
}