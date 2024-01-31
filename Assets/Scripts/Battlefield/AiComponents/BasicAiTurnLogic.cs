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
        var card = aiManager.playerHand.GetPlayableCards(aiManager.HasSufficientQuanta).FirstOrDefault(c => c.Item2.cardType.Equals(cardType));
        if (card.Equals(default)) return;
        
        EventBus<DisplayCardPlayedEvent>.Raise(new DisplayCardPlayedEvent(card.Item2.imageID, card.Item2.costElement.FastElementString(), card.Item2.cardName));
        EventBus<PlayCardFromHandEvent>.Raise(new PlayCardFromHandEvent(card.card, card.id));
    }

    public async override void PlaySpellFromHand(PlayerManager aiManager)
    {
        var cardList = aiManager.playerHand.GetPlayableCardsOfType(aiManager.HasSufficientQuanta, CardType.Spell);
        var spell = cardList.FirstOrDefault();
        if (spell.Equals(default)) return;

        if (SkillManager.Instance.ShouldAskForTarget(spell.card))
        {
            BattleVars.Shared.AbilityCardOrigin = spell.Item2;
            BattleVars.Shared.AbilityIDOrigin = spell.Item1;
            EventBus<SetupAbilityTargetsEvent>.Raise(new SetupAbilityTargetsEvent(aiManager, spell.card));
            await new WaitForSeconds(2f);
            var target = _targetingAi.BestTarget(spell.card.skill.GetAITargetType(), spell.card.skill);
            
            if (target.Equals(default))
            {
                DuelManager.Instance.ResetTargeting();
                return;
            }
            
            
            EventBus<ActivateSpellOrAbilityEvent>.Raise(new ActivateSpellOrAbilityEvent(target.id, target.card));
            EventBus<DisplayCardPlayedEvent>.Raise(new DisplayCardPlayedEvent(spell.Item2.imageID, spell.Item2.costElement.FastElementString(), spell.Item2.cardName));
        }
        else
        {
            SkillManager.Instance.SkillRoutineNoTarget(aiManager, spell.id, spell.card);
            EventBus<PlayCardFromHandEvent>.Raise(new PlayCardFromHandEvent(spell.card, spell.id));
            EventBus<DisplayCardPlayedEvent>.Raise(new DisplayCardPlayedEvent(spell.Item2.imageID, spell.Item2.costElement.FastElementString(), spell.Item2.cardName));
        }

    }

    public async override void ActivateCreatureAbility(PlayerManager aiManager)
    {
        var creature = aiManager.playerCreatureField.GetAllValidCardIds().Find(c => c.Item2.skill is not null or "" or " " && aiManager.IsAbilityUsable(c.Item2));
        if (creature.Equals(default)) return;

        if (SkillManager.Instance.ShouldAskForTarget(creature.Item2))
        {
            BattleVars.Shared.AbilityIDOrigin = creature.Item1;
            BattleVars.Shared.AbilityCardOrigin = creature.Item2;
            EventBus<SetupAbilityTargetsEvent>.Raise(new SetupAbilityTargetsEvent(aiManager, creature.card));
            await new WaitForSeconds(2f);
            var target = _targetingAi.BestTarget(creature.card.skill.GetAITargetType(), creature.card.skill);
            if (target.Equals(default))
            {
                DuelManager.Instance.ResetTargeting();
                return;
            }

            creature.Item2.AbilityUsed = true;
            
            EventBus<ActivateSpellOrAbilityEvent>.Raise(new ActivateSpellOrAbilityEvent(target.id, target.card));
            return;
        }
        
        creature.Item2.AbilityUsed = true;
        SkillManager.Instance.SkillRoutineNoTarget(aiManager, creature.Item1, creature.Item2);
    }

    public async override void ActivateArtifactAbility(PlayerManager aiManager)
    {
        var artifact = aiManager.playerPermanentManager.GetAllValidCardIds().Find(a => a.card.skill is not null or "" or " " && aiManager.IsAbilityUsable(a.card));
        if (artifact.Equals(default)) return;

        if (SkillManager.Instance.ShouldAskForTarget(artifact.card))
        {
            BattleVars.Shared.AbilityIDOrigin = artifact.id;
            BattleVars.Shared.AbilityCardOrigin = artifact.card;
            EventBus<SetupAbilityTargetsEvent>.Raise(new SetupAbilityTargetsEvent(aiManager, artifact.card));
            await new WaitForSeconds(2f);
            var target = _targetingAi.BestTarget(artifact.card.skill.GetAITargetType(), artifact.card.skill);
            if (target.Equals(default))
            {
                DuelManager.Instance.ResetTargeting();
                return;
            }

            artifact.Item2.AbilityUsed = true;
            EventBus<ActivateSpellOrAbilityEvent>.Raise(new ActivateSpellOrAbilityEvent(target.id, target.card));
            return;
        }
        
        artifact.card.AbilityUsed = true;
        SkillManager.Instance.SkillRoutineNoTarget(aiManager, artifact.id, artifact.card);
    }
    
    public override bool HasCardInHand(PlayerManager aiManager, CardType cardToCheck)
    {
        return aiManager.playerHand.GetPlayableCardsOfType(aiManager.HasSufficientQuanta, cardToCheck).Count > 0; 
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
    
}