using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RainbowAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{

    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {
        //Play Arsenic if none in play
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Eagle's Eye"));

        //Play Creatures
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Werewolf"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Graboid"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Forest Spectre"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Electrum Hourglass"));

        yield return aiManager.StartCoroutine(ActivateRepeatAbilityNoTarget(aiManager, CardType.Creature, "Lycanthrope", "Werewolf"));
        yield return aiManager.StartCoroutine(ActivateRepeatAbilityNoTarget(aiManager, CardType.Creature, "Graboid", "Elite Graboid"));
        yield return aiManager.StartCoroutine(ActivateRepeatAbilityNoTarget(aiManager, CardType.Creature, "Forest Spectre", "Forest Spectre"));
        yield return aiManager.StartCoroutine(ActivateRepeatAbilityNoTarget(aiManager, CardType.Artifact, "Electrum Hourglass", "Electrum Hourglass"));


        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Explosion", "Explosion", x => x.Owner.Equals(OwnerEnum.Player)));
        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Congeal", "Congeal", x => x.Owner.Equals(OwnerEnum.Player)));
        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Improved Steal", "Improved Steal", x => x.Owner.Equals(OwnerEnum.Player)));
        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "ThunderBolt", "ThunderBolt", x => x.Owner.Equals(OwnerEnum.Player)));

        yield return aiManager.StartCoroutine(ActivateRepeatAbilityWithTarget(aiManager, CardType.Artifact, "Eagle's Eye", "Eagle's Eye", x => x.Owner.Equals(OwnerEnum.Player)));
        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Gravity Force", "Gravity Force", x => x.Owner.Equals(OwnerEnum.Player)));
        yield return aiManager.StartCoroutine(spellManager.PlayMiracle(aiManager));
    }
}
