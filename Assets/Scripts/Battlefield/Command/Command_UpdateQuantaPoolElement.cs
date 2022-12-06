using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command_UpdateQuantaPoolElement : Command
{
    private PlayerManager playerManager;
    int elementInt;
    int newTotal;
    public Command_UpdateQuantaPoolElement(PlayerManager playerManager, int elementInt, int newTotal)
    {
        this.playerManager = playerManager;
        this.elementInt = elementInt;
        this.newTotal = newTotal;
    }

    public override void StartCommandExecution()
    {
        //playerManager.UpdateQuantaPoolVisual(elementInt, newTotal);
    }
}
