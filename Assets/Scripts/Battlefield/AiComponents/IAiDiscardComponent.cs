﻿using System.Threading.Tasks;
using UnityEngine;

public interface IAiDiscardComponent
{
    void DiscardCard(PlayerManager aiManager);
}


public class BaseAiDiscardComponent : IAiDiscardComponent
{
    public void DiscardCard(PlayerManager aiManager)
    {
        if (aiManager.playerHand.ShouldDiscard())
        {
            int rndCard = Random.Range(0, 8);
            ID cardToDiscard = new ID(OwnerEnum.Opponent, FieldEnum.Hand, rndCard);
            aiManager.DiscardCard(cardToDiscard);
        }
    }
}