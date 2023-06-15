using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OctaneAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{

    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {
        //Play Arsenic if none in play
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Eagle's Eye"));
        yield return aiManager.StartCoroutine(spellManager.PlayAnimateWeapon(aiManager));
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Eagle's Eye"));
        yield return aiManager.StartCoroutine(spellManager.PlayAnimateWeapon(aiManager));
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Eagle's Eye"));
        yield return aiManager.StartCoroutine(spellManager.PlayAnimateWeapon(aiManager));
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Eagle's Eye"));
        yield return aiManager.StartCoroutine(spellManager.PlayAnimateWeapon(aiManager));

        yield return aiManager.StartCoroutine(PlayShield(aiManager, "Fire Buckler"));
        //Play Creatures
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Unstable Gas"));

        yield return aiManager.StartCoroutine(ActivateRepeatAbilityNoTarget(aiManager, CardType.Creature, "Elite Unstable Gas", "Elite Unstable Gas"));


        yield return aiManager.StartCoroutine(ActivateRepeatAbilityWithTarget(aiManager, CardType.Artifact, "Eagle's Eye", "Eagle's Eye"));
        yield return aiManager.StartCoroutine(ActivateRepeatAbilityWithTarget(aiManager, CardType.Creature, "Eagle's Eye", "Eagle's Eye"));

        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Explosion", "Explosion"));

    }
}
