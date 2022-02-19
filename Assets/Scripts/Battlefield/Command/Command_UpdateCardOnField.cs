using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command_UpdateCardOnField : Command
{
    private PlayerManager playerManager;
    Card card;
    ID cardID;
    public Command_UpdateCardOnField(PlayerManager playerManager, Card card, ID cardID)
    {
        this.playerManager = playerManager;
        this.card = card;
        this.cardID = cardID;
    }

    public override void StartCommandExecution()
    {
        playerManager.UpdateCardOnFieldVisual(card, cardID);
    }
}
