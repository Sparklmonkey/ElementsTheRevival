using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireQueenAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{

    private IEnumerator ActivateEliteQueen(PlayerManager aiManager)
    {
        List<IDCardPair> creaturelist = new (aiManager.playerCreatureField.GetAllValidCardIds());
        List<IDCardPair> queenCards = new();

        foreach (var creature in creaturelist)
        {
            switch (creature.card.cardName)
            {
                case "Elite Queen":
                    queenCards.Add(creature);
                    break;
                default:
                    break;
            }
        }

        if (queenCards.Count == 0) { yield break; }

        foreach (var queen in queenCards)
        {
            if (!aiManager.IsAbilityUsable(queen.card)) { continue; }
            BattleVars.shared.abilityOrigin = queen;
            aiManager.ActivateAbility(queen);
        }
    }


    private IEnumerator ActivateWeapons(PlayerManager aiManager)
    {
        Card weapon = aiManager.playerPassiveManager.GetWeapon();
        if(weapon == null) { yield break; }
        if (weapon.cardName == "Weapon") { yield break; }
        SkillManager.Instance.SetupTargetHighlights(aiManager, DuelManager.Instance.player, weapon);
        List<ID> idList = DuelManager.GetAllValidTargets();
        if(idList.Count == 0) { yield break; }
        if (weapon != null)
        {
            if(weapon.cardName == "Eagle's Eye")
            {

                if (aiManager.IsAbilityUsable(weapon))
                {
                    BattleVars.shared.abilityOrigin = aiManager.playerPassiveManager.GetWeaponID();
                    BattleVars.shared.cardOnStandBy = weapon;

                    System.Random rnd = new System.Random();
                    ID target = idList.OrderBy(x => rnd.Next())
                                      .First();
                    yield return aiManager.StartCoroutine(aiManager.ActivateAbility(target));
                }
            }
        }

        List<Card> creatureCards = aiManager.playerCreatureField.GetAllCards();
        List<ID> creatureIds = aiManager.playerCreatureField.GetAllIds();
        List<Card> eagleEyeCards = new List<Card>();
        List<ID> eagleEyeIds = new List<ID>();

        for (int i = 0; i < creatureCards.Count; i++)
        {
            switch (creatureCards[i].cardName)
            {
                case "Eagle's Eye":
                    eagleEyeCards.Add(creatureCards[i]);
                    eagleEyeIds.Add(creatureIds[i]);
                    break;
                default:
                    break;
            }
        }

        if (eagleEyeCards.Count == 0) { yield break; }

        for (int i = 0; i < eagleEyeCards.Count; i++)
        {
            if (!aiManager.IsAbilityUsable(eagleEyeCards[i])) { continue; }
            BattleVars.shared.originId = eagleEyeIds[i];
            BattleVars.shared.cardOnStandBy = eagleEyeCards[i];
            SkillManager.Instance.SetupTargetHighlights(aiManager, DuelManager.Instance.player, BattleVars.shared.cardOnStandBy);
            idList = DuelManager.GetAllValidTargets();
            if (idList.Count == 0) { yield break; }

            System.Random rnd = new System.Random();
            ID target = idList.OrderBy(x => rnd.Next())
                              .First();
            yield return aiManager.StartCoroutine(aiManager.ActivateAbility(target));
        }

    }

    private IEnumerator PlayAnimateWeapon(PlayerManager aiManager)
    {
        if (aiManager.playerPassiveManager.GetWeapon() == null) { yield break; }
        if (aiManager.playerPassiveManager.GetWeapon().cardName == "Weapon") { yield break; }
        //Get Hand Cards
        List<Card> cardList = new List<Card>(aiManager.GetHandCards());
        List<ID> idList = new List<ID>(aiManager.GetHandIds());


        int cardIndex = -1;
        for (int i = 0; i < cardList.Count; i++)
        {
            if (cardList[i].cardName == "Animate Weapon")
            {
                cardIndex = i;
                break;
            }
        }

        if (cardIndex == -1) { yield break; }
        if (aiManager.playerQuantaManager.HasEnoughQuanta(cardList[cardIndex].costElement, cardList[cardIndex].cost))
        {
            BattleVars.shared.originId = idList[cardIndex];
            BattleVars.shared.cardOnStandBy = cardList[cardIndex];
            ID target = aiManager.playerPassiveManager.GetWeaponID();

            yield return aiManager.StartCoroutine(aiManager.ActivateAbility(target));
        }
    }

    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {
        //Play Weapon if none in play
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Eagle's Eye"));
        yield return aiManager.StartCoroutine(PlayAnimateWeapon(aiManager));
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Elite Fahrenheit"));
        yield return aiManager.StartCoroutine(PlayAnimateWeapon(aiManager));
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Eagle's Eye"));
        yield return aiManager.StartCoroutine(PlayAnimateWeapon(aiManager));
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Elite Fahrenheit"));
        yield return aiManager.StartCoroutine(PlayAnimateWeapon(aiManager));

        //Play Creatures
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Queen"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Feral Bond"));

        //Activate Elite Queen
        yield return aiManager.StartCoroutine(ActivateEliteQueen(aiManager));
        //Activate Weapons
        yield return aiManager.StartCoroutine(ActivateWeapons(aiManager));
    }
}
