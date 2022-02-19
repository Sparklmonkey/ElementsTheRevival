using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command_DrawCardFromDeck : Command
{
    PlayerManager player;
    Card card;
    ID cardID;
    public Command_DrawCardFromDeck(PlayerManager player, Card card, ID cardID)
    {
        this.player = player;
        this.card = card;
        this.cardID = cardID;
    }
    public override void StartCommandExecution()
    {
        player.DrawCardFromDeckVisual(card, cardID);
    }
}
