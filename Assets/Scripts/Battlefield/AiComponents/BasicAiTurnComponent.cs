using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAiTurnComponent : AiBaseFunctions, IAiTurnComponent
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
