namespace Battlefield.Abilities.Weapon
{
    public class BowSkill : WeaponSkill
    {
        public override void ModifyWeaponAtk(ID owner, ref int atk)
        {
            var markElement = DuelManager.Instance.GetIDOwner(owner).playerPassiveManager.GetMark().card.costElement;
            if (markElement is Element.Air)
            {
                atk += 1;
            }
        }
    }
}