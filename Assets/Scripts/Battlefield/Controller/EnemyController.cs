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

    public IEnumerator StartTurn()
    {
        self.TurnDownTick();
        Debug.Log("Draw Card for turn");
        aiDraw.StartTurnDrawCard(self);

        if (self.playerCounters.silence > 0) { self.StartCoroutine(DuelManager.Instance.EndTurn()); yield break; }
        Debug.Log("Play Pillars");
        yield return self.StartCoroutine(aiTurn.PlayPillars(self));
        //yield return new WaitForSeconds(5);
        Debug.Log("Play Rest of turn");
        yield return self.StartCoroutine(aiTurn.RestOfTurn(self));
        Debug.Log("Discard if needed");
        aiDiscard.DiscardCard(self);

        Debug.Log("End Turn");
        self.StartCoroutine(DuelManager.Instance.EndTurn());
    }

}
