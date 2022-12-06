public interface IWeaponAbility
{
    int GetDamageToDeal(int baseDamage, bool isPlayer);

    void EffectAfterDamage(ID targetDamaged);
}
