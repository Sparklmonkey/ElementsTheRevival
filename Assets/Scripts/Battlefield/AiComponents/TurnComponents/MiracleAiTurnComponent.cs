using System.Collections;

public class MiracleAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{
    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {

        //Play Weapon
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Morning Glory"));
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Jade Staff"));
        //Play Shield
        yield return aiManager.StartCoroutine(PlayShield(aiManager, "Jade Shield"));
        yield return aiManager.StartCoroutine(PlayShield(aiManager, "Solar Buckler"));
        //Play Creatures
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Light Dragon"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Jade Dragon"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Leaf Dragon"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Pegasus"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Queen"));

        //Activate Blessings
        yield return aiManager.StartCoroutine(spellManager.PlayBlessings(aiManager));

        //Activate Abilities
        yield return aiManager.StartCoroutine(ActivateRepeatAbilityNoTarget(aiManager, CardType.Creature, "Pegasus", "Elite Pegasus"));
        yield return aiManager.StartCoroutine(ActivateRepeatAbilityNoTarget(aiManager, CardType.Creature, "Firefly Queen", "Elite Queen"));

        yield return aiManager.StartCoroutine(spellManager.PlayMiracle(aiManager));
    }
}
