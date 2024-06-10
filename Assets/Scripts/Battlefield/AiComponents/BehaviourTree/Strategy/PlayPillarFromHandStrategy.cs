using System.Collections.Generic;
using System.Linq;

public class PlayPillarFromHandStrategy : IStrategy
{
    private readonly List<(Card card, ID id)> _cardsInHand;

    public PlayPillarFromHandStrategy(List<(Card card, ID id)> cardsInHand)
    {
        _cardsInHand = cardsInHand;
    }
    public Node.Status Process()
    {
        foreach (var tuple in _cardsInHand.Where(tuple => tuple.card.Type.Equals(CardType.Pillar)))
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