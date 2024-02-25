public class EPandemonium : ActivatedAbility
{
    public override bool NeedsTarget() => false;
    public override bool IsCardValid(ID id, Card card)
    {
        if (!id.owner.Equals(BattleVars.Shared.AbilityIDOrigin.owner)) return false;
        if (card is null) return false;
        return card.Type.Equals(CardType.Creature) && card.IsTargetable();
    }

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var chaos = new Chaos();
        chaos.Activate(targetId, targetCard);
    }
}