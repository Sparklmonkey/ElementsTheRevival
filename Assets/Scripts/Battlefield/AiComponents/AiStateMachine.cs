using System.Collections;
using System.Linq;
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
                if(_aiManager.playerHand.ShouldDiscard()) { _aiDiscard.DiscardCard(_aiManager); }
                
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
    private void PlayCardOnField(PlayerManager aiManager, Card card, ID id)
    {
        EventBus<PlaySoundEffectEvent>.Raise(new PlaySoundEffectEvent("CardPlay"));
        if (aiManager.playerCounters.neurotoxin > 0)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Neurotoxin, OwnerEnum.Opponent, 1));
        }
        
        if (!card.cardType.Equals(CardType.Spell))
        {
            EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(card, false));
            if (card.cardType is CardType.Artifact or CardType.Pillar)
            {
                EventBus<PlayPermanentOnFieldEvent>.Raise(new PlayPermanentOnFieldEvent(OwnerEnum.Opponent, card));
            }
            else
            {
                EventBus<PlayCardOnFieldEvent>.Raise(new PlayCardOnFieldEvent(card, id.owner));
            }
        }
        if (card.cost > 0)
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(card.cost, card.costElement, OwnerEnum.Opponent, false));
        }
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(id));
    }
    public override CardData PlayCardFromHand(PlayerManager aiManager, CardType cardType)
    {
        var card = aiManager.playerHand.GetPlayableCards(aiManager.HasSufficientQuanta).FirstOrDefault(c => c.Item2.cardType.Equals(cardType));
        if (card.Equals(default)) return null;
        var cardData = new CardData()
        {
            ImageId = card.Item2.imageID,
            Element = card.Item2.costElement.FastElementString(),
            CardName = card.Item2.cardName,
        };
        PlayCardOnField(aiManager, card.Item2, card.Item1);
        return cardData;

    }

    public override CardData PlaySpellFromHand(PlayerManager aiManager)
    {
        var cardList = aiManager.playerHand.GetPlayableCards(aiManager.HasSufficientQuanta);
        var spell = cardList.FirstOrDefault(s => s.Item2.cardType == CardType.Spell && aiManager.IsCardPlayable(s.Item2));
        if (spell.Equals(default))
        {
            return null;
        }

        if (SkillManager.Instance.ShouldAskForTarget(spell.Item2))
        {
            var target = SkillManager.Instance.GetRandomTarget(aiManager, spell.Item2);
            if (target.Equals(default)) return null;

            var cardData = new CardData()
            {
                ImageId = spell.Item2.imageID,
                Element = spell.Item2.costElement.FastElementString(),
                CardName = spell.Item2.cardName,
            };
            BattleVars.Shared.AbilityCardOrigin = spell.Item2;
            BattleVars.Shared.AbilityIDOrigin = spell.Item1;
            SkillManager.Instance.SkillRoutineWithTarget(aiManager, target.Item1, target.Item2);
            PlayCardOnField(aiManager, spell.Item2, spell.Item1);
            
            return cardData;
        }
        else
        {
            var cardData = new CardData()
            {
                ImageId = spell.Item2.imageID,
                Element = spell.Item2.costElement.FastElementString(),
                CardName = spell.Item2.cardName,
            };
            SkillManager.Instance.SkillRoutineNoTarget(aiManager, spell.Item1, spell.Item2);
            PlayCardOnField(aiManager, spell.Item2, spell.Item1);
            return cardData;
        }

    }

    public override void ActivateCreatureAbility(PlayerManager aiManager)
    {
        var creature = aiManager.playerCreatureField.GetAllValidCardIds().Find(c => c.Item2.skill is not null or "" or " " && aiManager.IsAbilityUsable(c.Item2));
        if (creature.Equals(default)) return;

        if (SkillManager.Instance.ShouldAskForTarget(creature.Item2))
        {
            var target = SkillManager.Instance.GetRandomTarget(aiManager, creature.Item2);
            if (target.Equals(default)) return;

            BattleVars.Shared.AbilityIDOrigin = creature.Item1;
            BattleVars.Shared.AbilityCardOrigin = creature.Item2;
            creature.Item2.AbilityUsed = true;
            SkillManager.Instance.SkillRoutineWithTarget(aiManager, target.Item1, target.Item2);
            return;
        }
        
        creature.Item2.AbilityUsed = true;
        SkillManager.Instance.SkillRoutineNoTarget(aiManager, creature.Item1, creature.Item2);
    }

    public override void ActivateArtifactAbility(PlayerManager aiManager)
    {
        var artifact = aiManager.playerPermanentManager.GetAllValidCardIds().Find(a => a.card.skill is not null or "" or " " && aiManager.IsAbilityUsable(a.card));
        if (artifact.Equals(default)) return;

        if (SkillManager.Instance.ShouldAskForTarget(artifact.card))
        {
            var target = SkillManager.Instance.GetRandomTarget(aiManager, artifact.card);
            if (target.Equals(default)) return;

            BattleVars.Shared.AbilityIDOrigin = artifact.id;
            BattleVars.Shared.AbilityCardOrigin = artifact.card;
            artifact.Item2.AbilityUsed = true;
            SkillManager.Instance.SkillRoutineWithTarget(aiManager, target.Item1, target.Item2);
            return;
        }
        
        artifact.card.AbilityUsed = true;
        SkillManager.Instance.SkillRoutineNoTarget(aiManager, artifact.id, artifact.card);
    }
    
    public override bool HasCardInHand(PlayerManager aiManager, CardType cardToCheck)
    {
        return aiManager.playerHand.GetPlayableCards(aiManager.HasSufficientQuanta).Count > 0; 
    }

    public override bool HasCreatureAbilityToUse(PlayerManager aiManager)
    {
        var creatureWithSkill = aiManager.playerCreatureField.GetAllValidCardIds().Find(x => x.Item2.skill is not null or "" or " ");
        return !creatureWithSkill.Equals(default) && aiManager.IsAbilityUsable(creatureWithSkill.Item2);
    }

    public override bool HasArtifactAbilityToUse(PlayerManager aiManager)
    {
        var artifactWithSkill = aiManager.playerPermanentManager.GetAllValidCardIds().Find(x => x.Item2.skill is not null or "" or " ");
        return !artifactWithSkill.Equals(default) && aiManager.IsAbilityUsable(artifactWithSkill.Item2);
    }

    public override bool HasSpellToUse(PlayerManager aiManager)
    {
        return aiManager.playerHand.GetPlayableCards(aiManager.HasSufficientQuanta).Where(x => x.Item2.cardType.Equals(CardType.Spell)).ToList().Count > 0; 
    }
} 