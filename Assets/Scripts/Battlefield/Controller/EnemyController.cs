using System;
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private AiStateMachine _aiStateMachine;
    private PlayerManager _self;
    private bool _isSetup = false;
    private bool _hasStarted = false;
    
    // private readonly IAiDrawComponent _aiDraw;
    // private readonly IAiDiscardComponent _aiDiscard;
    // private readonly IAiTurnComponent _aiTurn;

    public void SetupController(PlayerManager enemyManager, GameOverVisual gameOverVisual)
    {
        _aiStateMachine = new AiStateMachine(enemyManager, gameOverVisual);
        _self = enemyManager;
        _isSetup = true;
        // _aiDraw = BattleVars.Shared.EnemyAiData.drawComponent.GetScriptFromName<IAiDrawComponent>();
        // _aiDiscard = BattleVars.Shared.EnemyAiData.discardComponent.GetScriptFromName<IAiDiscardComponent>();
        // _aiTurn = BattleVars.Shared.EnemyAiData.turnComponent.GetScriptFromName<IAiTurnComponent>();
    }

    private void Update()
    {
        if (!_isSetup)
        {
            return;
        }

        if (_hasStarted)
        {
            return;
        }
        StartCoroutine(_aiStateMachine.Update(this));
        _hasStarted = true;
    }
    //
    // public IEnumerator StartTurn()
    // {
    //     _self.TurnDownTick();
    //
    //     _aiDraw.StartTurnDrawCard(_self);
    //
    //     if (_self.playerCounters.silence > 0) { DuelManager.Instance.EndTurn(); yield break; }
    //
    //     yield return _self.StartCoroutine(_aiTurn.PlayPillars(_self));
    //
    //     yield return _self.StartCoroutine(_aiTurn.RestOfTurn(_self));
    //
    //     _aiDiscard.DiscardCard(_self);
    //
    //     DuelManager.Instance.EndTurn();
    // }

}
