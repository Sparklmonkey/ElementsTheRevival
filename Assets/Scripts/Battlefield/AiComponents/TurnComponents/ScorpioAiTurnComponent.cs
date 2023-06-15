using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScorpioAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{
    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {
        //Play Arsenic if none in play
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Elite Arsenic"));
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Elite Poseidon"));
        yield return aiManager.StartCoroutine(PlayShield(aiManager, "Permafrost Shield"));

        //Play Creatures
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Physalia"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Abyss Crawler"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Arctic Octopus"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Arctic Dragon"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Puffer Fish"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Ulitharids"));

        yield return aiManager.StartCoroutine(ActivateRepeatSpellNoTarget(aiManager, "Poison", "Deadly Poison"));
        yield return aiManager.StartCoroutine(ActivateRepeatAbilityNoTarget(aiManager, CardType.Creature, "Chrysaora", "Physalia"));
        yield return aiManager.StartCoroutine(creatureManager.ActivateAllCreatureAbility(aiManager));
        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Congeal", "Congeal"));
        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Twin Universe", "Twin Universe"));
        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Ice Lance", "Ice Lance"));
    }
}
