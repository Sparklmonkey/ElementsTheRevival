using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command_UpdateHealth : Command
{
    private PlayerManager playerManager;
    int newHealth;
    public Command_UpdateHealth(PlayerManager playerManager, int newHealth)
    {
        this.playerManager = playerManager;
        this.newHealth = newHealth;
    }

    public override void StartCommandExecution()
    {
        //playerManager.ModifyHealthVisual(newHealth);
    }
}
