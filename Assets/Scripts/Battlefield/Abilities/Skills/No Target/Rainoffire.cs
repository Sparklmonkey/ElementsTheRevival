using System.Collections.Generic;
using System.Linq;

public class Rainoffire : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        var victim = DuelManager.GetNotIDOwner(target.id);
        var idList = victim.playerCreatureField.GetAllValidCardIds();

        Game_SoundManager.shared.PlayAudioClip("Lightning");
        foreach (var idCardi in idList)
        {
            if (idCardi.card.IsImmaterial) { continue; }
            if (idCardi.card.IsBurrowed) { continue; }
            idCardi.card.DefDamage += 3;
            idCardi.UpdateCard();
        }

        if(victim.playerCounters.invisibility > 0)
        {
            victim.RemoveAllCloaks();
        }
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        return new();
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        return null;
    }
}