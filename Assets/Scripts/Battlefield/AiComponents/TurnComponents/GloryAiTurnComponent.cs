using System.Collections;

public class GloryAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{

    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {

        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Morning Glory"));
        yield return aiManager.StartCoroutine(spellManager.PlayAnimateWeapon(aiManager));
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Morning Glory"));
        yield return aiManager.StartCoroutine(spellManager.PlayAnimateWeapon(aiManager));
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Morning Glory"));
        yield return aiManager.StartCoroutine(spellManager.PlayAnimateWeapon(aiManager));
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Morning Glory"));
        yield return aiManager.StartCoroutine(spellManager.PlayAnimateWeapon(aiManager));

        yield return aiManager.StartCoroutine(spellManager.PlayMiracle(aiManager));

        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Deflagration", "Explosion"));
    }
}
