using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command_PlayCardFromHand : Command
{
    PlayerManager player;
    ID cardId;
    public Command_PlayCardFromHand(PlayerManager player, ID cardId)
    {
        this.player = player;
        this.cardId = cardId;
    }
    public override void StartCommandExecution()
    {

    }
}
