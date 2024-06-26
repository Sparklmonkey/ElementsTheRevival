using System.Collections.Generic;
using System.Linq;
using Battlefield.Abilities;

public class PlaySpellFromHandStrategy : IStrategy
{
    private readonly (Card card, ID id) _cardToCheck;
    private readonly PlayerManager _aiOwner;
    private readonly TargetingAi _targetingAi;

    public PlaySpellFromHandStrategy(PlayerManager aiOwner)
    {
        _aiOwner = aiOwner;
        _targetingAi = new TargetingAi();
    }
    
    public Node.Status Process((Card card, ID id) cardId)
    {
        if(!_aiOwner.IsCardPlayable(cardId.card)) return Node.Status.Failure;
        PlaySpellLogic(cardId);
        return Node.Status.Success;
    }

    private void PlaySpellLogic((Card card, ID id) cardTuple)
    {
        BattleVars.Shared.AbilityCardOrigin = cardTuple.card;
        BattleVars.Shared.AbilityIDOrigin = cardTuple.id;
        if (SkillManager.Instance.ShouldAskForTarget(cardTuple.card))
        {
            EventBus<SetupAbilityTargetsEvent>.Raise(new SetupAbilityTargetsEvent(_aiOwner, cardTuple.card, true));
            var target = _targetingAi.BestTarget(cardTuple.card.Skill.GetTargetType(), nameof(cardTuple.card.Skill));
            if (target.Equals(default))
            {
                DuelManager.Instance.ResetTargeting();
                return;
            }
            
            EventBus<ActivateSpellOrAbilityEvent>.Raise(new ActivateSpellOrAbilityEvent(target.id, target.card));
            EventBus<DisplayCardPlayedEvent>.Raise(new DisplayCardPlayedEvent(cardTuple.card.cardImage, cardTuple.card.CostElement.FastElementString(), cardTuple.card.CardName));
        }
        else
        {
            SkillManager.Instance.SkillRoutineNoTarget(_aiOwner, cardTuple.id, cardTuple.card);
            EventBus<PlayCardFromHandEvent>.Raise(new PlayCardFromHandEvent(cardTuple.card, cardTuple.id));
            EventBus<DisplayCardPlayedEvent>.Raise(new DisplayCardPlayedEvent(cardTuple.card.cardImage, cardTuple.card.CostElement.FastElementString(), cardTuple.card.CardName));
        }
    }

    public void Reset()
    {
        return;
    } 
}