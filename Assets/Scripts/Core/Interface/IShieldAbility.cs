using System.Collections;

public interface IShieldAbility
{
    public int Damage { get;set; }
    IEnumerator GetDamageToDeal(Card attacker, ID cardId);

    void ApplyDebuff(Card attacker, ID cardId);
}
