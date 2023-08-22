using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObliteratorAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{
    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {
        //Play Arsenic if none in play
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Elite Pulverizer"));
        yield return aiManager.StartCoroutine(PlayShield(aiManager, "Diamond Shield"));

        //Play Creatures
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Basalt Dragon"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Shrieker"));

        yield return aiManager.StartCoroutine(ActivateRepeatAbilityWithTarget(aiManager, CardType.Creature, "Pulverizer", "Elite Pulverizer"));

        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Gravity Force", "Gravity Force"));
        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Unstoppable", "Unstoppable"));
        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Protect Artifact", "Protect Artifact"));

    }
}
