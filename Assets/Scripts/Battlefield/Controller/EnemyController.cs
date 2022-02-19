using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController
{
    private PlayerManager self;

    private IAiDrawComponent aiDraw;
    private IAiDiscardComponent aiDiscard;
    private IAiTurnComponent aiTurn;

    public EnemyController(PlayerManager enemyManager)
    {
        self = enemyManager;
        aiDraw = BattleVars.shared.enemyAiData.drawComponent.GetScriptFromName<IAiDrawComponent>();
        aiDiscard = BattleVars.shared.enemyAiData.discardComponent.GetScriptFromName<IAiDiscardComponent>();
        aiTurn = BattleVars.shared.enemyAiData.turnComponent.GetScriptFromName<IAiTurnComponent>();
    }
    //private bool shouldUpdateHand = false;

    public async void StartTurn()
    {
        Debug.Log("Draw Card for turn");
        aiDraw.StartTurnDrawCard(self);
        await new WaitUntil(() => !Command.playingQueue);

        if (self.playerCounters.silence > 0) { DuelManager.EndTurn(); return; }
        Debug.Log("Play Pillars");
        aiTurn.PlayPillars(self);
        await new WaitUntil(() => !Command.playingQueue);
        Debug.Log("Play Rest of turn");
        aiTurn.RestOfTurn(self);
        await new WaitUntil(() => !Command.playingQueue);
        Debug.Log("Discard if needed");
        aiDiscard.DiscardCard(self);
        await new WaitUntil(() => !Command.playingQueue);
        Debug.Log("Update Hand");
        self.UpdateHandLogic();
        await new WaitUntil(() => !Command.playingQueue);

        Debug.Log("End Turn");
        DuelManager.EndTurn();
    }

}
