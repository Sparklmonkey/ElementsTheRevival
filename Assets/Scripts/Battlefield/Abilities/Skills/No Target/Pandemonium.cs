using System.Collections.Generic;

public class Pandemonium : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        var idList = DuelManager.Instance.player.playerCreatureField.GetAllValidCardIds();

        foreach (var idCardi in idList)
        {
            if (idCardi.card.innateSkills.Immaterial) { continue; }
            if (idCardi.card.passiveSkills.Burrow) { continue; }
            var chaos = new Chaos { Owner = Owner };
            chaos.Activate(idCardi);
        }

        idList = DuelManager.Instance.enemy.playerCreatureField.GetAllValidCardIds();

        foreach (var idCardi in idList)
        {
            if (idCardi.card.innateSkills.Immaterial) { continue; }
            if (idCardi.card.passiveSkills.Burrow) { continue; }
            var chaos = new Chaos { Owner = Owner };
            chaos.Activate(idCardi);
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
