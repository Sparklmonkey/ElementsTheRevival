using System.Collections.Generic;

public class Rainoffire : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        var victim = DuelManager.Instance.GetNotIDOwner(target.id);
        var idList = victim.playerCreatureField.GetAllValidCardIds();
    
        EventBus<PlaySoundEffectEvent>.Raise(new PlaySoundEffectEvent("Lightning"));
        foreach (var idCardi in idList)
        {
            if (idCardi.card.innateSkills.Immaterial) { continue; }
            if (idCardi.card.passiveSkills.Burrow) { continue; }
            idCardi.card.DefDamage += 3;
            idCardi.UpdateCard();
        }

        if (victim.playerCounters.invisibility > 0)
        {
            victim.RemoveAllCloaks();
        }
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        return new();
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> possibleTargets)
    {
        return null;
    }
    public override TargetPriority GetPriority() => TargetPriority.Any;
}
