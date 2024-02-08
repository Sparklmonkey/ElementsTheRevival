namespace Battlefield.Abilities.Weapon
{
    public class RegenerateSkill : WeaponSkill
    {
        public override void EndTurnEffect(ID owner)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(5, false, false, owner.owner));
        }
    }
}