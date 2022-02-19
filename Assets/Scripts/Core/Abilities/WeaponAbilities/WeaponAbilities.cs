using UnityEngine;

public class WeaponArsenic : IWeaponAbility
{
    public int GetDamageToDeal(int baseDamage)
    {
        return baseDamage;
    }

    public void EffectAfterDamage(int damageDealt, ID targetDamaged)
    {
        DuelManager.GetIDOwner(targetDamaged).ApplyPlayerCounterLogic(CounterEnum.Poison, 1);
    }
}

public class WeaponDiscord : IWeaponAbility
{
    public int GetDamageToDeal(int baseDamage)
    {
        return baseDamage;
    }

    public void EffectAfterDamage(int damageDealt, ID targetDamaged)
    {
        DuelManager.GetIDOwner(targetDamaged).ScrambleQuanta();
    }
}

public class WeaponFiery : IWeaponAbility
{
    public int GetDamageToDeal(int baseDamage)
    {
        int num = DuelManager.GetAllQuantaOfElement(Element.Fire) / 5;
        return baseDamage + num;
    }

    public void EffectAfterDamage(int damageDealt, ID targetDamaged)
    {
    }
}


public class WeaponMarkAir : IWeaponAbility
{
    public int GetDamageToDeal(int baseDamage)
    {
        if (!DuelManager.GetMarkElement().Equals(Element.Air))
        {
            return baseDamage;
        }
        return baseDamage + 1;
    }

    public void EffectAfterDamage(int damageDealt, ID targetDamaged)
    {
    }
}


public class WeaponMarkDD : IWeaponAbility
{
    public void EffectAfterDamage(int damageDealt, ID targetDamaged)
    {
    }

    public int GetDamageToDeal(int baseDamage)
    {
        if (DuelManager.GetMarkElement().Equals(Element.Death) || DuelManager.GetMarkElement().Equals(Element.Darkness))
        {
            return baseDamage + 1;
        }
        return baseDamage;
    }
}

public class WeaponMarkGE : IWeaponAbility
{
    public int GetDamageToDeal(int baseDamage)
    {
        if (!DuelManager.GetMarkElement().Equals(Element.Gravity) && !DuelManager.GetMarkElement().Equals(Element.Earth))
        {
            return baseDamage;
        }
        return baseDamage + 1;
    }

    public void EffectAfterDamage(int damageDealt, ID targetDamaged)
    {
    }
}

public class WeaponNone : IWeaponAbility
{
    public int GetDamageToDeal(int baseDamage)
    {
        return baseDamage;
    }

    public void EffectAfterDamage(int damageDealt, ID targetDamaged)
    {
    }
}

public class WeaponRegen : IWeaponAbility
{
    public int GetDamageToDeal(int baseDamage)
    {
        return baseDamage;
    }

    public void EffectAfterDamage(int damageDealt, ID targetDamaged)
    {
        DuelManager.GetNotIDOwner(targetDamaged).ModifyHealthLogic(5, false, false);
    }
}

public class WeaponVampire : IWeaponAbility
{
    public int GetDamageToDeal(int baseDamage)
    {
        return baseDamage;
    }

    public void EffectAfterDamage(int damageDealt, ID targetDamaged)
    {
        DuelManager.GetNotIDOwner(targetDamaged).ModifyHealthLogic(damageDealt, false, false);
    }
}
public class WeaponFahrenheit : IWeaponAbility
{
    public int GetDamageToDeal(int baseDamage)
    {
        int fireQ = DuelManager.GetOtherPlayer().GetAllQuantaOfElement(Element.Fire);
        fireQ = fireQ > 5 ? Mathf.FloorToInt(fireQ / 5) : 0;
        return baseDamage + fireQ;
    }

    public void EffectAfterDamage(int damageDealt, ID targetDamaged)
    {
        return;
    }
}
