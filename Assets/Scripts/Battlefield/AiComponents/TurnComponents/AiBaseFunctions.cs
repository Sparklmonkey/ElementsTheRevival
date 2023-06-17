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
        var idCardList = aiManager.playerHand.GetAllValidCardIds();

        int cardIndex = idCardList.FindIndex(x => x.card.cardType == CardType.Pillar);
        int loopBreak = 0;
        while (cardIndex != -1 && loopBreak < 7)
        {
            loopBreak++;

            aiManager.PlayCardFromHandLogic(idCardList[cardIndex].id);

            idCardList = aiManager.playerHand.GetAllValidCardIds();

            cardIndex = idCardList.FindIndex(x => x.card.cardType == CardType.Pillar);
            yield return new WaitForSeconds(BattleVars.shared.aiPlaySpeed);
        }
    }

    public IEnumerator PlayPermanent(PlayerManager aiManager, string name)
    {
        var idCardList = aiManager.playerHand.GetAllValidCardIds();

        int cardIndex = idCardList.FindIndex(x => x.card.cardName == name);
        int loopBreak = 0;
        while (cardIndex != -1 && loopBreak < 7)
        {
            loopBreak++;

            if (cardIndex == -1) { yield break; }
            if (aiManager.playerQuantaManager.HasEnoughQuanta(idCardList[cardIndex].card.costElement, idCardList[cardIndex].card.cost))
            {
                aiManager.PlayCardFromHandLogic(idCardList[cardIndex].id);
            }
            else
            {
                yield break;
            }

            idCardList = aiManager.playerHand.GetAllValidCardIds();
            cardIndex = idCardList.FindIndex(x => x.card.cardName == name);
            yield return new WaitForSeconds(BattleVars.shared.aiPlaySpeed);
        }
    }


    public IEnumerator PlayWeapon(PlayerManager aiManager, string weaponName)
    {
        var idCardList = aiManager.playerHand.GetAllValidCardIds();
        if (aiManager.playerPassiveManager.GetWeapon().card.skill != "none")
        {
            yield break;
        }

        int cardIndex = idCardList.FindIndex(x => x.card.cardName == weaponName);


        if (cardIndex == -1) { yield break; }
        if (aiManager.playerQuantaManager.HasEnoughQuanta(idCardList[cardIndex].card.costElement, idCardList[cardIndex].card.cost))
        {
            aiManager.PlayCardFromHandLogic(idCardList[cardIndex].id);
        }
        yield return new WaitForSeconds(BattleVars.shared.aiPlaySpeed);
    }

    public IEnumerator PlayShield(PlayerManager aiManager, string shieldName)
    {
        var idCardList = aiManager.playerHand.GetAllValidCardIds();
        if (aiManager.playerPassiveManager.GetShield().card.skill != "none")
        {
            yield break;
        }
        int cardIndex = idCardList.FindIndex(x => x.card.cardName == shieldName);


        if (cardIndex == -1) { yield break; }
        if (aiManager.playerQuantaManager.HasEnoughQuanta(idCardList[cardIndex].card.costElement, idCardList[cardIndex].card.cost))
        {
            aiManager.PlayCardFromHandLogic(idCardList[cardIndex].id);
        }
        yield return new WaitForSeconds(BattleVars.shared.aiPlaySpeed);
    }

    public IEnumerator ActivateRepeatSpellNoTarget(PlayerManager aiManager, string regularName, string uppedName)
    {
        var idCardList = aiManager.playerHand.GetAllValidCardIds();
        if (idCardList.Count == 0) { yield break; }
        int cardIndex = idCardList.FindIndex(x => (x.card.cardName == regularName || x.card.cardName == uppedName) && aiManager.IsCardPlayable(x.card));
        if (cardIndex == -1) { yield break; }
        int loopBreak = 0;
        while (cardIndex != -1 && loopBreak < 7)
        {
            loopBreak++;
            //Play Spell
            BattleVars.shared.abilityOrigin = idCardList[cardIndex];
            aiManager.ActivateAbility(idCardList[cardIndex]);
            idCardList = aiManager.playerHand.GetAllValidCardIds();
            //Check if still has explosion in hand, repeat loop
            cardIndex = idCardList.FindIndex(x => (x.card.cardName == regularName || x.card.cardName == uppedName) && aiManager.IsCardPlayable(x.card));
        }
    }

    public IEnumerator ActivateRepeatSpellWithTarget(PlayerManager aiManager, string regularName, string uppedName)
    {
        var idCardList = aiManager.playerHand.GetAllValidCardIds();
        if (idCardList.Count == 0) { yield break; }
        int cardIndex = idCardList.FindIndex(x => (x.card.cardName == regularName || x.card.cardName == uppedName) && aiManager.IsCardPlayable(x.card));
        if (cardIndex == -1) { yield break; }
        int loopBreak = 0;
        while (cardIndex != -1 && loopBreak < 7)
        {
            loopBreak++;
            BattleVars.shared.abilityOrigin = idCardList[cardIndex];

            var target = SkillManager.Instance.GetRandomTarget(aiManager, idCardList[cardIndex]);
            if(target == null) { continue; }
            aiManager.ActivateAbility(target);

            idCardList = aiManager.playerHand.GetAllValidCardIds();
            cardIndex = idCardList.FindIndex(x => (x.card.cardName == regularName || x.card.cardName == uppedName) && aiManager.IsCardPlayable(x.card));
        }
    }

    public IEnumerator ActivateRepeatAbilityWithTarget(PlayerManager aiManager, CardType cardType, string regularName, string uppedName)
    {
        var idCardList = cardType.Equals(CardType.Creature) ? aiManager.playerCreatureField.GetAllValidCardIds() : aiManager.playerPermanentManager.GetAllValidCardIds();

        if (idCardList.Count == 0) { yield break; }
        int cardIndex = idCardList.FindIndex(x => (x.card.cardName == regularName || x.card.cardName == uppedName) && aiManager.IsAbilityUsable(x.card));
        if (cardIndex == -1) { yield break; }
        int loopBreak = 0;
        while (cardIndex != -1 && loopBreak < 7)
        {
            loopBreak++;
            BattleVars.shared.abilityOrigin = idCardList[cardIndex];

            var target = SkillManager.Instance.GetRandomTarget(aiManager, idCardList[cardIndex]);
            if (target == null) { continue; }
            aiManager.ActivateAbility(target);

            idCardList = cardType.Equals(CardType.Creature) ? aiManager.playerCreatureField.GetAllValidCardIds() : aiManager.playerPermanentManager.GetAllValidCardIds();
            cardIndex = idCardList.FindIndex(x => (x.card.cardName == regularName || x.card.cardName == uppedName) && aiManager.IsAbilityUsable(x.card));
        }
    }

    public IEnumerator ActivateRepeatAbilityNoTarget(PlayerManager aiManager, CardType cardType,string regularName, string uppedName)
    {
        var idCardList = cardType.Equals(CardType.Creature) ? aiManager.playerCreatureField.GetAllValidCardIds() : aiManager.playerPermanentManager.GetAllValidCardIds();

        if (idCardList.Count == 0) { yield break; }
        int cardIndex = idCardList.FindIndex(x => (x.card.cardName == regularName || x.card.cardName == uppedName) && aiManager.IsAbilityUsable(x.card));
        if (cardIndex == -1) { yield break; }
        int loopBreak = 0;
        while (cardIndex != -1 && loopBreak < 7)
        {
            loopBreak++;
            BattleVars.shared.abilityOrigin = idCardList[cardIndex];
            aiManager.ActivateAbility(idCardList[cardIndex]);
            idCardList = cardType.Equals(CardType.Creature) ? aiManager.playerCreatureField.GetAllValidCardIds() : aiManager.playerPermanentManager.GetAllValidCardIds();
            cardIndex = idCardList.FindIndex(x => (x.card.cardName == regularName || x.card.cardName == uppedName) && aiManager.IsAbilityUsable(x.card));
        }
    }

    public IEnumerator ActivateSpells(PlayerManager aiManager)
    {
        var idCardList = aiManager.playerHand.GetAllValidCardIds();

        if (!idCardList.Exists(x => x.card.cardType.Equals(CardType.Spell))) { yield break; }

        int cardIndex = idCardList.FindIndex(x => x.card.cardType.Equals(CardType.Spell) && aiManager.IsCardPlayable(x.card));

        if (cardIndex == -1) { yield break; }
        int loopBreak = 0;
        while (cardIndex != -1 && loopBreak < 8)
        {
            loopBreak++;
            Card cardToCheck = idCardList[cardIndex].card;
            //Setup Spell
            BattleVars.shared.abilityOrigin = idCardList[cardIndex];

            if (SkillManager.Instance.ShouldAskForTarget(BattleVars.shared.abilityOrigin))
            {
                var target = SkillManager.Instance.GetRandomTarget(aiManager, BattleVars.shared.abilityOrigin);
                if (target == null)
                {
                    idCardList = aiManager.playerHand.GetAllValidCardIds();
                    cardIndex = idCardList.FindIndex(x => x.card.cardType.Equals(CardType.Spell) && aiManager.IsCardPlayable(x.card) && x.card.cardName != cardToCheck.cardName);
                    continue;
                }

                aiManager.ActivateAbility(target);
            }
            else
            {
                aiManager.ActivateAbility(BattleVars.shared.abilityOrigin);
            }

            yield return new WaitForSeconds(BattleVars.shared.aiPlaySpeed);
            idCardList = aiManager.playerHand.GetAllValidCardIds();
            cardIndex = idCardList.FindIndex(x => x.card.cardType.Equals(CardType.Spell) && aiManager.IsCardPlayable(x.card) && x.card.cardName != cardToCheck.cardName);
        }
    }

    public IEnumerator ActivateAbilities(PlayerManager aiManager, CardType cardType)
    {
        var idCardList = new List<IDCardPair>();
        switch (cardType)
        {
            case CardType.Creature:
                idCardList = aiManager.playerCreatureField.GetAllValidCardIds();
                if (idCardList.Count == 0) { yield break; }
                break;
            case CardType.Artifact:
                idCardList = aiManager.playerPermanentManager.GetAllValidCardIds().FindAll(x => x.card.cardType == CardType.Artifact);
                if (idCardList.Count == 0) { yield break; }
                break;
            case CardType.Weapon:
                if (aiManager.playerPassiveManager.GetWeapon().card.cardName == "Weapon") { yield break; }
                idCardList.Add(aiManager.playerPassiveManager.GetWeapon());
                break;
            default:
                break;
        }


        int cardIndex = idCardList.FindIndex(x => aiManager.IsAbilityUsable(x.card));

        if (cardIndex == -1) { yield break; }

        int loopBreak = 0;
        while (cardIndex != -1 && loopBreak < 10)
        {
            Card cardToCheck = idCardList[cardIndex].card;
            //Setup Spell
            BattleVars.shared.abilityOrigin = idCardList[cardIndex];

            if (SkillManager.Instance.ShouldAskForTarget(BattleVars.shared.abilityOrigin))
            {
                var target = SkillManager.Instance.GetRandomTarget(aiManager, BattleVars.shared.abilityOrigin);
                if (target == null)
                {
                    idCardList = aiManager.playerHand.GetAllValidCardIds();
                    cardIndex = idCardList.FindIndex(x => aiManager.IsAbilityUsable(x.card) && x.card.cardName != cardToCheck.cardName);
                    continue;
                }

                aiManager.ActivateAbility(target);
            }
            else
            {
                aiManager.ActivateAbility(BattleVars.shared.abilityOrigin);
            }
            yield return new WaitForSeconds(BattleVars.shared.aiPlaySpeed);
            idCardList = aiManager.playerHand.GetAllValidCardIds();
            cardIndex = idCardList.FindIndex(x => aiManager.IsAbilityUsable(x.card));
        }
    }
}