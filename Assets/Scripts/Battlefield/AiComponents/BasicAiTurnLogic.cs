using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battlefield.Abilities;
using UnityEngine;

public class BasicAiTurnLogic : AiTurnBase
{
    private TargetingAi _targetingAi = new();
    
    public override void DiscardCard(PlayerManager aiManager)
    {
        if (!aiManager.playerHand.ShouldDiscard()) return;
        var index = Random.Range(0, 8);
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(new ID(OwnerEnum.Opponent, FieldEnum.Hand, index)));
    }
    
    public override void PlayCardFromHand(PlayerManager aiManager, CardType cardType)
    {
        var card = aiManager.playerHand.GetPlayableCards(aiManager.HasSufficientQuanta).FirstOrDefault(c => c.card.Type.Equals(cardType) && !_creatureList.Contains(c.card.CardName));
        
        if (card.Equals(default)) return;
        if (card.card.CardName.Contains("Chimera"))
        {
            if (aiManager.playerCreatureField.GetAllValidCardIds().Count == 0)
            {
                _creatureList.Add(card.card.CardName);
                return;
            }
        }
        EventBus<DisplayCardPlayedEvent>.Raise(new DisplayCardPlayedEvent(card.card.cardImage, card.card.CostElement.FastElementString(), card.card.CardName));
        EventBus<PlayCardFromHandEvent>.Raise(new PlayCardFromHandEvent(card.card, card.id));
    }

    public override IEnumerator PlaySpellFromHand(PlayerManager aiManager)
    {
        bool canPlaySingu = BattleVars.Shared.IsSingularity == 0;
        var cardList = aiManager.playerHand.GetPlayableCardsOfType(aiManager.HasSufficientQuanta, CardType.Spell);
        var spell = cardList.FirstOrDefault(s => !_skipList.Contains(nameof(s.card.Skill)) && (canPlaySingu || s.card.Id is "6u3" or "4vj"));
        if (spell.Equals(default)) yield break;

        BattleVars.Shared.AbilityCardOrigin = spell.Item2;
        BattleVars.Shared.AbilityIDOrigin = spell.Item1;
        if (SkillManager.Instance.ShouldAskForTarget(spell.card))
        {
            EventBus<SetupAbilityTargetsEvent>.Raise(new SetupAbilityTargetsEvent(aiManager, spell.card, true));
            yield return new WaitForSeconds(2f);
            var target = _targetingAi.BestTarget(spell.card.Skill.GetTargetType(), nameof(spell.card.Skill));
            
            if (target.Equals(default))
            {
                _skipList.Add(nameof(spell.card.Skill));
                DuelManager.Instance.ResetTargeting();
                yield break;
            }
            
            
            EventBus<ActivateSpellOrAbilityEvent>.Raise(new ActivateSpellOrAbilityEvent(target.id, target.card));
            EventBus<DisplayCardPlayedEvent>.Raise(new DisplayCardPlayedEvent(spell.Item2.cardImage, spell.Item2.CostElement.FastElementString(), spell.Item2.CardName));
        }
        else
        {
            SkillManager.Instance.SkillRoutineNoTarget(aiManager, spell.id, spell.card);
            EventBus<PlayCardFromHandEvent>.Raise(new PlayCardFromHandEvent(spell.card, spell.id));
            EventBus<DisplayCardPlayedEvent>.Raise(new DisplayCardPlayedEvent(spell.Item2.cardImage, spell.Item2.CostElement.FastElementString(), spell.Item2.CardName));
        }
    }

    public override IEnumerator ActivateCreatureAbility(PlayerManager aiManager)
    {
        var creature = aiManager.playerCreatureField.GetAllValidCardIds().Find(c => c.Item2.Skill is not null 
            && !_skipList.Contains(nameof(c.card.Skill)) && aiManager.IsAbilityUsable(c.Item2));
        if (creature.Equals(default)) yield break;

        BattleVars.Shared.AbilityIDOrigin = creature.Item1;
        BattleVars.Shared.AbilityCardOrigin = creature.Item2;
        if (SkillManager.Instance.ShouldAskForTarget(creature.Item2))
        {
            EventBus<SetupAbilityTargetsEvent>.Raise(new SetupAbilityTargetsEvent(aiManager, creature.card, true));
            yield return new WaitForSeconds(2f);
            var target = _targetingAi.BestTarget(creature.card.Skill.GetTargetType(), nameof(creature.card.Skill));
            if (target.Equals(default))
            {
                _skipList.Add(nameof(creature.card.Skill));
                DuelManager.Instance.ResetTargeting();
                yield break;
            }

            creature.Item2.AbilityUsed = true;
            
            EventBus<ActivateSpellOrAbilityEvent>.Raise(new ActivateSpellOrAbilityEvent(target.id, target.card));
            yield break;
        }
        
        creature.Item2.AbilityUsed = true;
        SkillManager.Instance.SkillRoutineNoTarget(aiManager, creature.Item1, creature.Item2);
    }

    public override IEnumerator ActivateArtifactAbility(PlayerManager aiManager)
    {
        var artifact = aiManager.playerPermanentManager.GetAllValidCardIds().Find(a => a.card.Skill is not null 
            && !_skipList.Contains(nameof(a.card.Skill)) && aiManager.IsAbilityUsable(a.card));
        if (artifact.Equals(default))  yield break;

        BattleVars.Shared.AbilityIDOrigin = artifact.id;
        BattleVars.Shared.AbilityCardOrigin = artifact.card;
        if (SkillManager.Instance.ShouldAskForTarget(artifact.card))
        {
            EventBus<SetupAbilityTargetsEvent>.Raise(new SetupAbilityTargetsEvent(aiManager, artifact.card, true));
            yield return new WaitForSeconds(2f);
            var target = _targetingAi.BestTarget(artifact.card.Skill.GetTargetType(), nameof(artifact.card.Skill));
            if (target.Equals(default))
            {
                _skipList.Add(nameof(artifact.card.Skill));
                DuelManager.Instance.ResetTargeting();
                yield break;
            }

            artifact.Item2.AbilityUsed = true;
            EventBus<ActivateSpellOrAbilityEvent>.Raise(new ActivateSpellOrAbilityEvent(target.id, target.card));
            yield break;
        }
        
        artifact.card.AbilityUsed = true;
        SkillManager.Instance.SkillRoutineNoTarget(aiManager, artifact.id, artifact.card);
    }
    
    public override bool HasCardInHand(PlayerManager aiManager, CardType cardToCheck)
    {
        var playableCards =  aiManager.playerHand.GetPlayableCardsOfType(aiManager.HasSufficientQuanta, cardToCheck);
        if (playableCards.Count == 0) return false;
        if (!cardToCheck.Equals(CardType.Spell)) return true;
        var reducedList = playableCards.FindAll(s => !_skipList.Contains(nameof(s.card.Skill)));
        return reducedList.Count > 0;

    }

    public override bool HasCreatureAbilityToUse(PlayerManager aiManager)
    {
        var creatureWithSkillList = aiManager.playerCreatureField.GetAllValidCardIds().FindAll(x => x.Item2.Skill is not null 
            && !_skipList.Contains(nameof(x.card.Skill)));
        var reducedList = creatureWithSkillList.FindAll(s => aiManager.IsAbilityUsable(s.card));
        return reducedList.Count > 0;
    }

    public override bool HasArtifactAbilityToUse(PlayerManager aiManager)
    {
        var artifactWithSkillList = aiManager.playerPermanentManager.GetAllValidCardIds().FindAll(x => x.Item2.Skill is not null 
            && !_skipList.Contains(nameof(x.card.Skill)));
        var reducedList = artifactWithSkillList.FindAll(s => aiManager.IsAbilityUsable(s.card));
        return reducedList.Count > 0;
    }
    
}