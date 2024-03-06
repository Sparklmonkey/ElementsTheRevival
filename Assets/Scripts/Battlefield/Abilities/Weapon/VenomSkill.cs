namespace Battlefield.Abilities.Weapon
{
    public class VenomSkill : WeaponSkill
    {
        public override void EndTurnEffect(ID owner)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Poison,
                owner.owner.Not(), 1));
        }
    }
}