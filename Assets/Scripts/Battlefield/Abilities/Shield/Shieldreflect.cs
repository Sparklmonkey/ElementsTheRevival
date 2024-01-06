public class Shieldreflect : ShieldAbility
{
    public override int ActivateShield(int atkNow, (ID id, Card card) cardPair)
    {
        if (!cardPair.card.passiveSkills.Psion) return atkNow;
        EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(atkNow, true, false, Enemy.Owner));
        return 0;
    }
}
