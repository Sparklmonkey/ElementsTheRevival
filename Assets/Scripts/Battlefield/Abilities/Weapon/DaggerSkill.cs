namespace Battlefield.Abilities.Weapon
{
    public class DaggerSkill : WeaponSkill
    {
        public override void ModifyWeaponAtk(ID owner, ref int atk)
        {
            var markElement = DuelManager.Instance.GetIDOwner(owner).playerPassiveManager.GetMark().card.CostElement;
            if (markElement is Element.Death or Element.Darkness)
            {
                atk += 1;
            }
        }
    }
}