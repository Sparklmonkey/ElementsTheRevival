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
    ActivateWeaponAbilities,
    DrawCard,
    EndTurn,
    Idle
}

public class AiStateMachine 
{
    private GameState _currentState;
    private readonly PlayerManager _aiManager;
    private readonly IAiDrawComponent _aiDraw;
    private readonly AiTurnBase _aiTurn;
    private readonly GameOverVisual _gameOverVisual;
    private int _stateCount;
    private TurnPhase _turnPhase;

    public AiStateMachine(PlayerManager aiManager, GameOverVisual gameOverVisual)
    {
        _gameOverVisual = gameOverVisual;
        _aiManager = aiManager;
        _currentState = GameState.Idle;
        _stateCount = 0;
        _turnPhase = new TurnPhase(true);
        EventBus<ResetAiTurnCountEvent>.Raise(new ResetAiTurnCountEvent());
        _aiDraw = BattleVars.Shared.EnemyAiData.drawComponent.GetScriptFromName<IAiDrawComponent>();
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
            _turnPhase.SetEndTurn();
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
                    _stateCount = 0; 
                    EventBus<ResetAiTurnCountEvent>.Raise(new ResetAiTurnCountEvent()); 
                    _turnPhase.ResetForTurn();
                }
                break;
            case GameState.PlayPillars:
                _aiTurn.PlayCardFromHand(_aiManager, CardType.Pillar);
                _turnPhase.PlayPillarsPhase = true;
                break;

            case GameState.PlayCreatures:
                _aiTurn.PlayCardFromHand(_aiManager, CardType.Creature);
                _turnPhase.PlayCreaturesPhase = true;
                break;

            case GameState.PlayArtifacts:
                _aiTurn.PlayCardFromHand(_aiManager, CardType.Artifact);
                _turnPhase.PlayArtifactsPhase = true;
                break;
            case GameState.PlaySpells:
                yield return _aiManager.StartCoroutine(_aiTurn.PlaySpellFromHand(_aiManager));
                _turnPhase.PlaySpellsPhase = true;
                break;
            case GameState.PlayShield:
                if (_aiManager.playerPassiveManager.GetShield().card.Id == "4t1")
                {
                    _aiTurn.PlayCardFromHand(_aiManager, CardType.Shield);
                }
                _turnPhase.PlayShieldsPhase = true;
                break;
            case GameState.PlayWeapon:
                if (_aiManager.playerPassiveManager.GetShield().card.Id == "4t2")
                {
                    _aiTurn.PlayCardFromHand(_aiManager, CardType.Weapon);
                }
                _turnPhase.PlayWeaponsPhase = true;
                break;
            
            case GameState.ActivateCreatureAbilities:
                yield return _aiManager.StartCoroutine(_aiTurn.ActivateCreatureAbility(_aiManager));
                _turnPhase.ActivateCreaturesPhase = true;
                break;
            case GameState.ActivateArtifactAbilities:
                yield return _aiManager.StartCoroutine(_aiTurn.ActivateArtifactAbility(_aiManager));
                _turnPhase.ActivateArtifactsPhase = true;
                break;
            case GameState.ActivateWeaponAbilities:
                yield return _aiManager.StartCoroutine(_aiTurn.ActivateWeaponAbility(_aiManager));
                _turnPhase.ActivateWeaponPhase = true;
                break;

            case GameState.DrawCard:
                EventBus<OnTurnStartEvent>.Raise(new OnTurnStartEvent(_aiManager.owner));
                _aiDraw.StartTurnDrawCard(_aiManager);
                _turnPhase.DrawPhase = true;
                break;

