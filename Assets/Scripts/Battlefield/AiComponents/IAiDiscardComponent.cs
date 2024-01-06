using UnityEngine;

public interface IAiDiscardComponent
{
    void DiscardCard(PlayerManager aiManager);
}


public class BaseAiDiscardComponent : IAiDiscardComponent
{
    public void DiscardCard(PlayerManager aiManager)
    {
        if (!aiManager.playerHand.ShouldDiscard()) return;
        var index = Random.Range(0, 8);
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(new ID(OwnerEnum.Opponent, FieldEnum.Hand, index)));
    }
}