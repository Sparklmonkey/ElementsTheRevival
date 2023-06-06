using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IncarnateAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{
    private IEnumerator ActivateBloodsucker(PlayerManager aiManager)
    {
        if (DuelManager.GetOtherPlayer().playerCreatureField.GetAllCards().Count < 1) { yield break; }

        List<Card> cardList = new List<Card>(aiManager.playerCreatureField.GetAllCards());
        List<ID> idList = new List<ID>(aiManager.playerCreatureField.GetAllIds());
        List<Card> bloodsuckerCards = new List<Card>();
        List<ID> bloodsuckerIds = new List<ID>();

        for (int i = 0; i < cardList.Count; i++)
        {
            switch (cardList[i].iD)
            {
                case "7t7":
                    bloodsuckerCards.Add(cardList[i]);
                    bloodsuckerIds.Add(idList[i]);
                    break;
                default:
                    break;
            }
        }

        if (bloodsuckerCards.Count == 0) { yield break; }


        for (int i = 0; i < bloodsuckerCards.Count; i++)
        {
            if (!aiManager.IsAbilityUsable(bloodsuckerCards[i])) { continue; }
            BattleVars.shared.originId = bloodsuckerIds[i];
            BattleVars.shared.cardOnStandBy = bloodsuckerCards[i];

            SkillManager.Instance.SetupTargetHighlights(aiManager, DuelManager.Instance.player, BattleVars.shared.cardOnStandBy);
            idList = DuelManager.GetAllValidTargets();
            if (idList.Count == 0) { yield break; }

            System.Random rnd = new System.Random();
            ID target = idList.OrderBy(x => rnd.Next())
                              .First();
            yield return aiManager.StartCoroutine(aiManager.ActivateAbility(target));
        }

    }


    private IEnumerator ActivateRetroVirus(PlayerManager aiManager)
    {

        if (DuelManager.GetOtherPlayer().playerCreatureField.GetAllCards().Count < 2) { yield break; }

        List<Card> cardList = new List<Card>(aiManager.playerCreatureField.GetAllCards());
        List<ID> idList = new List<ID>(aiManager.playerCreatureField.GetAllIds());
        List<Card> retroVirusCards = new List<Card>();
        List<ID> retroVirusIds = new List<ID>();

        for (int i = 0; i < cardList.Count; i++)
        {
            switch (cardList[i].cardName)
            {
                case "Retrovirus":
                    retroVirusCards.Add(cardList[i]);
                    retroVirusIds.Add(idList[i]);
                    break;
                default:
                    break;
            }
        }

        if (retroVirusCards.Count == 0) { yield break; }

        for (int i = 0; i < retroVirusCards.Count; i++)
        {
            if (!aiManager.IsAbilityUsable(retroVirusCards[i])) { continue; }
            BattleVars.shared.originId = retroVirusIds[i];
            BattleVars.shared.cardOnStandBy = retroVirusCards[i];

            ID target = new ID(OwnerEnum.Opponent, FieldEnum.Player, 1);
            yield return aiManager.StartCoroutine(aiManager.ActivateAbility(target));
        }

    }

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
        yield return aiManager.StartCoroutine(ActivateBloodsucker(aiManager));
        //Activate Retrovirus
        yield return aiManager.StartCoroutine(ActivateRetroVirus(aiManager));
    }
}
