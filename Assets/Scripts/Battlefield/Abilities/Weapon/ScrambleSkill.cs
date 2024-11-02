namespace Battlefield.Abilities.Weapon
{
    public class ScrambleSkill : WeaponSkill
    {
        public override void EndTurnEffect(ID owner)
        {
            if (DuelManager.Instance.GetNotIDOwner(owner).playerCounters.sanctuary > 0) return;
            var total = DuelManager.Instance.GetNotIDOwner(owner).PlayerQuantaManager.GetQuantaForElement(Element.Other);

            if (total <= 9)
            {
                EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(total, Element.Other, owner.owner.Not(), false));
                EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(total, Element.Other, owner.owner.Not(), true));
                return;
            }

            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(9, Element.Other, owner.owner.Not(), false));
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(9, Element.Other, owner.owner.Not(), true));
        }
    }
}