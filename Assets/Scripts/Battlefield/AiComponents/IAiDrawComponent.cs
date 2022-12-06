using System.Threading.Tasks;
using UnityEngine;

public interface IAiDrawComponent
{
    void StartTurnDrawCard(PlayerManager aiManager);
}

public class FalseGodDrawComponenet : IAiDrawComponent
{
    public void StartTurnDrawCard(PlayerManager aiManager)
    {
        if(aiManager.GetHandCards().Count < 8)
        {
            aiManager.DrawCardFromDeckLogic();
        }
        aiManager.DrawCardFromDeckLogic();
    }
}

public class HalfBloodDrawComponenet : IAiDrawComponent
{
    public void StartTurnDrawCard(PlayerManager aiManager)
    {
        if (aiManager.GetHandCards().Count < 3)
        {
            aiManager.DrawCardFromDeckLogic();
        }
        aiManager.DrawCardFromDeckLogic();
    }
}


public class BaseAiDrawComponent : IAiDrawComponent
{
    public void StartTurnDrawCard(PlayerManager aiManager)
    {
        aiManager.DrawCardFromDeckLogic();
    }
}