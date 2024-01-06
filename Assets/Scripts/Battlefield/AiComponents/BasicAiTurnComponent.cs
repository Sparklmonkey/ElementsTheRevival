using System.Collections;

public class BasicAiTurnComponent : IAiTurnComponent
{
    public IEnumerator PlayPillars(PlayerManager aiManager)
    {
        yield return aiManager.StartCoroutine(PlayPillars(aiManager));
    }

    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {
        throw new System.NotImplementedException();
    }
}
