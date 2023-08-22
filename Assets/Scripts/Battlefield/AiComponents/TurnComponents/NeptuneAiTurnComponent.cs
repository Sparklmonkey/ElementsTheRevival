using System.Collections;

public class NeptuneAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{
    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Elite Poseidon"));
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Eagle's Eye"));
        yield return aiManager.StartCoroutine(PlayShield(aiManager, "Permafrost Shield"));

        //Play Creatures
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Arctic Octopus"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Abyss Crawler"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Arctic Dragon"));

        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Innundation"));

        yield return aiManager.StartCoroutine(ActivateRepeatAbilityWithTarget(aiManager, CardType.Creature, "Arctic Squid", "Arctic Octopus"));

        yield return aiManager.StartCoroutine(ActivateRepeatAbilityNoTarget(aiManager, CardType.Creature, "Armagio", "Elite Armagio"));

        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Freeze", "Congeal"));
        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Shockwave", "Elite Shockwave"));
    }
}
