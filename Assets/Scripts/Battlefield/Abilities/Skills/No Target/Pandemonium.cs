using System.Collections.Generic;
using System.Linq;

public class Pandemonium : AbilityEffect
{
    public override bool NeedsTarget() => false;
    public override bool IsCardValid(ID id, Card card) => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var cardList = DuelManager.Instance.player.playerCreatureField.GetAllValidCardIds();

        foreach (var pair in cardList.Where(pair => !pair.Item2.innateSkills.Immaterial).Where(pair => !pair.Item2.passiveSkills.Burrow))
        {
            var chaos = new Chaos { Owner = Owner };
            chaos.Activate(pair.Item1, pair.Item2);
        }

        cardList = DuelManager.Instance.enemy.playerCreatureField.GetAllValidCardIds();

        foreach (var pair in cardList.Where(pair => !pair.Item2.innateSkills.Immaterial).Where(pair => !pair.Item2.passiveSkills.Burrow))
        {
            if (pair.Item2.innateSkills.Immaterial) { continue; }
            if (pair.Item2.passiveSkills.Burrow) { continue; }
            var chaos = new Chaos { Owner = Owner };
            chaos.Activate(pair.Item1, pair.Item2);
        }
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy) => new List<(ID, Card)>();

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets) => default;
    public override TargetPriority GetPriority() => TargetPriority.Any;
}
