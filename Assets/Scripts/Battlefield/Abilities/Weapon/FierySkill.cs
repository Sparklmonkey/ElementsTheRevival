using UnityEngine;

namespace Battlefield.Abilities.Weapon
{
    public class FierySkill : WeaponSkill
    {
        public override void ModifyWeaponAtk(ID owner, ref int atk)
        {
            var ownerPlayer = DuelManager.Instance.GetIDOwner(owner);
            atk += Mathf.FloorToInt(ownerPlayer.GetAllQuantaOfElement(Element.Fire) / 5f);
        }
    }
}