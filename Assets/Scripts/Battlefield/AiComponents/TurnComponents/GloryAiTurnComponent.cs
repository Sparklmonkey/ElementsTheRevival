using System.Collections;

public class GloryAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{

    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {

        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Morning Glory"));
        yield return aiManager.StartCoroutine(SpellManager.PlayAnimateWeapon(aiManager));
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Morning Glory"));
        yield return aiManager.StartCoroutine(SpellManager.PlayAnimateWeapon(aiManager));
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Morning Glory"));
        yield return aiManager.StartCoroutine(SpellManager.PlayAnimateWeapon(aiManager));
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Morning Glory"));
        yield return aiManager.StartCoroutine(SpellManager.PlayAnimateWeapon(aiManager));

        yield return aiManager.StartCoroutine(SpellManager.PlayMiracle(aiManager));

        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Deflagration", "Explosion"));
    }
}
