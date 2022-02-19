using System.Collections.Generic;

public class OnPlayAddDeathTrigger : IOnPlayAbility
{
    public void ActiveActionWhenDestroyed(ID owner)
    {
        DuelManager.GetIDOwner(owner).deathTriggers.Remove(owner);
    }

    public void ActiveActionWhenPlayed(ID owner)
    {
        DuelManager.GetIDOwner(owner).deathTriggers.Add(owner);
    }
}

public class OnPlayFlooding : IOnPlayAbility
{
    public void ActiveActionWhenDestroyed(ID owner)
    {
        DuelManager.RemoveFloodCount();
    }

    public void ActiveActionWhenPlayed(ID owner)
    {
        Game_SoundManager.PlayAudioClip("Flood");
        DuelManager.AddFloodCount();
    }
}

public class OnPlayBoneShield : IOnPlayAbility
{
    public void ActiveActionWhenDestroyed(ID owner)
    {
        DuelManager.GetIDOwner(owner).deathTriggers.Remove(owner);
    }

    public void ActiveActionWhenPlayed(ID owner)
    {
        DuelManager.GetIDOwner(owner).deathTriggers.Add(owner);
        DuelManager.GetIDOwner(owner).ApplyPlayerCounterLogic(CounterEnum.Bone, 7);
    }
}

public class OnPlaySwarm : IOnPlayAbility
{
    public void ActiveActionWhenDestroyed(ID owner)
    {
        DuelManager.GetIDOwner(owner).UpdateScarabs(false);
    }

    public void ActiveActionWhenPlayed(ID owner)
    {
        int scarabHP = DuelManager.GetIDOwner(owner).UpdateScarabs(true);
        DuelManager.GetIDOwner(owner).ModifyCreatureLogic(owner, null, 0, 0, scarabHP);
    }
}

public class OnPlayChimera : IOnPlayAbility
{
    public void ActiveActionWhenDestroyed(ID owner)
    {
        return;
    }

    public void ActiveActionWhenPlayed(ID owner)
    {
        (int, int) chimeraPwrHP = (0, 0);
        List<Card> creatures = DuelManager.GetIDOwner(owner).playerCreatureField.GetAllCards();

        if(creatures != null)
        {
            if(creatures.Count > 0)
            {
                foreach (Card creature in creatures)
                {
                    chimeraPwrHP.Item1 += creature.power;
                    chimeraPwrHP.Item2 += creature.hp;
                }
            }
        }

        DuelManager.GetIDOwner(owner).ModifyCreatureLogic(owner, null, 0, chimeraPwrHP.Item1, chimeraPwrHP.Item2);
    }
}

public class OnPlayEclipse : IOnPlayAbility
{
    public void ActiveActionWhenDestroyed(ID owner)
    {
        DuelManager.UpdateEclipseNightCounter(false, true);
    }

    public void ActiveActionWhenPlayed(ID owner)
    {
        DuelManager.UpdateEclipseNightCounter(true, true);
    }
}

public class OnPlayNightfall : IOnPlayAbility
{
    public void ActiveActionWhenDestroyed(ID owner)
    {
        DuelManager.UpdateEclipseNightCounter(false, false);
    }

    public void ActiveActionWhenPlayed(ID owner)
    {
        DuelManager.UpdateEclipseNightCounter(true, false);
    }
}

public class OnPlayAddSanctuaryFlag : IOnPlayAbility
{
    public void ActiveActionWhenDestroyed(ID owner)
    {
        DuelManager.GetIDOwner(owner).sanctuaryCount--;
    }

    public void ActiveActionWhenPlayed(ID owner)
    {
        DuelManager.GetIDOwner(owner).sanctuaryCount++;
    }
}

public class OnPlayAddStasis : IOnPlayAbility
{
    public void ActiveActionWhenDestroyed(ID owner)
    {
        return;
    }

    public void ActiveActionWhenPlayed(ID owner)
    {
        DuelManager.GetNotIDOwner(owner).ApplyPlayerCounterLogic(CounterEnum.Delay, 1);
    }
}

public class OnPlayAddPatienceFlag : IOnPlayAbility
{
    public void ActiveActionWhenDestroyed(ID owner)
    {
        DuelManager.GetIDOwner(owner).patienceCount--;
    }

    public void ActiveActionWhenPlayed(ID owner)
    { 
        DuelManager.GetIDOwner(owner).patienceCount++;
    }
}

public class OnPlayAddFreedomFlag : IOnPlayAbility
{
    public void ActiveActionWhenDestroyed(ID owner)
    {
        DuelManager.GetIDOwner(owner).freedomCount--;
    }

    public void ActiveActionWhenPlayed(ID owner)
    { 
        DuelManager.GetIDOwner(owner).freedomCount++;
    }
}
public class OnPlayAddInvisibleThree : IOnPlayAbility
{
    public void ActiveActionWhenDestroyed(ID owner)
    {
        return;
    }

    public void ActiveActionWhenPlayed(ID owner)
    { 
        PlayerManager player = DuelManager.GetIDOwner(owner);
        List<ID> allIDs = player.GetAllIds();

        foreach (ID item in allIDs)
        {
            if (!item.Field.Equals(FieldEnum.Creature)) { continue; }
            player.ModifyCreatureLogic(item, CounterEnum.Invisible, 3);
        }
    }
}