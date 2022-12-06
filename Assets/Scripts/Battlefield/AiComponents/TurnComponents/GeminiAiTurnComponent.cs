using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeminiAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{

    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {
        //Play Arsenic if none in play
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Electrocutor"));

        yield return aiManager.StartCoroutine(PlayShield(aiManager, "Phase Shield"));
        //Play Creatures
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Massive Dragon"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Immortal"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Phase Dragon"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Phase Recluse"));



        yield return aiManager.StartCoroutine(ActivateRepeatAbilityWithTarget(aiManager, CardType.Artifact, "Electrocutor", "Electrocutor", x => x.Owner.Equals(OwnerEnum.Player)));
        yield return aiManager.StartCoroutine(ActivateRepeatAbilityWithTarget(aiManager, CardType.Creature, "Phase Recluse", "Phase Recluse", x => x.Owner.Equals(OwnerEnum.Player)));

        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Unstoppable", "Unstoppable", x => x.Owner.Equals(OwnerEnum.Opponent)));
        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Twin Universe", "Twin Universe", x => x.Owner.Equals(OwnerEnum.Opponent)));

    }
}
