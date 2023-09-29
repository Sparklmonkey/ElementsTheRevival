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

            case GameState.PlayShield:
                await _aiTurn.PlayShield(_aiManager);
                SetNewState();
                break;
            
            case GameState.PlayWeapon:
                await _aiTurn.PlayWeapon(_aiManager);
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
                if(_aiManager.GetHandCards().Count > 7) { _aiDiscard.DiscardCard(_aiManager); }
                DuelManager.Instance.EndTurn();
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

public class BasicAiTurnLogic : AiTurnBase
{
    public override async Task PlayPillar(PlayerManager aiManager)
    {
        var pillar = aiManager.playerHand.GetAllValidCardIds().Find(p => p.card.cardType == CardType.Pillar);
        if (pillar is null)
        {
            return;
        }

        aiManager.PlayCardOnField(pillar.card);
    }

    public override Task PlayArtifact(PlayerManager aiManager)
    {
        throw new System.NotImplementedException();
    }

    public override Task PlayCreature(PlayerManager aiManager)
    {
        throw new System.NotImplementedException();
    }

    public override Task PlaySpell(PlayerManager aiManager)
    {
        throw new System.NotImplementedException();
    }

    public override Task ActivateCreature(PlayerManager aiManager)
    {
        throw new System.NotImplementedException();
    }

    public override Task ActivateArtifact(PlayerManager aiManager)
    {
        throw new System.NotImplementedException();
    }

    public override Task PlayShield(PlayerManager aiManager)
    {
        throw new System.NotImplementedException();
    }

    public override Task PlayWeapon(PlayerManager aiManager)
    {
        throw new System.NotImplementedException();
    }

    public override bool HasPillarToPlay()
    {
        throw new System.NotImplementedException();
    }

    public override bool HasCreatureToPlay()
    {
        throw new System.NotImplementedException();
    }

    public override bool HasSpellToPlay()
    {
        throw new System.NotImplementedException();
    }

    public override bool HasWeaponToPlay()
    {
        throw new System.NotImplementedException();
    }

    public override bool HasArtifactToPlay()
    {
        throw new System.NotImplementedException();
    }

    public override bool HasShieldToPlay()
    {
        throw new System.NotImplementedException();
    }

    public override bool HasCreatureAbilityToUse()
    {
        throw new System.NotImplementedException();
    }

    public override bool HasArtifactAbilityToUse()
    {
        throw new System.NotImplementedException();
    }
} 