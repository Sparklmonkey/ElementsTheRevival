namespace Battlefield.Abilities.Weapon
{
    public class HammerSkill : WeaponSkill
    {
        public override void ModifyWeaponAtk(ID owner, ref int atk)
        {
            var markElement = DuelManager.Instance.GetIDOwner(owner).playerPassiveManager.GetMark().card.CostElement;
            if (markElement is Element.Earth or Element.Gravity)
            {
                atk += 1;
            }
        }
    }
}