            case GameState.EndTurn:
                if(_aiManager.playerHand.ShouldDiscard()) { _aiTurn.DiscardCard(_aiManager); }
                yield return _aiManager.StartCoroutine(_aiManager.EndTurnRoutine(DuelManager.Instance.ShouldEndGame));
                DuelManager.Instance.EndTurn();
                _turnPhase.EndTurnPhase = true;
                break;
        }

        yield return new WaitForSeconds(0.5f);
        _currentState = _turnPhase.GetNextState();
        owner.StartCoroutine(Update(owner));
    }
    
    private void SetNewState()
    {
        if (_aiManager.playerCounters.silence <= 0)
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
            if (_aiTurn.HasCardInHand(_aiManager, CardType.Shield) && !_aiManager.playerPassiveManager.GetShield().HasCard())
            {
                _currentState = GameState.PlayShield;
                return;
            }
            if (_aiTurn.HasCardInHand(_aiManager, CardType.Weapon) && !_aiManager.playerPassiveManager.GetWeapon().HasCard())
            {
                _currentState = GameState.PlayWeapon;
                return;
            }
            if (_aiTurn.HasCardInHand(_aiManager, CardType.Spell))
            {
                _currentState = GameState.PlaySpells;
                return;
            }
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


    private void TurnOrder()
    {
        //First Draw Cards
        //Iterate over cards and play pillars
        //Iterate over cards and play Creatures
        //Iterate over cards and play Spells
        //Iterate over cards and play Weapons
        //Iterate over cards and play Shields
        //
    }
}

struct TurnPhase
{
    public bool DrawPhase { get; set; }
    //Play From Hand
    public bool PlayPillarsPhase { get; set; }
    public bool PlayCreaturesPhase { get; set; }
    public bool PlaySpellsPhase { get; set; }
    public bool PlayWeaponsPhase { get; set; }
    public bool PlayShieldsPhase { get; set; }
    public bool PlayArtifactsPhase { get; set; }
    //Activate Abilities
    public bool ActivateCreaturesPhase { get; set; }
    public bool ActivateArtifactsPhase { get; set; }
    public bool ActivateWeaponPhase { get; set; }
    //End Turn
    public bool EndTurnPhase { get; set; }

    public TurnPhase(bool initialValue)
    {
        DrawPhase = initialValue;
        PlayPillarsPhase = initialValue;
        PlayCreaturesPhase = initialValue;
        PlaySpellsPhase = initialValue;
        PlayWeaponsPhase = initialValue;
        PlayShieldsPhase = initialValue;
        PlayArtifactsPhase = initialValue;
        ActivateCreaturesPhase = initialValue;
        ActivateArtifactsPhase = initialValue;
        ActivateWeaponPhase = initialValue;
        EndTurnPhase = initialValue;
    }

    public GameState GetNextState()
    {
        if (!DrawPhase) return GameState.DrawCard;
        if (!PlayPillarsPhase) return GameState.PlayPillars;
        if (!PlayCreaturesPhase) return GameState.PlayCreatures;
        if (!PlayArtifactsPhase) return GameState.PlayArtifacts;
        if (!PlaySpellsPhase) return GameState.PlaySpells;
        if (!PlayWeaponsPhase) return GameState.PlayWeapon;
        if (!PlayShieldsPhase) return GameState.PlayShield;
        if (!ActivateCreaturesPhase) return GameState.ActivateCreatureAbilities;
        if (!ActivateArtifactsPhase) return GameState.ActivateArtifactAbilities;
        if (!ActivateWeaponPhase) return GameState.ActivateWeaponAbilities;
        return !EndTurnPhase ? GameState.EndTurn : GameState.Idle;
    }

    public void ResetForTurn()
    {
        DrawPhase = false;
        PlayPillarsPhase = false;
        PlayCreaturesPhase = false;
        PlaySpellsPhase = false;
        PlayWeaponsPhase = false;
        PlayShieldsPhase = false;
        PlayArtifactsPhase = false;
        ActivateCreaturesPhase = false;
        ActivateArtifactsPhase = false;
        ActivateWeaponPhase = false;
        EndTurnPhase = false;
    }

    public void SetEndTurn()
    {
        DrawPhase = true;
        PlayPillarsPhase = true;
        PlayCreaturesPhase = true;
        PlaySpellsPhase = true;
        PlayWeaponsPhase = true;
        PlayShieldsPhase = true;
        PlayArtifactsPhase = true;
        ActivateCreaturesPhase = true;
        ActivateArtifactsPhase = true;
        ActivateWeaponPhase = true;
        EndTurnPhase = true;
    }
}