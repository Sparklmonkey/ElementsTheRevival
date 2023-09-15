using System.Collections;
using UnityEngine;


public class BaseAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{

    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {

        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, CardType.Creature));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, CardType.Artifact));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, CardType.Weapon));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, CardType.Shield));
        yield return aiManager.StartCoroutine(ActivateSpells(aiManager));

        yield return aiManager.StartCoroutine(ActivateAbilities(aiManager, CardType.Creature));
        yield return aiManager.StartCoroutine(ActivateAbilities(aiManager, CardType.Artifact));
        yield return aiManager.StartCoroutine(ActivateAbilities(aiManager, CardType.Weapon));

        yield break;
    }

    private IEnumerator PlayPermanent(PlayerManager aiManager, CardType cardType)
    {
        var idCardList = aiManager.playerHand.GetAllValidCardIds();

        if (!idCardList.Exists(x => x.card.cardType.Equals(cardType))) { yield break; }
        int cardIndex = idCardList.FindIndex(x => x.card.cardType.Equals(cardType) && aiManager.IsCardPlayable(x.card));
        if (cardIndex == -1) { yield break; }

        for (int i = 0; i < 7; i++)
        {
            if (cardIndex == -1) { yield break; }
            aiManager.PlayCardFromHandLogic(idCardList[cardIndex]);

            idCardList = aiManager.playerHand.GetAllValidCardIds();
            cardIndex = idCardList.FindIndex(x => x.card.cardType.Equals(cardType) && aiManager.IsCardPlayable(x.card));
            yield return new WaitForSeconds(BattleVars.Shared.AIPlaySpeed);
        }
    }

}