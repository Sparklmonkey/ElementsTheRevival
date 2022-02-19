using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class BaseAiTurnComponent : IAiTurnComponent
{
    public void PlayPillars(PlayerManager aiManager)
    {
        //Pillars
        List<Card> cardList = aiManager.GetHandCards();
        List<ID> idList = aiManager.GetHandIds();
        for (int i = 0; i < cardList.Count; i++)
        {
            if (cardList[i].type.Equals(CardType.Pillar))
            {
                aiManager.PlayCardFromHandLogic(idList[i]);
            }
        }
    }

    public void RestOfTurn(PlayerManager aiManager)
    {
        List<Card> cardList = aiManager.GetHandCards();
        List<ID> idList = aiManager.GetHandIds();
        for (int i = 0; i < cardList.Count; i++)
        {
            if (cardList[i].type.Equals(CardType.Creature) && aiManager.IsCardPlayable(cardList[i]))
            {
                aiManager.PlayCardFromHandLogic(idList[i]);
            }
        }


        //Weapon / Shield
        cardList = aiManager.GetHandCards();
        idList = aiManager.GetHandIds();
        for (int i = 0; i < cardList.Count; i++)
        {
            if (cardList[i].type.Equals(CardType.Shield) && aiManager.IsCardPlayable(cardList[i]))
            {
                aiManager.PlayCardFromHandLogic(idList[i]);
            }

            if (cardList[i].type.Equals(CardType.Weapon) && aiManager.IsCardPlayable(cardList[i]))
            {
                aiManager.PlayCardFromHandLogic(idList[i]);
            }
        }

        //Artifacts
        cardList = aiManager.GetHandCards();
        idList = aiManager.GetHandIds();
        for (int i = 0; i < cardList.Count; i++)
        {
            if (cardList[i].type.Equals(CardType.Artifact) && aiManager.IsCardPlayable(cardList[i]))
            {
                aiManager.PlayCardFromHandLogic(idList[i]);
            }
        }
    }
}