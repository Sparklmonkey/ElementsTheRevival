using System.Collections;
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
    private readonly GameOverVisual _gameOverVisual;
    private int _stateCount;
    public AiStateMachine(PlayerManager aiManager, GameOverVisual gameOverVisual)
    {
        _gameOverVisual = gameOverVisual;
        _aiManager = aiManager;
        _currentState = GameState.Idle;
        _stateCount = 0;
        _aiDraw = BattleVars.Shared.EnemyAiData.drawComponent.GetScriptFromName<IAiDrawComponent>();
        _aiDiscard = BattleVars.Shared.EnemyAiData.discardComponent.GetScriptFromName<IAiDiscardComponent>();
        _aiTurn = new BasicAiTurnLogic();
    }

    public IEnumerator Update(MonoBehaviour controller)
    {
        if (_gameOverVisual.IsGameOver)
        {
            yield break;
        }

        _stateCount++;
        if (_stateCount > 20 && _currentState != GameState.Idle)
        {
            _currentState = GameState.EndTurn;
        }
        switch (_currentState)
        {
            case GameState.Idle:
                if (!BattleVars.Shared.IsPlayerTurn)
                {
                    _aiManager.TurnDownTick();

                    if (_aiManager.playerCounters.silence > 0)
                    {
                        _currentState = GameState.EndTurn;
                    }
                    else
                    {
                        _stateCount = 0;
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
                _aiTurn.PlaySpellFromHand(_aiManager);
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
                _aiTurn.ActivateCreatureAbility(_aiManager);
                SetNewState();
                break;
            case GameState.ActivateArtifactAbilities:
                _aiTurn.ActivateArtifactAbility(_aiManager);
                SetNewState();
                break;

            case GameState.DrawCard:
                _aiDraw.StartTurnDrawCard(_aiManager);
                SetNewState();
                break;

            case GameState.EndTurn:
                if(_aiManager.GetHandCards().Count > 7) { _aiDiscard.DiscardCard(_aiManager); }
                _aiManager.EndTurnRoutine();
                _aiManager.UpdateCounterAndEffects();
                DuelManager.Instance.EndTurn();
                _currentState = GameState.Idle;
                break;
        }

        yield return new WaitForSeconds(0.5f);
        controller.StartCoroutine(Update(controller));
    }

    private void SetNewState()
    {
        if (_aiTurn.HasCardInHand(_aiManager, CardType.Pillar))
        {
            _currentState = GameState.PlayPillars;
        }
        else if (_aiTurn.HasCardInHand(_aiManager, CardType.Creature))
        {
            _currentState = GameState.PlayCreatures;
        }
        else if (_aiTurn.HasCardInHand(_aiManager, CardType.Artifact))
        {
            _currentState = GameState.PlayArtifacts;
        }
        else if (_aiTurn.HasCardInHand(_aiManager, CardType.Shield))
        {
            _currentState = GameState.PlayShield;
        }
        else if (_aiTurn.HasCardInHand(_aiManager, CardType.Weapon))
        {
            _currentState = GameState.PlayWeapon;
        }
        else if (_aiTurn.HasSpellToUse(_aiManager))
        {
            _currentState = GameState.PlaySpells;
        }
        else if (_aiTurn.HasCreatureAbilityToUse(_aiManager))
        {
            _currentState = GameState.ActivateCreatureAbilities;
        }
        else if (_aiTurn.HasArtifactAbilityToUse(_aiManager))
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
    public override void PlayCardFromHand(PlayerManager aiManager, CardType cardType)
    {
        var idCard = aiManager.playerHand.GetAllValidCardIds().Find(p => p.card.cardType == cardType && aiManager.IsCardPlayable(p.card));
        if (idCard is not null)
        {
            if (aiManager.IsCardPlayable(idCard.card))
            {
                aiManager.PlayCardFromHandLogic(idCard);
            }
        }
    }

    public override void PlaySpellFromHand(PlayerManager aiManager)
    {
        var spell = aiManager.playerHand.GetAllValidCardIds().Find(s => s.card.cardType == CardType.Spell && aiManager.IsCardPlayable(s.card));
        if (spell is null)
        {
            return;
        }

        if (SkillManager.Instance.ShouldAskForTarget(spell))
        {
            var target = SkillManager.Instance.GetRandomTarget(aiManager, spell);
            if (target is null)
            {
                return;
            }

            BattleVars.Shared.AbilityOrigin = spell;
            SkillManager.Instance.SkillRoutineWithTarget(aiManager, target);
            aiManager.PlayCardFromHandLogic(spell);
            return;
        }

        SkillManager.Instance.SkillRoutineNoTarget(aiManager, spell);
        aiManager.PlayCardFromHandLogic(spell);
    }

    public override void ActivateCreatureAbility(PlayerManager aiManager)
    {
        var creature = aiManager.playerCreatureField.GetAllValidCardIds().Find(c => c.card.skill is not null or "" or " " && aiManager.IsAbilityUsable(c));
        if (creature is null)
        {
            return;
        }

        if (SkillManager.Instance.ShouldAskForTarget(creature))
        {
            var target = SkillManager.Instance.GetRandomTarget(aiManager, creature);
            if (target is null)
            {
                return;
            }

            BattleVars.Shared.AbilityOrigin = creature;
            creature.card.AbilityUsed = true;
            SkillManager.Instance.SkillRoutineWithTarget(aiManager, target);
            return;
        }
        
        creature.card.AbilityUsed = true;
        SkillManager.Instance.SkillRoutineNoTarget(aiManager, creature);
    }

    public override void ActivateArtifactAbility(PlayerManager aiManager)
    {
        var artifact = aiManager.playerPermanentManager.GetAllValidCardIds().Find(a => a.card.skill is not null or "" or " " && aiManager.IsAbilityUsable(a));
        if (artifact is null)
        {
            return;
        }

        if (SkillManager.Instance.ShouldAskForTarget(artifact))
        {
            var target = SkillManager.Instance.GetRandomTarget(aiManager, artifact);
            if (target is null)
            {
                return;
            }

            BattleVars.Shared.AbilityOrigin = artifact;
            artifact.card.AbilityUsed = true;
            SkillManager.Instance.SkillRoutineWithTarget(aiManager, target);
            return;
        }
        
        artifact.card.AbilityUsed = true;
        SkillManager.Instance.SkillRoutineNoTarget(aiManager, artifact);
    }
    
    public override bool HasCardInHand(PlayerManager aiManager, CardType cardToCheck)
    {
        return aiManager.playerHand.GetAllValidCardIds().FindIndex(x => x.card.cardType == cardToCheck && aiManager.IsCardPlayable(x.card)) >= 0;
    }

    public override bool HasCreatureAbilityToUse(PlayerManager aiManager)
    {
        var creatureWithSkill = aiManager.playerCreatureField.GetAllValidCardIds().Find(x => x.card.skill is not null or "" or " ");
        if (creatureWithSkill is null)
        {
            return false;
        }
        return aiManager.IsAbilityUsable(creatureWithSkill);
    }

    public override bool HasArtifactAbilityToUse(PlayerManager aiManager)
    {
        var artifactWithSkill = aiManager.playerPermanentManager.GetAllValidCardIds().Find(x => x.card.skill is not null or "" or " ");
        if (artifactWithSkill is null)
        {
            return false;
        }
        return aiManager.IsAbilityUsable(artifactWithSkill);
    }

    public override bool HasSpellToUse(PlayerManager aiManager)
    {
        return aiManager.playerHand.GetAllValidCardIds()
            .FindIndex(x => x.card.cardType == CardType.Spell && aiManager.IsCardPlayable(x.card)) >= 0;
    }
} 