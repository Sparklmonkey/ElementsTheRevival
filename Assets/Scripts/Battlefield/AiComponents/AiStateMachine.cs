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

public delegate Task DisplayCardPlayed(CardData card); 

public class AiStateMachine 
{
    private GameState _currentState;
    private readonly PlayerManager _aiManager;
    private readonly IAiDrawComponent _aiDraw;
    private readonly IAiDiscardComponent _aiDiscard;
    private readonly AiTurnBase _aiTurn;
    private readonly GameOverVisual _gameOverVisual;
    private int _stateCount;

    public DisplayCardPlayed DisplayCardPlayed;
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

        CardData card = null;
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
                        EventBus<ResetAiTurnCountEvent>.Raise(new ResetAiTurnCountEvent());
                        _currentState = GameState.DrawCard;
                    }
                }
                break;
            case GameState.PlayPillars:
                card = _aiTurn.PlayCardFromHand(_aiManager, CardType.Pillar);
                SetNewState();
                break;

            case GameState.PlayCreatures:
                card = _aiTurn.PlayCardFromHand(_aiManager, CardType.Creature);
                SetNewState();
                break;

            case GameState.PlayArtifacts:
                card = _aiTurn.PlayCardFromHand(_aiManager, CardType.Artifact);
                SetNewState();
                break;

            case GameState.PlaySpells:
                card = _aiTurn.PlaySpellFromHand(_aiManager);
                SetNewState();
                break;

            case GameState.PlayShield:
                card = _aiTurn.PlayCardFromHand(_aiManager, CardType.Shield);
                SetNewState();
                break;
            
            case GameState.PlayWeapon:
                card = _aiTurn.PlayCardFromHand(_aiManager, CardType.Weapon);
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
                
                yield return _aiManager.StartCoroutine(_aiManager.EndTurnRoutine());
                _aiManager.UpdateCounterAndEffects();
                DuelManager.Instance.EndTurn();
                _currentState = GameState.Idle;
                break;
        }

        if (card is not null)
        {
            var task = DisplayCardPlayed.Invoke(card);
            yield return new WaitUntil(() => task.IsCompleted);
        }
        yield return new WaitForSeconds(0.5f);
        owner.StartCoroutine(Update(owner));
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

public class CardData
{
    public string ImageId;
    public string Element;
    public string CardName;
}

public class BasicAiTurnLogic : AiTurnBase
{
    public override CardData PlayCardFromHand(PlayerManager aiManager, CardType cardType)
    {
        var idCard = aiManager.playerHand.GetAllValidCardIds().Find(p => p.card.cardType == cardType && aiManager.IsCardPlayable(p.card));
        if (idCard == null) return null;
        if (aiManager.IsCardPlayable(idCard.card))
        {
            var cardData = new CardData()
            {
                ImageId = idCard.card.imageID,
                Element = idCard.card.costElement.FastElementString(),
                CardName = idCard.card.cardName,
            };
            aiManager.PlayCardFromHandLogic(idCard);
            return cardData;
        }

        return null;
    }

    public override CardData PlaySpellFromHand(PlayerManager aiManager)
    {
        var spell = aiManager.playerHand.GetAllValidCardIds().Find(s => s.card.cardType == CardType.Spell && aiManager.IsCardPlayable(s.card));
        if (spell == null)
        {
            return null;
        }

        if (SkillManager.Instance.ShouldAskForTarget(spell))
        {
            var target = SkillManager.Instance.GetRandomTarget(aiManager, spell);
            if (target == null)
            {
                return null;
            }

            var cardData = new CardData()
            {
                ImageId = spell.card.imageID,
                Element = spell.card.costElement.FastElementString(),
                CardName = spell.card.cardName,
            };
            BattleVars.Shared.AbilityOrigin = spell;
            SkillManager.Instance.SkillRoutineWithTarget(aiManager, target);
            aiManager.PlayCardFromHandLogic(spell);
            
            return cardData;
        }
        else
        {
            var cardData = new CardData()
            {
                ImageId = spell.card.imageID,
                Element = spell.card.costElement.FastElementString(),
                CardName = spell.card.cardName,
            };
            SkillManager.Instance.SkillRoutineNoTarget(aiManager, spell);
            aiManager.PlayCardFromHandLogic(spell);
            return cardData;
        }

    }

    public override void ActivateCreatureAbility(PlayerManager aiManager)
    {
        var creature = aiManager.playerCreatureField.GetAllValidCardIds().Find(c => c.card.skill is not null or "" or " " && aiManager.IsAbilityUsable(c));
        if (creature == null)
        {
            return;
        }

        if (SkillManager.Instance.ShouldAskForTarget(creature))
        {
            var target = SkillManager.Instance.GetRandomTarget(aiManager, creature);
            if (target == null)
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
        if (artifact == null)
        {
            return;
        }

        if (SkillManager.Instance.ShouldAskForTarget(artifact))
        {
            var target = SkillManager.Instance.GetRandomTarget(aiManager, artifact);
            if (target == null)
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
        if (creatureWithSkill == null)
        {
            return false;
        }
        return aiManager.IsAbilityUsable(creatureWithSkill);
    }

    public override bool HasArtifactAbilityToUse(PlayerManager aiManager)
    {
        var artifactWithSkill = aiManager.playerPermanentManager.GetAllValidCardIds().Find(x => x.card.skill is not null or "" or " ");
        if (artifactWithSkill == null)
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