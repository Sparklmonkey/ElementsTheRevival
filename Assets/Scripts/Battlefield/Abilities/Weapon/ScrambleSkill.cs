namespace Battlefield.Abilities.Weapon
{
    public class ScrambleSkill : WeaponSkill
    {
        public override void EndTurnEffect(ID owner)
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(9, Element.Other, owner.owner.Not(), false));
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(9, Element.Other, owner.owner.Not(), true));
        }
    }
}