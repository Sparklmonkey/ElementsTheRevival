using System.Collections;

public class ParadoxAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{
    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {

        //Play Arsenic if none in play
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Morning Glory"));
        //Play Arsenic if none in play
        yield return aiManager.StartCoroutine(PlayShield(aiManager, "Mirror Shield"));

        //Play Creatures
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Deja Vu"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Electrum Hourglass"));

        //Activate Graboids
        yield return aiManager.StartCoroutine(ActivateRepeatAbilityNoTarget(aiManager, CardType.Artifact, "Golden Hourglass", "Electrum Hourglass"));
        //Activate Stone Skin
        yield return aiManager.StartCoroutine(spellManager.PlayBlessings(aiManager));

        //
    }
}
