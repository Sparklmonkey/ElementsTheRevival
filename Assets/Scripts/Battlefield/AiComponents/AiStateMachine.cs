using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public enum GameState
{
    PlayPillars,
    PlayCreatures,
    PlayArtifacts,
    PlayShield,
    PlayWeapon,
    PlaySpells,
    ActivateCreatureAbilities,
    ActivateArtifactAbilities,
    DrawCard,
    EndTurn,
    Idle
}

public class AiStateMachine 
{
    private GameState _currentState;
    private readonly PlayerManager _aiManager;
    private readonly IAiDrawComponent _aiDraw;
    private readonly IAiDiscardComponent _aiDiscard;
    private readonly AiTurnBase _aiTurn;
    private readonly GameOverVisual _gameOverVisual;
    private int _stateCount;

    public AiStateMachine(PlayerManager aiManager, GameOverVisual gameOverVisual)
    {
        _gameOverVisual = gameOverVisual;
        _aiManager = aiManager;
        _currentState = GameState.Idle;
        _stateCount = 0;
        EventBus<ResetAiTurnCountEvent>.Raise(new ResetAiTurnCountEvent());
        _aiDraw = BattleVars.Shared.EnemyAiData.drawComponent.GetScriptFromName<IAiDrawComponent>();
        _aiDiscard = BattleVars.Shared.EnemyAiData.discardComponent.GetScriptFromName<IAiDiscardComponent>();
        _aiTurn = new BasicAiTurnLogic();
    }

    public IEnumerator Update(MonoBehaviour owner)
    {
        if (_gameOverVisual.isGameOver)
        {
            yield break;
        }
        if (_stateCount >= 20 && _currentState != GameState.Idle)
        {
            _currentState = GameState.EndTurn;
        }

        if (_currentState != GameState.Idle)
        {
            _stateCount++;
            EventBus<AddAiTurnCountEvent>.Raise(new AddAiTurnCountEvent());
        }

        switch (_currentState)
        {
            case GameState.Idle:
                if (!BattleVars.Shared.IsPlayerTurn)
                {
                    if (_aiManager.playerCounters.silence > 0)
                    {
                        _currentState = GameState.EndTurn;
                    }
                    else
                    {
                        _stateCount = 0;
                        EventBus<ResetAiTurnCountEvent>.Raise(new ResetAiTurnCountEvent());
                        _currentState = GameState.DrawCard;
                    }
                }
                break;
            case GameState.PlayPillars:
                _aiTurn.PlayCardFromHand(_aiManager, CardType.Pillar);
                SetNewState();
                break;

            case GameState.PlayCreatures:
                _aiTurn.PlayCardFromHand(_aiManager, CardType.Creature);
                SetNewState();
                break;

            case GameState.PlayArtifacts:
                _aiTurn.PlayCardFromHand(_aiManager, CardType.Artifact);
                SetNewState();
                break;

            case GameState.PlaySpells:
                yield return _aiManager.StartCoroutine(_aiTurn.PlaySpellFromHand(_aiManager));
                SetNewState();
                break;

            case GameState.PlayShield:
                _aiTurn.PlayCardFromHand(_aiManager, CardType.Shield);
                SetNewState();
                break;
            
            case GameState.PlayWeapon:
                _aiTurn.PlayCardFromHand(_aiManager, CardType.Weapon);
                SetNewState();
                break;
            
            case GameState.ActivateCreatureAbilities:
                yield return _aiManager.StartCoroutine(_aiTurn.ActivateCreatureAbility(_aiManager));
                SetNewState();
                break;
            case GameState.ActivateArtifactAbilities:
                yield return _aiManager.StartCoroutine(_aiTurn.ActivateArtifactAbility(_aiManager));
                SetNewState();
                break;

            case GameState.DrawCard:
                EventBus<OnTurnStartEvent>.Raise(new OnTurnStartEvent(_aiManager.Owner));
                _aiDraw.StartTurnDrawCard(_aiManager);
                SetNewState();
                break;

            case GameState.EndTurn:
                if(_aiManager.playerHand.ShouldDiscard()) { _aiTurn.DiscardCard(_aiManager); }
                _aiTurn.ResetSkipList();
                yield return _aiManager.StartCoroutine(_aiManager.EndTurnRoutine());
                _aiManager.UpdateCounterAndEffects();
                DuelManager.Instance.EndTurn();
                _currentState = GameState.Idle;
                break;
        }

        yield return new WaitForSeconds(1f);
        owner.StartCoroutine(Update(owner));
    }

    private void SetNewState()
    {
        if (_aiTurn.HasCardInHand(_aiManager, CardType.Pillar))
        {
            _currentState = GameState.PlayPillars;
            return;
        }
        if (_aiTurn.HasCardInHand(_aiManager, CardType.Creature))
        {
            _currentState = GameState.PlayCreatures;
            return;
        }
        if (_aiTurn.HasCardInHand(_aiManager, CardType.Artifact))
        {
            _currentState = GameState.PlayArtifacts;
            return;
        }
        if (_aiTurn.HasCardInHand(_aiManager, CardType.Shield))
        {
            _currentState = GameState.PlayShield;
            return;
        }
        if (_aiTurn.HasCardInHand(_aiManager, CardType.Weapon))
        {
            _currentState = GameState.PlayWeapon;
            return;
        }
        if (_aiTurn.HasCardInHand(_aiManager, CardType.Spell))
        {
            _currentState = GameState.PlaySpells;
            return;
        }
        if (_aiTurn.HasCreatureAbilityToUse(_aiManager))
        {
            _currentState = GameState.ActivateCreatureAbilities;
            return;
        }
        if (_aiTurn.HasArtifactAbilityToUse(_aiManager))
        {
            _currentState = GameState.ActivateArtifactAbilities;
            return;
        }
        _currentState = GameState.EndTurn;
    }
}