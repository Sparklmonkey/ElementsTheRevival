using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MorteAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{
    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {
        yield return aiManager.StartCoroutine(spellManager.PlayMiracle(aiManager));
        
        //Play shield
        if(aiManager.playerCounters.bone < 1)
        {
            yield return aiManager.StartCoroutine(PlayShield(aiManager, "Elite Bone Wall"));
        }
        //Play graveyards
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Graveyard"));
        //PlayWeapon
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Elite Arsenic"));
        //Play Creatures in order of priority
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Crusader"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Anubis"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Electrum Hourglass"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Archangel"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Ivory Dragon"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Flesh Recluse"));
        //Play Spells
        yield return aiManager.StartCoroutine(ActivateRepeatSpellNoTarget(aiManager, "Poison", "Deadly Poison"));
        //Activate Abilities
        yield return aiManager.StartCoroutine(ActivateRepeatAbilityNoTarget(aiManager, CardType.Creature, "Virus", "Retrovirus"));
        yield return aiManager.StartCoroutine(ActivateRepeatSpellNoTarget(aiManager, "Plague", "Improved Plague"));

    }
}