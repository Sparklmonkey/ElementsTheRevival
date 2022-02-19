using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command_RemoveCardFromField : Command
{
    private PlayerManager playerManager;
    ID cardID;
    public Command_RemoveCardFromField(PlayerManager playerManager, ID cardID)
    {
        this.playerManager = playerManager;
        this.cardID = cardID;
    }

    public override void StartCommandExecution()
    {
        playerManager.RemoveCardFromFieldVisual(cardID);
    }
}
