using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AiBaseFunctions
{
    public CreatureAbilities creatureManager = new CreatureAbilities();
    public SpellAbilities spellManager = new SpellAbilities();
    public PermanentAbilities permanentManager = new PermanentAbilities();

    public IEnumerator PlayPillars(PlayerManager aiManager)
    {

        List<Card> cardList = aiManager.GetHandCards();
        List<ID> idList = aiManager.GetHandIds();

        int cardIndex = cardList.FindIndex(card => card.cardType == CardType.Pillar); int loopBreak = 0;
        while (cardIndex != -1 && loopBreak < 7)
        {
            loopBreak++;

            if (cardIndex == -1) { yield break; }
            aiManager.PlayCardFromHandLogic(idList[cardIndex]);

            cardList = new List<Card>(aiManager.GetHandCards());
            idList = new List<ID>(aiManager.GetHandIds());

            cardIndex = cardList.FindIndex(card => card.cardType == CardType.Pillar);
            yield return new WaitForSeconds(BattleVars.shared.aiPlaySpeed);
        }
    }

    public IEnumerator PlayPermanent(PlayerManager aiManager, string name)
    {
        List<Card> cardList = new List<Card>(aiManager.GetHandCards());
        List<ID> idList = new List<ID>(aiManager.GetHandIds());

        int cardIndex = cardList.FindIndex(card => card.cardName == name);
        int loopBreak = 0;
        while (cardIndex != -1 && loopBreak < 7)
        {
            loopBreak++;

            if (cardIndex == -1) { yield break; }
            if (aiManager.playerQuantaManager.HasEnoughQuanta(cardList[cardIndex].costElement, cardList[cardIndex].cost))
            {
                aiManager.PlayCardFromHandLogic(idList[cardIndex]);
            }
            else
            {
                yield break;
            }

            cardList = new List<Card>(aiManager.GetHandCards());
            idList = new List<ID>(aiManager.GetHandIds());
            cardIndex = cardList.FindIndex(card => card.cardName == name);
            yield return new WaitForSeconds(BattleVars.shared.aiPlaySpeed);
        }
    }


    public IEnumerator PlayWeapon(PlayerManager aiManager, string weaponName)
    {

        List<Card> cardList = new List<Card>(aiManager.GetHandCards());
        List<ID> idList = new List<ID>(aiManager.GetHandIds());
        if (aiManager.playerPassiveManager.GetWeapon() != null)
        {
            if (aiManager.playerPassiveManager.GetWeapon().cardName != "Weapon") { yield break; }
        }
        int cardIndex = cardList.FindIndex(card => card.cardName == weaponName);


        if (cardIndex == -1) { yield break; }

        if (aiManager.playerQuantaManager.HasEnoughQuanta(cardList[cardIndex].costElement, cardList[cardIndex].cost))
        {
            aiManager.PlayCardFromHandLogic(idList[cardIndex]);
        }
        yield return new WaitForSeconds(BattleVars.shared.aiPlaySpeed);
    }

    public IEnumerator PlayShield(PlayerManager aiManager, string shieldName)
    {

        List<Card> cardList = new List<Card>(aiManager.GetHandCards());
        List<ID> idList = new List<ID>(aiManager.GetHandIds());
        if (aiManager.playerPassiveManager.GetShield() != null)
        {
            if (aiManager.playerPassiveManager.GetShield().cardName != "Shield_") { yield break; }
        }
        int cardIndex = cardList.FindIndex(card => card.cardName == shieldName);

        if (cardIndex == -1) { yield break; }

        if (aiManager.playerQuantaManager.HasEnoughQuanta(cardList[cardIndex].costElement, cardList[cardIndex].cost))
        {
            aiManager.PlayCardFromHandLogic(idList[cardIndex]);
        }
        yield return new WaitForSeconds(BattleVars.shared.aiPlaySpeed);
    }

    public IEnumerator ActivateRepeatSpellNoTarget(PlayerManager aiManager, string regularName, string uppedName)
    {
        List<Card> cardList = new List<Card>(aiManager.GetHandCards());
        List<ID> idList = new List<ID>(aiManager.GetHandIds());

        if (cardList.Count == 0) { yield break; }
        int cardIndex = cardList.FindIndex(x => (x.cardName == regularName || x.cardName == uppedName) && aiManager.IsCardPlayable(x));
        if (cardIndex == -1) { yield break; }
        int loopBreak = 0;
        while (cardIndex != -1 && loopBreak < 7)
        {
            loopBreak++;
            //Play Spell
            BattleVars.shared.originId = idList[cardIndex];
            BattleVars.shared.cardOnStandBy = cardList[cardIndex];
            yield return aiManager.StartCoroutine(aiManager.ActivateAbility(idList[cardIndex]));


            cardList = new List<Card>(aiManager.GetHandCards());
            idList = new List<ID>(aiManager.GetHandIds());
            //Check if still has explosion in hand, repeat loop
            cardIndex = cardList.FindIndex(x => (x.cardName == regularName || x.cardName == uppedName) && aiManager.IsCardPlayable(x));
        }
    }

    public IEnumerator ActivateRepeatSpellWithTarget(PlayerManager aiManager, string regularName, string uppedName, Predicate<ID> targetPredicate)
    {
        List<Card> cardList = new List<Card>(aiManager.GetHandCards());
        List<ID> idList = new List<ID>(aiManager.GetHandIds());

        if (cardList.Count == 0) { yield break; }
        int cardIndex = cardList.FindIndex(x => (x.cardName == regularName || x.cardName == uppedName) && aiManager.IsCardPlayable(x));
        if (cardIndex == -1) { yield break; }
        int loopBreak = 0;
        while (cardIndex != -1 && loopBreak < 7)
        {
            loopBreak++;
            //Play Spell
            BattleVars.shared.originId = idList[cardIndex];
            BattleVars.shared.cardOnStandBy = cardList[cardIndex];

            SkillManager.Instance.SetupTargetHighlights(aiManager, DuelManager.player, BattleVars.shared.cardOnStandBy);

            List<ID> opCreatureIds = DuelManager.GetAllValidTargets();
            opCreatureIds = opCreatureIds.FindAll(targetPredicate);
            if (opCreatureIds.Count == 0) { DuelManager.ResetTargeting(); yield break; }
            System.Random rnd = new System.Random();
            ID target = opCreatureIds.OrderBy(x => rnd.Next())
                              .First();


            yield return aiManager.StartCoroutine(aiManager.ActivateAbility(target));

            cardList = new List<Card>(aiManager.GetHandCards());
            idList = new List<ID>(aiManager.GetHandIds());
            //Check if still has explosion in hand, repeat loop
            cardIndex = cardList.FindIndex(x => (x.cardName == regularName || x.cardName == uppedName) && aiManager.IsCardPlayable(x));
        }
    }

    public IEnumerator ActivateRepeatAbilityWithTarget(PlayerManager aiManager, CardType cardType, string regularName, string uppedName, Predicate<ID> targetPredicate)
    {
        List<Card> cardList = cardType.Equals(CardType.Creature) ? aiManager.playerCreatureField.GetAllCards() : aiManager.playerPermanentManager.GetAllCards();
        List<ID> idList = cardType.Equals(CardType.Creature) ? aiManager.playerCreatureField.GetAllIds() : aiManager.playerPermanentManager.GetAllIds();

        if (cardList.Count == 0) { yield break; }
        int cardIndex = cardList.FindIndex(x => (x.cardName == regularName || x.cardName == uppedName) && aiManager.IsAbilityUsable(x));
        if (cardIndex == -1) { yield break; }
        int loopBreak = 0;
        while (cardIndex != -1 && loopBreak < 7)
        {
            loopBreak++;

            BattleVars.shared.originId = idList[cardIndex];
            BattleVars.shared.cardOnStandBy = cardList[cardIndex];

            SkillManager.Instance.SetupTargetHighlights(aiManager, DuelManager.player, BattleVars.shared.cardOnStandBy);

            List<ID> opCreatureIds = DuelManager.GetAllValidTargets();
            opCreatureIds = opCreatureIds.FindAll(targetPredicate);
            if (opCreatureIds.Count == 0) { DuelManager.ResetTargeting(); yield break; }
            System.Random rnd = new System.Random();
            ID target = opCreatureIds.OrderBy(x => rnd.Next())
                              .First();


            yield return aiManager.StartCoroutine(aiManager.ActivateAbility(target));


            cardList = cardType.Equals(CardType.Creature) ? aiManager.playerCreatureField.GetAllCards() : aiManager.playerPermanentManager.GetAllCards();
            idList = cardType.Equals(CardType.Creature) ? aiManager.playerCreatureField.GetAllIds() : aiManager.playerPermanentManager.GetAllIds();
            //Check if still has explosion in hand, repeat loop
            cardIndex = cardList.FindIndex(x => (x.cardName == regularName || x.cardName == uppedName) && aiManager.IsAbilityUsable(x));
        }
    }

    public IEnumerator ActivateRepeatAbilityNoTarget(PlayerManager aiManager, CardType cardType,string regularName, string uppedName)
    {
        List<Card> cardList = cardType.Equals(CardType.Creature) ? aiManager.playerCreatureField.GetAllCards() : aiManager.playerPermanentManager.GetAllCards();
        List<ID> idList = cardType.Equals(CardType.Creature) ? aiManager.playerCreatureField.GetAllIds() : aiManager.playerPermanentManager.GetAllIds();

        if (cardList.Count == 0) { yield break; }
        int cardIndex = cardList.FindIndex(x => (x.cardName == regularName || x.cardName == uppedName) && aiManager.IsAbilityUsable(x));
        if (cardIndex == -1) { yield break; }
        int loopBreak = 0;
        while (cardIndex != -1 && loopBreak < 7)
        {
            loopBreak++;
            //Play Spell
            BattleVars.shared.originId = idList[cardIndex];
            BattleVars.shared.cardOnStandBy = cardList[cardIndex];
            yield return aiManager.StartCoroutine(aiManager.ActivateAbility(idList[cardIndex]));

            cardList = cardType.Equals(CardType.Creature) ? aiManager.playerCreatureField.GetAllCards() : aiManager.playerPermanentManager.GetAllCards();
            idList = cardType.Equals(CardType.Creature) ? aiManager.playerCreatureField.GetAllIds() : aiManager.playerPermanentManager.GetAllIds();
            //Check if still has explosion in hand, repeat loop
            cardIndex = cardList.FindIndex(x => (x.cardName == regularName || x.cardName == uppedName) && aiManager.IsAbilityUsable(x));
        }
    }

}