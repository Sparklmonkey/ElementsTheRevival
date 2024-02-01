using System.Collections;
using System.Collections.Generic;

public abstract class AiTurnBase
{
    
    protected List<string> _skipList = new();
    public void ResetSkipList()
    {
        _skipList.Clear();
    }
    
    public abstract void DiscardCard(PlayerManager aiManager);
    public abstract void PlayCardFromHand(PlayerManager aiManager, CardType cardType);
    public virtual IEnumerator PlaySpellFromHand(PlayerManager aiManager)
    {
        yield break;
    }

    public virtual IEnumerator ActivateCreatureAbility(PlayerManager aiManager)
    {
        yield break;
    }
    public virtual IEnumerator ActivateArtifactAbility(PlayerManager aiManager)
    {
        yield break;
    }

    public abstract bool HasCardInHand(PlayerManager aiManager, CardType cardToCheck);
    public abstract bool HasCreatureAbilityToUse(PlayerManager aiManager);
    public abstract bool HasArtifactAbilityToUse(PlayerManager aiManager);
}