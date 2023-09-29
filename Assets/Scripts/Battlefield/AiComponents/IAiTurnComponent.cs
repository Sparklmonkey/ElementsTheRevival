using System.Collections;
using System.Threading.Tasks;

public interface IAiTurnComponent
{
    IEnumerator PlayPillars(PlayerManager aiManager);
    IEnumerator RestOfTurn(PlayerManager aiManager);
}

public abstract class AiTurnBase
{
    public abstract Task PlayPillar(PlayerManager aiManager);
    public abstract Task PlayArtifact(PlayerManager aiManager);
    public abstract Task PlayCreature(PlayerManager aiManager);
    public abstract Task PlaySpell(PlayerManager aiManager);
    public abstract Task ActivateCreature(PlayerManager aiManager);
    public abstract Task ActivateArtifact(PlayerManager aiManager);
    public abstract Task PlayShield(PlayerManager aiManager);
    public abstract Task PlayWeapon(PlayerManager aiManager);

    public abstract bool HasPillarToPlay();
    public abstract bool HasCreatureToPlay();
    public abstract bool HasSpellToPlay();
    public abstract bool HasWeaponToPlay();
    public abstract bool HasArtifactToPlay();
    public abstract bool HasShieldToPlay();
    public abstract bool HasCreatureAbilityToUse();
    public abstract bool HasArtifactAbilityToUse();
}