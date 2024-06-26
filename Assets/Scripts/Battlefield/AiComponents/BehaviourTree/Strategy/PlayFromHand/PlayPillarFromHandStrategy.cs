using System.Collections.Generic;
using System.Linq;

public class PlayPillarFromHandStrategy : IStrategy
{
    public PlayPillarFromHandStrategy()
    {
    }
    public Node.Status Process((Card card, ID id) cardId)
    {
        EventBus<PlayCardFromHandEvent>.Raise(new PlayCardFromHandEvent(cardId.card, cardId.id));
        return Node.Status.Success;
    }

    public void Reset()
    {
        return;
    } 
}