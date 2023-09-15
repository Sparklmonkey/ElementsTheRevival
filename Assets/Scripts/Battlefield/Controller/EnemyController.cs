using System.Collections;

public class EnemyController
{
    private readonly PlayerManager _self;

    private readonly IAiDrawComponent _aiDraw;
    private readonly IAiDiscardComponent _aiDiscard;
    private readonly IAiTurnComponent _aiTurn;

    public EnemyController(PlayerManager enemyManager)
    {
        _self = enemyManager;
        _aiDraw = BattleVars.Shared.EnemyAiData.drawComponent.GetScriptFromName<IAiDrawComponent>();
        _aiDiscard = BattleVars.Shared.EnemyAiData.discardComponent.GetScriptFromName<IAiDiscardComponent>();
        _aiTurn = BattleVars.Shared.EnemyAiData.turnComponent.GetScriptFromName<IAiTurnComponent>();
    }

    public IEnumerator StartTurn()
    {
        _self.TurnDownTick();

        _aiDraw.StartTurnDrawCard(_self);

        if (_self.playerCounters.silence > 0) { _self.StartCoroutine(DuelManager.Instance.EndTurn()); yield break; }

        yield return _self.StartCoroutine(_aiTurn.PlayPillars(_self));

        yield return _self.StartCoroutine(_aiTurn.RestOfTurn(_self));

        _aiDiscard.DiscardCard(_self);

        _self.StartCoroutine(DuelManager.Instance.EndTurn());
    }

}
