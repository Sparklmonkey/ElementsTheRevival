public interface IWeaponAbility
{
    int GetDamageToDeal(int baseDamage);

    void EffectAfterDamage(int damageDealt, ID targetDamaged);
}
