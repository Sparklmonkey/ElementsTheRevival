using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command_UpdateHand : Command
{
    private PlayerManager playerManager;

    public Command_UpdateHand(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
    }

    public override void StartCommandExecution()
    {
        playerManager.UpdateHandVisual();
    }
}
