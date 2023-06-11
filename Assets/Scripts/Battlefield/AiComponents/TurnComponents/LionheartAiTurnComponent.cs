using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LionheartAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{
    

    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {
        //PlayWeapon
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Elite Eternity"));

        //Play Creatures in order of priority
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Crusader"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Anubis"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Electrum Hourglass"));

        //Play Shield
        yield return aiManager.StartCoroutine(PlayShield(aiManager, "Turtle Shield"));

        //Play Spells
        yield return aiManager.StartCoroutine(spellManager.PlayAnimateWeapon(aiManager));

        //Activate Abilities
        yield return aiManager.StartCoroutine(ActivateRepeatAbilityNoTarget(aiManager, CardType.Artifact, "Golden Hourglass", "Electrum Hourglass"));
        yield return aiManager.StartCoroutine(ActivateCrusaders(aiManager));
        yield return aiManager.StartCoroutine(ActivateRepeatAbilityWithTarget(aiManager, CardType.Artifact, "Anubis", "Elite Anubis", x => x.Owner.Equals(OwnerEnum.Opponent)));
        yield return aiManager.StartCoroutine(spellManager.ActivateQuintessence(aiManager));
        yield return aiManager.StartCoroutine(ActivateRepeatAbilityWithTarget(aiManager, CardType.Artifact, "Eternity", "Elite Eternity", x => x.Owner.Equals(OwnerEnum.Player)));

    }
}
