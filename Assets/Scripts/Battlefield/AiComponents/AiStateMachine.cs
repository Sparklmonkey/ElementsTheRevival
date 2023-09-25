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
    private PlayerManager _aiManager;
    private readonly IAiDrawComponent _aiDraw;
    private readonly IAiDiscardComponent _aiDiscard;
    private readonly AiTurnBase _aiTurn;

    public AiStateMachine(PlayerManager aiManager)
    {
        _aiManager = aiManager;
        _currentState = GameState.Idle;
        _aiDraw = BattleVars.Shared.EnemyAiData.drawComponent.GetScriptFromName<IAiDrawComponent>();
        _aiDiscard = BattleVars.Shared.EnemyAiData.discardComponent.GetScriptFromName<IAiDiscardComponent>();
        _aiTurn = BattleVars.Shared.EnemyAiData.turnComponent.GetScriptFromName<AiTurnBase>();
    }

    public async void Update()
    {
        switch (_currentState)
        {
            case GameState.Idle:
                if (!BattleVars.Shared.IsPlayerTurn)
                {
                    _currentState = GameState.DrawCard;
                }
                break;
            case GameState.PlayPillars:
                await _aiTurn.PlayPillar(_aiManager);
                SetNewState();
                break;

            case GameState.PlayCreatures:
                await _aiTurn.PlayCreature(_aiManager);
                SetNewState();
                break;

            case GameState.PlayArtifacts:
                await  _aiTurn.PlayArtifact(_aiManager);
                SetNewState();
                break;

            case GameState.PlaySpells:
                await _aiTurn.PlaySpell(_aiManager);
                SetNewState();
                break;

            case GameState.ActivateCreatureAbilities:
                await _aiTurn.ActivateCreature(_aiManager);
                SetNewState();
                break;
            case GameState.ActivateArtifactAbilities:
                await _aiTurn.ActivateArtifact(_aiManager);
                SetNewState();
                break;

            case GameState.DrawCard:
                _aiDraw.StartTurnDrawCard(_aiManager);
                SetNewState();
                break;

            case GameState.EndTurn:
                _aiManager.StartCoroutine(DuelManager.Instance.EndTurn());
                _currentState = GameState.Idle;
                break;
        }
    }

    private void SetNewState()
    {
        if (_aiManager.playerHand.GetAllValidCardIds().FindIndex(x => x.card.cardType == CardType.Pillar) >= 0)
        {
            _currentState = GameState.PlayPillars;
        }
        else if (_aiManager.playerHand.GetAllValidCardIds().FindIndex(x => x.card.cardType == CardType.Creature && _aiManager.IsCardPlayable(x.card)) >= 0)
        {
            _currentState = GameState.PlayCreatures;
        }
        else if (_aiManager.playerHand.GetAllValidCardIds().FindIndex(x => x.card.cardType == CardType.Artifact && _aiManager.IsCardPlayable(x.card)) >= 0)
        {
            _currentState = GameState.PlayArtifacts;
        }
        else if (_aiManager.playerHand.GetAllValidCardIds().FindIndex(x => x.card.cardType == CardType.Shield && _aiManager.IsCardPlayable(x.card)) >= 0)
        {
            _currentState = GameState.PlayShield;
        }
        else if (_aiManager.playerHand.GetAllValidCardIds().FindIndex(x => x.card.cardType == CardType.Weapon && _aiManager.IsCardPlayable(x.card)) >= 0)
        {
            _currentState = GameState.PlayWeapon;
        }
        else if (_aiManager.playerHand.GetAllValidCardIds().FindIndex(x => x.card.cardType == CardType.Spell && _aiManager.IsAbilityUsable(x)) >= 0)
        {
            _currentState = GameState.PlaySpells;
        }
        else if (_aiManager.playerCreatureField.GetAllValidCardIds().FindIndex(x => _aiManager.IsAbilityUsable(x)) >= 0)
        {
            _currentState = GameState.ActivateCreatureAbilities;
        }
        else if (_aiManager.playerPermanentManager.GetAllValidCardIds().FindIndex(x => _aiManager.IsAbilityUsable(x)) >= 0)
        {
            _currentState = GameState.ActivateArtifactAbilities;
        }
        else
        {
            _currentState = GameState.EndTurn;
        }
    }
}