public interface IShieldAbility
{
    int GetDamageToDeal(int damage, int toughness, bool canReach);

    void ApplyDebuff(Card attacker);
}
