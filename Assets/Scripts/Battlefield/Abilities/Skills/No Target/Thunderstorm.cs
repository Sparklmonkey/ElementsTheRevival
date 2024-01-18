using System.Collections.Generic;
using System.Linq;

public class Thunderstorm : AbilityEffect
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
            pair.Item2.DefDamage += 2;
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
        }
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy) => new List<(ID, Card)>();

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets) => default;

    public override TargetPriority GetPriority() => TargetPriority.Any;
}
