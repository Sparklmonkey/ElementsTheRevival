using System.Collections;

public class SerketAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{
    private int _cloakCount = 0;


    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {
        _cloakCount--;
        //Play Arsenic if none in play
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Elite Arsenic"));

        //Play Creatures
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Scorpion"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Deathstalker"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Life Nymph"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Flesh Recluse"));

        //Play Eclipse
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Eclipse"));

        //Activate Life Nymphs on Death stalkers and Scorpions
        yield return aiManager.StartCoroutine(CreatureManager.ActivateAllCreatureAbility(aiManager));
        if (_cloakCount <= 0)
        {
            yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Cloak"));
        }
    }
}
