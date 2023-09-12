using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JezebelAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{
    private int cloakCount = 0;

    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {
        cloakCount--;
        //Play Arsenic if none in play
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Vampire Dagger"));
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Permafrost Shield"));

        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Nymph's Tear", "Elite Nymph's Tear"));
        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Siphon Life", "Siphon Life"));
        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Improved Steal", "Improved Steal"));

        yield return aiManager.StartCoroutine(ActivateAbilities(aiManager, CardType.Creature));
        //Activate Cloak if no cloak active
        if (cloakCount <= 0)
        {
            yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Cloak"));
        }
    }
}
