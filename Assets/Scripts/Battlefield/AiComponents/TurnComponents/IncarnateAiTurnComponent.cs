using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IncarnateAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{
    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {
        //Play Weapon if none in play
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Vampire Dagger"));

        //Play Creatures
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Retrovirus"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Vampire"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Bloodsucker"));

        //Play Eclipse
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Eclipse"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Graveyard"));

        //Play Shield if none in play
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Bone Wall"));

        //Activate Life Nymphs on Death stalkers and Scorpions
        yield return aiManager.StartCoroutine(creatureManager.ActivateBloodsucker(aiManager));
        //Activate Retrovirus
        yield return aiManager.StartCoroutine(creatureManager.ActivateRetroVirus(aiManager));
    }
}
