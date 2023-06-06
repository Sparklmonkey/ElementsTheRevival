using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


public class BaseAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{

    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {

        Debug.Log("Play Creatures");
        //Play Creatures
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, CardType.Creature));
        //Play Artifacts
        Debug.Log("Play Artifacts");
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, CardType.Artifact));
        //Play Weapon
        Debug.Log("Play Weapon");
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, CardType.Weapon));
        //Play Shield
        Debug.Log("Play Shield");
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, CardType.Shield));

        //Activate Spells
        Debug.Log("Play Spells");
        yield return aiManager.StartCoroutine(ActivateSpells(aiManager));


        //Activate Creature Abilities
        Debug.Log("Activate Creatures");
        yield return aiManager.StartCoroutine(ActivateAbilities(aiManager, CardType.Creature));
        Debug.Log("Activate Artifacts");
        yield return aiManager.StartCoroutine(ActivateAbilities(aiManager, CardType.Artifact));
        Debug.Log("Activate Weapon");
        yield return aiManager.StartCoroutine(ActivateAbilities(aiManager, CardType.Weapon));

        yield break;
    }
    private Dictionary<ID, int> threatList;

    private IEnumerator PlayPermanent(PlayerManager aiManager, CardType cardType)
    {
        //Get Card list
        List<Card> cardList = new List<Card>(aiManager.GetHandCards());
        List<ID> idList = new List<ID>(aiManager.GetHandIds());

        if (!cardList.Exists(x => x.cardType.Equals(cardType))) { yield break; }
        int cardIndex = cardList.FindIndex(x => x.cardType.Equals(cardType) && aiManager.IsCardPlayable(x));
        if (cardIndex == -1) { yield break; }
        int loopBreak = 0;
        while (cardIndex != -1 && loopBreak < 7)
        {
            loopBreak++;
            aiManager.PlayCardFromHandLogic(idList[cardIndex]);

            cardList = new List<Card>(aiManager.GetHandCards());
            idList = new List<ID>(aiManager.GetHandIds());
            cardIndex = cardList.FindIndex(x => x.cardType.Equals(cardType) && aiManager.IsCardPlayable(x));
            yield return new WaitForSeconds(BattleVars.shared.aiPlaySpeed);
        }
        yield return new WaitForSeconds(BattleVars.shared.aiPlaySpeed);
    }

    private IEnumerator ActivateSpells(PlayerManager aiManager)
    {
        List<Card> cardList = new List<Card>(aiManager.GetHandCards());
        List<ID> idList = new List<ID>(aiManager.GetHandIds());

        if (!cardList.Exists(x => x.cardType.Equals(CardType.Spell))) { yield break; }

        int cardIndex = cardList.FindIndex(x => x.cardType.Equals(CardType.Spell) && aiManager.IsCardPlayable(x));

        if(cardIndex == -1) { yield break; }
        int loopBreak = 0;
        while(cardIndex != -1 && loopBreak < 8)
        {
            loopBreak++;
            Card cardToCheck = cardList[cardIndex];
            //Setup Spell
            BattleVars.shared.originId = idList[cardIndex];
            BattleVars.shared.cardOnStandBy = cardToCheck;
            ID target = idList[cardIndex];

            if (!BattleVars.shared.IsFixedTarget())
            {
                //Get Target
                SkillManager.Instance.SetupTargetHighlights(aiManager, DuelManager.Instance.player, BattleVars.shared.cardOnStandBy);
                //Get List of all Valid Targets
                List<ID> allValidTargets = DuelManager.GetAllValidTargets();
                //If there is no valid targets, reset and skip to next iteration
                if (allValidTargets.Count == 0)
                {
                    DuelManager.Instance.ResetTargeting();
                    cardList = new List<Card>(aiManager.GetHandCards());
                    idList = new List<ID>(aiManager.GetHandIds());
                    cardIndex = cardList.FindIndex(x => x.cardType.Equals(CardType.Spell) && aiManager.IsCardPlayable(x) && x.cardName != cardToCheck.cardName);
                    continue;
                }
                //Get Random Target
                System.Random rnd = new System.Random();
                target = allValidTargets.OrderBy(x => rnd.Next())
                                  .First();

            }

            yield return aiManager.StartCoroutine(aiManager.ActivateAbility(target));

            yield return new WaitForSeconds(BattleVars.shared.aiPlaySpeed);
            cardList = new List<Card>(aiManager.GetHandCards());
            idList = new List<ID>(aiManager.GetHandIds());
            cardIndex = cardList.FindIndex(x => x.cardType.Equals(CardType.Spell) && aiManager.IsCardPlayable(x));
        }
    }

    private IEnumerator ActivateAbilities(PlayerManager aiManager, CardType cardType)
    {
        List<Card> cardList = new List<Card>();
        List<ID> idList = new List<ID>();

        switch (cardType)
        {
            case CardType.Creature:
                cardList = aiManager.playerCreatureField.GetAllCards();
                idList = aiManager.playerCreatureField.GetAllIds();
                if (cardList.Count == 0) { yield break; }
                break;
            case CardType.Artifact:
                cardList = aiManager.playerPermanentManager.GetAllCards();
                idList = aiManager.playerPermanentManager.GetAllIds();
                if (cardList.Count == 0) { yield break; }
                break;
            case CardType.Weapon:
                if (aiManager.playerPassiveManager.GetWeapon() == null) { yield break; }
                if (aiManager.playerPassiveManager.GetWeapon().cardName == "Weapon") { yield break; }
                cardList = new List<Card> { aiManager.playerPassiveManager.GetWeapon() };
                idList = new List<ID> { aiManager.playerPassiveManager.GetWeaponID() };
                break;
            default:
                break;
        }


        int cardIndex = cardList.FindIndex(x => aiManager.IsAbilityUsable(x));

        if (cardIndex == -1) { yield break; }

        int loopBreak = 0;
        while (cardIndex != -1 && loopBreak < 10)
        {
            loopBreak++;
            Card cardToCheck = cardList[cardIndex];
            //Setup Spell
            BattleVars.shared.originId = idList[cardIndex];
            BattleVars.shared.cardOnStandBy = cardToCheck;
            ID target = BattleVars.shared.originId;

            if (BattleVars.shared.isSelectingTarget)
            {
                //Get Target
                SkillManager.Instance.SetupTargetHighlights(aiManager, DuelManager.Instance.player, BattleVars.shared.cardOnStandBy);
                //Get List of all Valid Targets
                List<ID> allValidTargets = DuelManager.GetAllValidTargets();
                //If there is no valid targets, reset and skip to next iteration
                if (allValidTargets.Count == 0)
                {
                    DuelManager.Instance.ResetTargeting();
                    cardIndex = cardList.FindIndex(x => aiManager.IsAbilityUsable(x) && x.cardName != cardToCheck.cardName);
                    continue;
                }
                //Get Random Target
                System.Random rnd = new System.Random();
                target = allValidTargets.OrderBy(x => rnd.Next())
                                  .First();

            }

            yield return aiManager.StartCoroutine(aiManager.ActivateAbility(target));
            cardIndex = cardList.FindIndex(x => aiManager.IsAbilityUsable(x));
        }
    }

}