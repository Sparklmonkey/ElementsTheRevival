using System.Collections.Generic;
using System.Linq;

public class Pandemonium : ActivatedAbility
{
    public override bool NeedsTarget() => false;
    public override bool IsCardValid(ID id, Card card)
    {
        return card.cardType.Equals(CardType.Creature);
    }

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var cardList = DuelManager.Instance.player.playerCreatureField.GetAllValidCardIds();

        foreach (var pair in cardList.Where(pair => !pair.Item2.innateSkills.Immaterial).Where(pair => !pair.Item2.passiveSkills.Burrow))
        {
            var chaos = new Chaos();
            chaos.Activate(pair.Item1, pair.Item2);
        }

        cardList = DuelManager.Instance.enemy.playerCreatureField.GetAllValidCardIds();

        foreach (var pair in cardList.Where(pair => !pair.Item2.innateSkills.Immaterial).Where(pair => !pair.Item2.passiveSkills.Burrow))
        {
            if (pair.Item2.innateSkills.Immaterial) { continue; }
            if (pair.Item2.passiveSkills.Burrow) { continue; }
            var chaos = new Chaos();
            chaos.Activate(pair.Item1, pair.Item2);
        }
    }
}
