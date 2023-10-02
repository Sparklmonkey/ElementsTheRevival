using System.Collections;
using System.Threading.Tasks;

public interface IAiTurnComponent
{
    IEnumerator PlayPillars(PlayerManager aiManager);
    IEnumerator RestOfTurn(PlayerManager aiManager);
}

public abstract class AiTurnBase
{
    public abstract void PlayCardFromHand(PlayerManager aiManager, CardType cardType);
    public abstract void PlaySpellFromHand(PlayerManager aiManager);
    public abstract void ActivateCreatureAbility(PlayerManager aiManager);
    public abstract void ActivateArtifactAbility(PlayerManager aiManager);

    public abstract bool HasCardInHand(PlayerManager aiManager, CardType cardToCheck);
    public abstract bool HasCreatureAbilityToUse(PlayerManager aiManager);
    public abstract bool HasArtifactAbilityToUse(PlayerManager aiManager);
    public abstract bool HasSpellToUse(PlayerManager aiManager);
}