public class ShieldReflect : ShieldAbility
{
    public override int ActivateShield(int atkNow, (ID id, Card card) cardPair)
    {
        if (!cardPair.card.passiveSkills.Psion) return atkNow;
        EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(atkNow, true, false, cardPair.id.owner));
        return 0;
    }
}
