using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAbilities
{
    public IEnumerator ActivatePegasus(PlayerManager aiManager)
    {
        List<Card> cardList = new List<Card>(aiManager.playerCreatureField.GetAllCards());
        List<ID> idList = new List<ID>(aiManager.playerCreatureField.GetAllIds());
        List<Card> queenCards = new List<Card>();
        List<ID> queenIds = new List<ID>();

        for (int i = 0; i < cardList.Count; i++)
        {
            switch (cardList[i].cardName)
            {
                case "Elite Pegasus":
                    queenCards.Add(cardList[i]);
                    queenIds.Add(idList[i]);
                    break;
                case "Pegasus":
                    queenCards.Add(cardList[i]);
                    queenIds.Add(idList[i]);
                    break;
                default:
                    break;
            }
        }

        if (queenCards.Count == 0) { yield break; }

        for (int i = 0; i < queenCards.Count; i++)
        {
            if (!aiManager.IsAbilityUsable(queenCards[i])) { continue; }
            BattleVars.shared.originId = queenIds[i];
            BattleVars.shared.cardOnStandBy = queenCards[i];

            yield return aiManager.StartCoroutine(aiManager.ActivateAbility(BattleVars.shared.originId));
        }

    }

    public IEnumerator ActivateRetroVirus(PlayerManager aiManager)
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
                case "Virus":
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
            ID target = new ID(OwnerEnum.Player, FieldEnum.Player, 1);
            if (BattleVars.shared.isSelectingTarget)
            {
                SkillManager.Instance.SetupTargetHighlights(aiManager, DuelManager.player, BattleVars.shared.cardOnStandBy);
                target = DuelManager.GetAllValidTargets().Find(x => x.Owner.Equals(OwnerEnum.Player));
                if(target == null) { continue; }
            }

            yield return aiManager.StartCoroutine(aiManager.ActivateAbility(target));
        }

    }

    public IEnumerator ActivateGraboid(PlayerManager aiManager)
    {
        List<Card> cardList = new List<Card>(aiManager.playerCreatureField.GetAllCards());
        List<ID> idList = new List<ID>(aiManager.playerCreatureField.GetAllIds());

        if (cardList.Count == 0) { yield break; }

        List<Card> graboidCards = new List<Card>();
        List<ID> graboidIds = new List<ID>();

        for (int i = 0; i < cardList.Count; i++)
        {
            switch (cardList[i].cardName)
            {
                case "Elite Graboid":
                    graboidCards.Add(cardList[i]);
                    graboidIds.Add(idList[i]);
                    break;
                case "Graboid":
                    graboidCards.Add(cardList[i]);
                    graboidIds.Add(idList[i]);
                    break;
                default:
                    break;
            }
        }

        if (graboidCards.Count == 0) { yield break; }

        for (int i = 0; i < graboidCards.Count; i++)
        {
            if (!aiManager.IsAbilityUsable(graboidCards[i])) { continue; }
            BattleVars.shared.originId = graboidIds[i];
            BattleVars.shared.cardOnStandBy = graboidCards[i];

            yield return aiManager.StartCoroutine(aiManager.ActivateAbility(graboidIds[i]));
        }
    }

}
