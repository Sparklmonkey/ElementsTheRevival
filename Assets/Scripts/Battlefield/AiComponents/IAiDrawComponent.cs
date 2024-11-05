public interface IAiDrawComponent
{
    void StartTurnDrawCard(PlayerManager aiManager);
}

public class FalseGodDrawComponenet : IAiDrawComponent
{
    public void StartTurnDrawCard(PlayerManager aiManager)
    {
        if (aiManager.playerHand.GetHandCount() < 7)
        {
            EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(OwnerEnum.Opponent));
        }
        EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(OwnerEnum.Opponent));
    }
}

public class HalfBloodDrawComponenet : IAiDrawComponent
{
    public void StartTurnDrawCard(PlayerManager aiManager)
    {
        if (aiManager.playerHand.GetHandCount() < 3)
        {
            EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(OwnerEnum.Opponent));
        }
        EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(OwnerEnum.Opponent));
    }
}


public class BaseAiDrawComponent : IAiDrawComponent
{
    public void StartTurnDrawCard(PlayerManager aiManager)
    {
        EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(OwnerEnum.Opponent));
    }
}