using System.Collections.Generic;
using System.Linq;

public class Thunderstorm : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        var victim = DuelManager.GetNotIDOwner(target.id);
        var idList = victim.playerCreatureField.GetAllValidCardIds();

        Game_SoundManager.shared.PlayAudioClip("Lightning");
        foreach (var idCardi in idList)
        {
            idCardi.card.DefDamage += 2;
            idCardi.UpdateCard();
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
