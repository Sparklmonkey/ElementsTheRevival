using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SerketAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{
    private int cloakCount = 0;

    private IEnumerator ActivateNymphAbility(PlayerManager aiManager)
    {
        List<Card> cardList = new List<Card>(aiManager.playerCreatureField.GetAllCards());
        List<ID> idList = new List<ID>(aiManager.playerCreatureField.GetAllIds());

        if(cardList.Count == 0) { yield break; }

        List<int> nymphIndex = Enumerable.Range(0, cardList.Count)
             .Where(i => cardList[i].cardName == "Life Nymph")
             .ToList();

        if (nymphIndex.Count == 0) { yield break; }

        List<int> scorpionIndex = Enumerable.Range(0, cardList.Count)
             .Where(i => cardList[i].cardName == "Scorpion")
             .ToList();

        List<int> deathStalkerIndex = Enumerable.Range(0, cardList.Count)
             .Where(i => cardList[i].cardName == "Elite Deathstalker")
             .ToList();

        List<int> recluseIndex = Enumerable.Range(0, cardList.Count)
             .Where(i => cardList[i].cardName == "Flesh Recluse")
             .ToList();


        for (int i = 0; i < nymphIndex.Count; i++)
        {
            if (!aiManager.IsAbilityUsable(cardList[nymphIndex[i]])) { break; }
            BattleVars.shared.originId = idList[nymphIndex[i]];
            BattleVars.shared.cardOnStandBy = cardList[nymphIndex[i]];

            if(scorpionIndex.Count > 0)
            {
                for (int j = 0; j < scorpionIndex.Count; j++)
                {
                    if (cardList[scorpionIndex[j]].passive.Contains("adrenaline")) { continue; }
                    yield return aiManager.StartCoroutine(aiManager.ActivateAbility(idList[scorpionIndex[j]]));
                }
            }

            if (deathStalkerIndex.Count > 0)
            {
                for (int j = 0; j < deathStalkerIndex.Count; j++)
                {
                    if (cardList[deathStalkerIndex[j]].passive.Contains("adrenaline")) { continue; }
                    yield return aiManager.StartCoroutine(aiManager.ActivateAbility(idList[deathStalkerIndex[j]]));
                }
            }

            if (recluseIndex.Count > 0)
            {
                for (int j = 0; j < recluseIndex.Count; j++)
                {
                    if (cardList[recluseIndex[j]].passive.Contains("adrenaline")) { continue; }
                    yield return aiManager.StartCoroutine(aiManager.ActivateAbility(idList[recluseIndex[j]]));
                }
            }

            //if (nymphIndex.Count > 0)
            //{
            //    for (int j = 0; j < nymphIndex.Count; j++)
            //    {
            //        if (cardList[nymphIndex[j]].cardPassives.hasAdrenaline) { continue; }
            //        yield return aiManager.StartCoroutine(aiManager.ActivateAbility(idList[nymphIndex[j]]));
            //    }
            //}

            DuelManager.Instance.ResetTargeting();
            //If ability is still usable, that means there are no more creatures to use ability on
            if (aiManager.IsAbilityUsable(cardList[nymphIndex[i]]))
            {
                yield break;
            }
        }

    }

    private IEnumerator ActivateWebAbility(PlayerManager aiManager)
    {
        List<Card> cardList = new List<Card>(aiManager.playerCreatureField.GetAllCards());
        List<ID> idList = new List<ID>(aiManager.playerCreatureField.GetAllIds());

        if (cardList.Count == 0) { yield break; }

        List<Card> fleshRecluseCards = new List<Card>();
        List<ID> fleshRecluseIds = new List<ID>();

        for (int i = 0; i < cardList.Count; i++)
        {
            switch (cardList[i].cardName)
            {
                case "Flesh Recluse":
                    fleshRecluseCards.Add(cardList[i]);
                    fleshRecluseIds.Add(idList[i]);
                    break;
                default:
                    break;
            }
        }

        if (fleshRecluseCards.Count == 0) { yield break; }

        for (int i = 0; i < fleshRecluseCards.Count; i++)
        {
            if (!aiManager.IsAbilityUsable(fleshRecluseCards[i])) { continue; }
            BattleVars.shared.originId = fleshRecluseIds[i];
            BattleVars.shared.cardOnStandBy = fleshRecluseCards[i];

            SkillManager.Instance.SetupTargetHighlights(aiManager, DuelManager.Instance.player, BattleVars.shared.cardOnStandBy);

            List<ID> validTargets = DuelManager.GetAllValidTargets();

            if (validTargets.Count == 0)
            {
                DuelManager.Instance.ResetTargeting();
                break;
            }

            yield return aiManager.StartCoroutine(aiManager.ActivateAbility(validTargets[0]));
        }
    }

    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {
        cloakCount--;
        //Play Arsenic if none in play
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Elite Arsenic"));

        //Play Creatures
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Scorpion"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Deathstalker"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Life Nymph"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Flesh Recluse"));

        //Play Eclipse
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Eclipse"));

        //Activate Life Nymphs on Death stalkers and Scorpions
        yield return aiManager.StartCoroutine(ActivateNymphAbility(aiManager));
        //Activate Web on flying creatures
        yield return aiManager.StartCoroutine(ActivateWebAbility(aiManager));
        //Activate Cloak if no cloak active
        if (cloakCount <= 0)
        {
            yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Cloak"));
        }
    }
}
