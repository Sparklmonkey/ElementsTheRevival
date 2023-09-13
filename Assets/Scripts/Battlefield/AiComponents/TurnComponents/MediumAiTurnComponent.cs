using System.Collections;
using UnityEngine;


public class MediumAiTurnComponent : AiBaseFunctions, IAiTurnComponent
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
        if (cardType.Equals(CardType.Shield))
        {
            if (aiManager.playerPassiveManager.GetShield().card.skill != "none")
            {
                yield break;
            }
        }
        if (cardType.Equals(CardType.Weapon))
        {
            if (aiManager.playerPassiveManager.GetWeapon().card.skill != "none")
            {
                yield break;
            }
        }

        var idCardList = aiManager.playerHand.GetAllValidCardIds();

        if (!idCardList.Exists(x => x.card.cardType.Equals(cardType))) { yield break; }
        int cardIndex = idCardList.FindIndex(x => x.card.cardType.Equals(cardType) && aiManager.IsCardPlayable(x.card));
        if (cardIndex == -1) { yield break; }
        int loopBreak = 0;
        while (cardIndex != -1 && loopBreak < 7)
        {
            loopBreak++;

            if (DuelManager.Instance.GetCardCount(new() { "5rp", "7q9" }) > 0 && (idCardList[cardIndex].card.iD == "5rp" || idCardList[cardIndex].card.iD == "7q9"))
            {
                continue;
            }

            aiManager.PlayCardFromHandLogic(idCardList[cardIndex]);

            idCardList = aiManager.playerHand.GetAllValidCardIds();
            cardIndex = idCardList.FindIndex(x => x.card.cardType.Equals(cardType) && aiManager.IsCardPlayable(x.card));
            yield return new WaitForSeconds(BattleVars.shared.aiPlaySpeed);
        }
    }



}