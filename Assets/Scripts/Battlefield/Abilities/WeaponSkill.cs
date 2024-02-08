public abstract class WeaponSkill
{
    public virtual void ModifyWeaponAtk(ID owner, ref int atk)
    {
    }

    public virtual void EndTurnEffect(ID owner)
    {
    }
}