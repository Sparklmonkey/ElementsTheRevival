public abstract class WeaponSkill
{
    public virtual void ModifyWeaponAtk(ID owner, ref int atk)
    {
        atk += 0;
    }

    public virtual void EndTurnEffect(ID owner)
    {
    }
}