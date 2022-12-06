using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MiracleAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{
    private IEnumerator PlayBlessingsMiracle(PlayerManager aiManager)
    {
        //Get Hand Cards
        List<Card> cardList = new List<Card>(aiManager.GetHandCards());
        List<ID> idList = new List<ID>(aiManager.GetHandIds());

        //Find a Dragon
        List<Card> creatureList = new List<Card>(aiManager.playerCreatureField.GetAllCards());
        List<ID> creatureIds = new List<ID>(aiManager.playerCreatureField.GetAllIds());
        int dragonIndex = creatureList.FindIndex(x => x.cardName == "Light Dragon");
        if (dragonIndex == -1)
        {
            dragonIndex = creatureList.FindIndex(x => x.cardName == "Jade Dragon");
        }
        if (dragonIndex == -1)
        {
            dragonIndex = creatureList.FindIndex(x => x.cardName == "Leaf Dragon");
        }
        if (dragonIndex == -1)
        {
            dragonIndex = creatureList.FindIndex(x => x.cardName == "Elite Pegasus");
        }
        if (dragonIndex == -1)
        {
            dragonIndex = creatureList.FindIndex(x => x.cardName == "Elite Queen");
        }
        if (dragonIndex == -1)
        {
            dragonIndex = creatureList.FindIndex(x => x.cardName == "Elite Firefly");
        }
        if (dragonIndex == -1) { yield break; }

        int cardIndex = -1;
        for (int i = 0; i < cardList.Count; i++)
        {
            if (cardList[i].cardName == "Improved Blessing")
            {
                cardIndex = i;
                break;
            }
        }

        if (cardIndex == -1) { yield break; }
        int loopbreak = 0;
        while (cardIndex != -1 && loopbreak < 7)
        {
            loopbreak++;
            if (aiManager.playerQuantaManager.HasEnoughQuanta(cardList[cardIndex].costElement, cardList[cardIndex].cost))
            {
                BattleVars.shared.originId = idList[cardIndex];
                BattleVars.shared.cardOnStandBy = cardList[cardIndex];
                ID target = creatureIds[dragonIndex];

                yield return aiManager.StartCoroutine(aiManager.ActivateAbility(target));

                cardList = new List<Card>(aiManager.GetHandCards());
                idList = new List<ID>(aiManager.GetHandIds());
                cardIndex = -1;
                for (int i = 0; i < cardList.Count; i++)
                {
                    if (cardList[i].cardName == "Improved Blessing")
                    {
                        cardIndex = i;
                        yield break;
                    }
                }
            }
            else
            {
                yield break;
            }
        }
    }

    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {

        //Play Weapon
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Morning Glory"));
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Jade Staff"));
        //Play Shield
        yield return aiManager.StartCoroutine(PlayShield(aiManager, "Jade Shield"));
        yield return aiManager.StartCoroutine(PlayShield(aiManager, "Solar Buckler"));
        //Play Creatures
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Light Dragon"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Jade Dragon"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Leaf Dragon"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Pegasus"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Queen"));

        //Activate Blessings
        yield return aiManager.StartCoroutine(PlayBlessingsMiracle(aiManager));

        //Activate Abilities
        yield return aiManager.StartCoroutine(ActivateRepeatAbilityNoTarget(aiManager, CardType.Creature, "Pegasus", "Elite Pegasus"));
        yield return aiManager.StartCoroutine(ActivateRepeatAbilityNoTarget(aiManager, CardType.Creature, "Firefly Queen", "Elite Queen"));

        yield return aiManager.StartCoroutine(spellManager.PlayMiracle(aiManager));
    }
}
