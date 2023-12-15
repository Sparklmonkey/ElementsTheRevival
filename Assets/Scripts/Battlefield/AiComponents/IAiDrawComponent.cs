public interface IAiDrawComponent
{
    void StartTurnDrawCard(PlayerManager aiManager);
}

public class FalseGodDrawComponenet : IAiDrawComponent
{
    public void StartTurnDrawCard(PlayerManager aiManager)
    {
        if (aiManager.GetHandCards().Count < 8)
        {
            EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(false));
        }
        EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(false));
    }
}

public class HalfBloodDrawComponenet : IAiDrawComponent
{
    public void StartTurnDrawCard(PlayerManager aiManager)
    {
        if (aiManager.GetHandCards().Count < 3)
        {
            EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(false));
        }
        EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(false));
    }
}


public class BaseAiDrawComponent : IAiDrawComponent
{
    public void StartTurnDrawCard(PlayerManager aiManager)
    {
        EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(false));
    }
}