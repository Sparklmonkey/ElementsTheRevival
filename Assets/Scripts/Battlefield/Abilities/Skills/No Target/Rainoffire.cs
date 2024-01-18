using System.Collections.Generic;
using System.Linq;

public class Rainoffire : AbilityEffect
{
    public override bool NeedsTarget() => false;
    public override bool IsCardValid(ID id, Card card) => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        var victim = DuelManager.Instance.GetNotIDOwner(targetId);
        var idList = victim.playerCreatureField.GetAllValidCardIds();
    
        EventBus<PlaySoundEffectEvent>.Raise(new PlaySoundEffectEvent("Lightning"));
        foreach (var pair in idList.Where(pair => !pair.Item2.innateSkills.Immaterial).Where(pair => !pair.Item2.passiveSkills.Burrow))
        {
            pair.Item2.DefDamage += 3;
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
        }

        if (victim.playerCounters.invisibility > 0)
        {
            victim.RemoveAllCloaks();
        }
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy) => new List<(ID, Card)>();

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets) => default;
    public override TargetPriority GetPriority() => TargetPriority.Any;
}
