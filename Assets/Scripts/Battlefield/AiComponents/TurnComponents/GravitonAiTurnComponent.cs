using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GravitonAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{
    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {
        
        //Play Arsenic if none in play
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Elite Titan"));
        //Play Arsenic if none in play
        yield return aiManager.StartCoroutine(PlayShield(aiManager, "Elite Gravity Shield"));

        //Play Creatures
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Charger"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Armagio"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Otyugh"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Graviton Firemaster"));

        //Activate Graboids
        yield return aiManager.StartCoroutine(ActivateRepeatAbilityWithTarget(aiManager, CardType.Creature, "Otyugh", "Elite Otyugh"));
        yield return aiManager.StartCoroutine(ActivateRepeatAbilityNoTarget(aiManager, CardType.Creature, "Armagio", "Elite Armagio"));
        yield return aiManager.StartCoroutine(ActivateRepeatAbilityNoTarget(aiManager, CardType.Creature, "Graviton Fire Eater", "Graviton Firemaster"));

        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Deflagration", "Explosion"));
        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Momentum", "Unstoppable"));
        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Gravity Pull", "Gravity Force"));
        yield return aiManager.StartCoroutine(ActivateRepeatSpellNoTarget(aiManager, "Rain of Fire", "Fire Storm"));

        //
    }
}
