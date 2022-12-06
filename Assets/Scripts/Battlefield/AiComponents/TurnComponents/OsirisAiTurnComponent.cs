using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OsirisAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{
    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {
        //Play Arsenic if none in play
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Elite Eternity"));
        yield return aiManager.StartCoroutine(PlayShield(aiManager, "Turtle Shield"));

        //Play Creatures
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Trebuchet"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Shard of Focus"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Pharaoh"));

        yield return aiManager.StartCoroutine(ActivateRepeatSpellNoTarget(aiManager, "Elite Precognition", "Elite Precognition"));
        yield return aiManager.StartCoroutine(ActivateRepeatAbilityNoTarget(aiManager, CardType.Creature, "Elite Pharaoh", "Elite Pharaoh"));
        yield return aiManager.StartCoroutine(ActivateRepeatAbilityWithTarget(aiManager, CardType.Creature, "Elite Shard of Focus", "Shard of Focus", x => x.Owner.Equals(OwnerEnum.Player)));
        yield return aiManager.StartCoroutine(ActivateRepeatAbilityWithTarget(aiManager, CardType.Creature, "Elite Eternity", "Elite Eternity", x => x.Owner.Equals(OwnerEnum.Player)));
        yield return aiManager.StartCoroutine(ActivateRepeatAbilityWithTarget(aiManager, CardType.Creature, "Trebuchet", "Trebuchet", x => x.Owner.Equals(OwnerEnum.Opponent)));
        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Rewind", "Rewind", x => x.Owner.Equals(OwnerEnum.Player)));
        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Unstoppable", "Unstoppable", x => x.Owner.Equals(OwnerEnum.Opponent)));

    }
}
