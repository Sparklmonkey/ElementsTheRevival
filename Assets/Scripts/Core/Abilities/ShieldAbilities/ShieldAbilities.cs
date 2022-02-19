using UnityEngine;

public class ShieldBubble : IShieldAbility
{
    public void ApplyDebuff(Card attacker)
    {
        if (attacker.power > 0 && attacker.type.Equals(CardType.Creature))
        {
            attacker.cardCounters.delay += 2;
        }
    }

    public int GetDamageToDeal(int damage, int toughness, bool canReach)
    {
        return damage;
    }
}



public class ShieldDissipate : IShieldAbility
{
    public void ApplyDebuff(Card attacker)
    {
    }

    public int GetDamageToDeal(int damage, int toughness, bool canReach)
    {
        if (DuelManager.GetAllOpponentQuanta(Element.Entropy) > 0)
        {
            DuelManager.StealQuanta(Element.Entropy, 1);
            return damage - 3 > 0 ? damage - 3 : 0;
        }
        return damage;
    }
}



public class ShieldDissipatePlus : IShieldAbility
{
    public void ApplyDebuff(Card attacker)
    {
    }

    public int GetDamageToDeal(int damage, int toughness, bool canReach)
    {
        int allOpponentQuanta = DuelManager.GetAllOpponentQuanta(Element.Other);
        if (allOpponentQuanta > damage)
        {
            DuelManager.StealQuanta(Element.Other, damage);
            return 0;
        }
        if (allOpponentQuanta > 0)
        {
            DuelManager.StealQuanta(Element.Other, allOpponentQuanta);
            return damage - allOpponentQuanta;
        }
        return damage;
    }
}


public class ShieldDodgeFifty : IShieldAbility
{
    public void ApplyDebuff(Card attacker)
    {
    }

    public int GetDamageToDeal(int damage, int toughness, bool canReach)
    {
        if (Random.Range(1, 100) >= 50)
        {
            return damage;
        }
        return 0;
    }
}

public class ShieldDodgeForty : IShieldAbility
{
    public void ApplyDebuff(Card attacker)
    {
    }

    public int GetDamageToDeal(int damage, int toughness, bool canReach)
    {
        if (Random.Range(1, 100) >= 40)
        {
            return damage;
        }
        return 0;
    }
}


public class ShieldFreezeReduceOne : IShieldAbility
{
    public void ApplyDebuff(Card attacker)
    {
        attacker.cardCounters.freeze = ((Random.Range(0, 100) < 30) ? 2 : attacker.cardCounters.freeze);
    }

    public int GetDamageToDeal(int damage, int toughness, bool canReach)
    {
        if (damage <= 0)
        {
            return 0;
        }
        return damage - 1;
    }
}



public class ShieldFreezeReduceTwo : IShieldAbility
{
    public void ApplyDebuff(Card attacker)
    {
        attacker.cardCounters.freeze = ((Random.Range(0, 100) < 30) ? 2 : 0);
    }

    public int GetDamageToDeal(int damage, int toughness, bool canReach)
    {
        if (damage <= 1)
        {
            return 0;
        }
        return damage - 2;
    }
}

public class ShieldGravity : IShieldAbility
{
    public void ApplyDebuff(Card attacker)
    {
    }

    public int GetDamageToDeal(int damage, int toughness, bool canReach)
    {
        if (toughness <= 5)
        {
            return damage;
        }
        return 0;
    }
}

public class ShieldHope : IShieldAbility
{
    public void ApplyDebuff(Card attacker)
    {
    }

    public int GetDamageToDeal(int damage, int toughness, bool canReach)
    {
        int lightEmittingCreatures = DuelManager.GetLightEmittingCreatures();
        if (damage <= lightEmittingCreatures)
        {
            return 0;
        }
        return damage - lightEmittingCreatures;
    }
}

public class ShieldHopePlus : IShieldAbility
{
    public void ApplyDebuff(Card attacker)
    {
    }

    public int GetDamageToDeal(int damage, int toughness, bool canReach)
    {
        int num = DuelManager.GetLightEmittingCreatures() + 1;
        if (damage <= num)
        {
            return 0;
        }
        return damage - num;
    }
}

public class ShieldNone : IShieldAbility
{
    public void ApplyDebuff(Card attacker)
    {
    }

    public int GetDamageToDeal(int damage, int toughness, bool canReach)
    {
        return damage;
    }
}

public class ShieldPoison : IShieldAbility
{
    public void ApplyDebuff(Card attacker)
    {
        attacker.cardCounters.poison++;
    }

    public int GetDamageToDeal(int damage, int toughness, bool canReach)
    {
        return damage;
    }
}

public class ShieldPoisonReduceOne : IShieldAbility
{
    public void ApplyDebuff(Card attacker)
    {
        attacker.cardCounters.poison++;
    }

    public int GetDamageToDeal(int damage, int toughness, bool canReach)
    {
        if (damage <= 0)
        {
            return 0;
        }
        return damage - 1;
    }
}


public class ShieldReduceOne : IShieldAbility
{
    public void ApplyDebuff(Card attacker)
    {
    }

    public int GetDamageToDeal(int damage, int toughness, bool canReach)
    {
        if (damage <= 0)
        {
            return 0;
        }
        return damage - 1;
    }
}

public class ShieldReduceOneLight : IShieldAbility
{
    public void ApplyDebuff(Card attacker)
    {
    }

    public int GetDamageToDeal(int damage, int toughness, bool canReach)
    {
        if (damage > 0)
        {
            DuelManager.GenerateQuanta(Element.Light);
            return damage - 1;
        }
        return 0;
    }
}

public class ShieldReduceOneSkeleton : IShieldAbility
{
    public void ApplyDebuff(Card attacker)
    {
        attacker.cardPassives.willSkeleton = Random.Range(0, 100) < attacker.hp / 2;
    }

    public int GetDamageToDeal(int damage, int toughness, bool canReach)
    {
        if (damage <= 0)
        {
            return 0;
        }
        return damage - 1;
    }
}

public class ShieldPhase : IShieldAbility
{
    public void ApplyDebuff(Card attacker)
    {
        return;
    }

    public int GetDamageToDeal(int damage, int toughness, bool canReach)
    {
        return 0;
    }
}

public class ShieldReduceThree : IShieldAbility
{
    public void ApplyDebuff(Card attacker)
    {
    }

    public int GetDamageToDeal(int damage, int toughness, bool canReach)
    {
        if (damage <= 2)
        {
            return 0;
        }
        return damage - 3;
    }
}

public class ShieldReduceTwo : IShieldAbility
{
    public void ApplyDebuff(Card attacker)
    {
    }

    public int GetDamageToDeal(int damage, int toughness, bool canReach)
    {
        if (damage <= 1)
        {
            return 0;
        }
        return damage - 2;
    }
}
public class ShieldThorns : IShieldAbility
{
    public void ApplyDebuff(Card attacker)
    {
        attacker.hp--;
    }

    public int GetDamageToDeal(int damage, int toughness, bool canReach)
    {
        return damage;
    }
}
public class ShieldWings : IShieldAbility
{
    public void ApplyDebuff(Card attacker)
    {
    }

    public int GetDamageToDeal(int damage, int toughness, bool canReach)
    {
        if (!canReach)
        {
            return 0;
        }
        return damage;
    }
}