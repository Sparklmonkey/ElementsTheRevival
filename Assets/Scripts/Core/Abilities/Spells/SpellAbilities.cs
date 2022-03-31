using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpellNova : ISpellAbility
{
    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        PlayerManager user = DuelManager.GetIDOwner(iD);
        for (int i = 0; i < 12; i++)
        {
            user.GenerateQuantaLogic((Element)i, 1);
        }
        if (BattleVars.shared.isSingularity)
        {
            Card singularity = CardDatabase.GetCardFromResources("Singularity", "Creature", false);
            user.PlayCardOnFieldLogic(singularity);
        }
        BattleVars.shared.isSingularity = true;
    }

    public bool IsValidTarget(ID iD)
    {
        return false;
    }

}

public class SpellSuperNova : ISpellAbility
{
    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        PlayerManager user = DuelManager.GetIDOwner(iD);
        for (int i = 0; i < 12; i++)
        {
            user.GenerateQuantaLogic((Element)i, 2);
        }
        if (BattleVars.shared.isSingularity)
        {
            Card singularity = CardDatabase.GetCardFromResources("Elite Singularity", "Creature", true);
            user.PlayCardOnFieldLogic(singularity);
        }
        BattleVars.shared.isSingularity = true;
    }

    public bool IsValidTarget(ID iD)
    {
        return false;
    }

}

public class SpellSteal : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        Card cardToSteal = DuelManager.GetOpponentCard(iD);
        PlayerManager victim = DuelManager.GetIDOwner(iD);
        PlayerManager user = DuelManager.GetIDOwner(BattleVars.shared.originId);
        Card cardToPlay = CardDatabase.GetCardFromResources(cardToSteal.name, cardToSteal.type.FastCardTypeString(), !cardToSteal.isUpgradable);
        user.PlayCardOnFieldLogic(cardToPlay);
        
        void Effect() => victim.RemoveCardFromFieldLogic(iD);
        Game_AnimationManager.PlayAnimation("Steal", Battlefield_ObjectIDManager.shared.GetObjectFromID(iD), Effect);
    }

    public bool IsValidTarget(ID iD)
    {
        return (iD.Field.Equals(FieldEnum.Passive) || iD.Field.Equals(FieldEnum.Permanent)) && iD.Owner.Equals(OwnerEnum.Opponent);
    }

}

public class SpellThunderstorm : ISpellAbility
{
    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        DuelManager.SetupValidTargets(this, null);
        List<ID> validTargets = DuelManager.GetAllValidTargets();
        Game_SoundManager.PlayAudioClip("Lightning");
        foreach (ID target in validTargets)
        {
            void Effect() => DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, null, 0, 0, 0, null, false, 2);
            Game_AnimationManager.PlayAnimation("Lightning", Battlefield_ObjectIDManager.shared.GetObjectFromID(target), Effect);
        }

    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player);
    }
}

public class SpellFirebolt : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        int extraDamage = DuelManager.GetAllQuantaOfElement(Element.Fire);
        extraDamage = (extraDamage / 10 > 0) ? (extraDamage / 10 * 3) : 3;

        if (iD.Field.Equals(FieldEnum.Creature))
        {
            DuelManager.GetIDOwner(iD).ModifyCreatureLogic(iD, null, 0, 0, 0, null, false, extraDamage);
        }
        else
        {
            DuelManager.GetIDOwner(iD).ModifyHealthLogic(extraDamage, true, true);
        }
    }

    public bool IsValidTarget(ID iD)
    {
        return (iD.Field.Equals(FieldEnum.Creature) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player)) ||
            (iD.Field.Equals(FieldEnum.Player) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player));
    }
}

public class SpellIceBolt : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        PlayerManager player = DuelManager.GetIDOwner(iD);
        int extraDamage = DuelManager.GetAllQuantaOfElement(Element.Water);
        extraDamage = (extraDamage / 10 > 0) ? (extraDamage / 10 * 3) : 3;
        bool willFreeze = (extraDamage * 5) > Random.Range(0, 100);
        if (iD.Field.Equals(FieldEnum.Creature))
        {
            void Effect() => player.ModifyCreatureLogic(iD, CounterEnum.Freeze, willFreeze ? 1 : 0, 0, 0, null, false, extraDamage);
            Game_AnimationManager.PlayAnimation("IceBolt", Battlefield_ObjectIDManager.shared.GetObjectFromID(iD), Effect);
        }
        else
        {
            if (willFreeze)
            {
                player.ApplyPlayerCounterLogic(CounterEnum.Freeze, 1);
            }
            player.ModifyHealthLogic(extraDamage, true, true);
        }
    }

    public bool IsValidTarget(ID iD)
    {
        return (iD.Field.Equals(FieldEnum.Creature) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player)) ||
            (iD.Field.Equals(FieldEnum.Player) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player));
    }
}

public class SpellRainOfFire : ISpellAbility
{
    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {

        DuelManager.SetupValidTargets(this, null);
        List<ID> validTargets = DuelManager.GetAllValidTargets();
        PlayerManager targetOwners = DuelManager.GetIDOwner(validTargets[0]);
        foreach (ID target in validTargets)
        {
            targetOwners.ModifyCreatureLogic(target, null, 0, 0, 0, null, false, 3);
        }
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player);
    }
}

public class SpellDeflagration : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        DuelManager.GetIDOwner(iD).RemoveCardFromFieldLogic(iD);
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player) && !iD.Field.Equals(FieldEnum.Hand) && !iD.Field.Equals(FieldEnum.Player);
    }
}

public class SpellLightning : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        Game_SoundManager.PlayAudioClip("Lightning");
        if (iD.Field.Equals(FieldEnum.Creature))
        {
            void Effect() => DuelManager.GetIDOwner(iD).ModifyCreatureLogic(iD, null, 0, 0, 0, null, false, 5);
            Game_AnimationManager.PlayAnimation("Lightning", Battlefield_ObjectIDManager.shared.GetObjectFromID(iD), Effect);
        }
        else
        {
            DuelManager.GetIDOwner(iD).ModifyHealthLogic(5, true, true);
        }
    }

    public bool IsValidTarget(ID iD)
    {
        if (!iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player))
        {
            return false;
        }
        return iD.Field.Equals(FieldEnum.Creature) || iD.Field.Equals(FieldEnum.Player);
    }
}

public class SpellDrainLife : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        int extraDamage = DuelManager.GetAllQuantaOfElement(Element.Darkness);
        Game_SoundManager.PlayAudioClip("DrainLife");
        extraDamage = (extraDamage / 10 > 0) ? (extraDamage / 10 * 2) : 2;
        if (iD.Field.Equals(FieldEnum.Creature))
        {
            void Effect() => DuelManager.GetIDOwner(iD).ModifyCreatureLogic(iD, null, 0, 0, 0, null, false, extraDamage);
            Game_AnimationManager.PlayAnimation("DrainLife", Battlefield_ObjectIDManager.shared.GetObjectFromID(iD), Effect);
        }
        else
        {
            DuelManager.GetIDOwner(BattleVars.shared.originId).ModifyHealthLogic(extraDamage, true, true);
        }
        DuelManager.GetIDOwner(BattleVars.shared.originId).ModifyHealthLogic(extraDamage, false, true);
    }

    public bool IsValidTarget(ID iD)
    {
        return (iD.Field.Equals(FieldEnum.Creature) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player)) ||
            (iD.Field.Equals(FieldEnum.Player) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player));
    }
}

public class SpellBlessing : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        DuelManager.GetIDOwner(iD).ModifyCreatureLogic(iD, null, 0, 3, 3);
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent);
    }
}

public class SpellReverseTime : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        PlayerManager iDOwner = DuelManager.GetIDOwner(iD);
        Card creature = iDOwner.GetCard(iD);
        if (creature.cardPassives.isMummy)
        {
            Card pharoah = CardDatabase.GetCardFromResources(creature.isUpgradable ? "Pharaoh" : "Elite Pharaoh", "Creature", !creature.isUpgradable);
            iDOwner.PlayCardOnFieldLogic(pharoah);
        }
        else if (creature.cardPassives.isUndead)
        {
            Card rndCreature = creature.isUpgradable ? CardDatabase.GetRandomCreature() : CardDatabase.GetRandomEliteCreature();
            iDOwner.PlayCardOnFieldLogic(rndCreature);
        }
        else
        {
            iDOwner.AddCardToDeck(creature);
        }
        iDOwner.RemoveCardFromFieldLogic(iD);
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature);
    }
}

public class SpellChaosPower : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        int modifyAmount = Random.Range(0, 6);
        if (DuelManager.GetIDOwner(iD).GetCard(iD).name.Contains("Singularity"))
        {
            DuelManager.GetIDOwner(iD).ModifyCreatureLogic(iD, null, 0, -modifyAmount, -modifyAmount);
        }
        else
        {
            DuelManager.GetIDOwner(iD).ModifyCreatureLogic(iD, null, 0, modifyAmount, modifyAmount);
        }
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature);
    }
}

public class SpellHeal : ISpellAbility
{
    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        DuelManager.GetIDOwner(iD).ModifyHealthLogic(20, false, true);
    }

    public bool IsValidTarget(ID iD)
    {
        return false;
    }
}

public class SpellCongeal : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        DuelManager.GetIDOwner(iD).ModifyCreatureLogic(iD, CounterEnum.Freeze, 4);
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player);
    }
}

public class SpellPlateArmor : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        DuelManager.GetIDOwner(iD).ModifyCreatureLogic(iD, null, 0, 0, 3);
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent);
    }
}

public class SpellHeavyArmor : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        DuelManager.GetIDOwner(iD).ModifyCreatureLogic(iD, null, 0, 0, 6);
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent);
    }
}

public class SpellParallelUniverse : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        Card cardToSteal = DuelManager.GetIDOwner(iD).GetCard(iD);
        Card cardToPlay = CardDatabase.GetCardFromResources(cardToSteal.name, cardToSteal.type.FastCardTypeString(), !cardToSteal.isUpgradable);
        PlayerManager user = DuelManager.GetIDOwner(BattleVars.shared.originId);
        void Effect() => user.PlayCardOnFieldLogic(cardToPlay);
        Game_AnimationManager.PlayAnimation("ParallelUniverse", Battlefield_ObjectIDManager.shared.GetObjectFromID(iD), Effect);
        
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature);
    }
}

public class SpellMutation : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        
        void Effect() => DuelManager.GetNotIDOwner(iD).RemoveCardFromFieldLogic(iD);
        Game_AnimationManager.PlayAnimation("Mutation", Battlefield_ObjectIDManager.shared.GetObjectFromID(iD), Effect);
        switch (GetMutationResult())
        {
            case MutationEnum.Kill:
                break;
            case MutationEnum.Mutate:
                Card mutant = CardDatabase.GetMutant();
                DuelManager.GetIDOwner(iD).PlayCardOnFieldLogic(mutant);
                break;
            case MutationEnum.Abomination:
                Card abomination = CardDatabase.GetCardFromResources("Abomination", "Creature", false);
                DuelManager.GetIDOwner(iD).PlayCardOnFieldLogic(abomination);
                break;
            default:
                break;
        }
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature);
    }

    private MutationEnum GetMutationResult()
    {
        int num = Random.Range(0, 100);
        if (num >= 90)
        {
            return MutationEnum.Kill;
        }
        else if (num >= 50)
        {
            return MutationEnum.Mutate;
        }
        else
        {
            return MutationEnum.Abomination;
        }

    }
}
public class SpellImprovedMutation : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        DuelManager.GetNotIDOwner(iD).RemoveCardFromFieldLogic(iD);
                Card mutant = CardDatabase.GetMutant();
                DuelManager.GetIDOwner(iD).PlayCardOnFieldLogic(mutant);
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature);
        
    }
}

public class SpellCremation : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        PlayerManager user = DuelManager.GetIDOwner(iD);
        for (int i = 0; i < 12; i++)
        {
            user.GenerateQuantaLogic((Element)i, 1);
        }

        user.GenerateQuantaLogic(Element.Fire, 7);

    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent);
    }

}

public class SpellImmolation : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        PlayerManager user = DuelManager.GetIDOwner(iD);
        for (int i = 0; i < 12; i++)
        {
            user.GenerateQuantaLogic((Element)i, 1);
        }

        user.GenerateQuantaLogic(Element.Fire, 5);

    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent);
    }

}

public class SpellMomentum : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        DuelManager.GetIDOwner(iD).ModifyCreatureLogic(iD, null, 0, 1, 1, PassiveEnum.HasMomentum, true);
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent);
    }

}

public class SpellGravityPull : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        DuelManager.GetIDOwner(iD).ModifyCreatureLogic(iD, null, 0, 0, 0, PassiveEnum.HasGravity, true);
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature);
    }

}

public class SpellFreeze : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        DuelManager.GetIDOwner(iD).ModifyCreatureLogic(iD, CounterEnum.Freeze, 3);
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player);
    }
}

public class SpellEarthquake : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        DuelManager.GetIDOwner(iD).RemoveCardFromFieldLogic(iD, 3);
    }

    public bool IsValidTarget(ID iD)
    {
        Card toCheck = DuelManager.GetIDOwner(iD).GetCard(iD);
        return toCheck.type.Equals(CardType.Pillar) && iD.Field.Equals(FieldEnum.Permanent) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player);
    }
}

public class SpellHolyLight : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        PlayerManager target = DuelManager.GetIDOwner(iD);
        if (iD.Field.Equals(FieldEnum.Player))
        {
            target.ModifyHealthLogic(10, false, true);
        }
        else
        {
            Card toCheck = target.GetCard(iD);
            if (toCheck.element.Equals(Element.Darkness) || toCheck.element.Equals(Element.Death))
            {
                target.ModifyCreatureLogic(iD, null, 0, 0, 0, null, false, 10);
            }
            else
            {
                target.ModifyCreatureLogic(iD, null, 0, 0, 0, null, false, -10);
            }
        }
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature) || iD.Field.Equals(FieldEnum.Player);
    }
}

public class SpellShockwave : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        if(DuelManager.GetIDOwner(iD).GetCard(iD).cardCounters.freeze > 0)
        {
            DuelManager.GetIDOwner(iD).RemoveCardFromFieldLogic(iD);
            return;
        }
        DuelManager.GetIDOwner(iD).ModifyCreatureLogic(iD, null, 0, 0, 0, null, false, 4);
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player);
    }
}

public class SpellChaosSeed : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        PlayerManager target = DuelManager.GetIDOwner(iD);
        int effect = Random.Range(0, 12);
        switch (effect)
        {
            case 0:
                target.ModifyCreatureLogic(iD, CounterEnum.Poison, 1);
                break;
            case 1:
                new SpellLightning().ActivateAbility(iD);
                break;
            case 2:
                new SpellIceBolt().ActivateAbility(iD);
                break;
            case 3:
                new SpellFirebolt().ActivateAbility(iD);
                break;
            case 4:
                new SpellFreeze().ActivateAbility(iD);
                break;
            case 5:
                new SpellParallelUniverse().ActivateAbility(iD);
                break;
            case 6:
                target.GetCard(iD).activeAbility = null;
                target.GetCard(iD).description = "";
                break;
            case 7:
                new SpellSteal().ActivateAbility(iD);
                break;
            case 8:
                target.ModifyCreatureLogic(iD, null, 0, 0, 0, null, false, 3);
                break;
            case 9:
                new SpellShockwave().ActivateAbility(iD);
                break;
            case 10:
                new SpellReverseTime().ActivateAbility(iD);
                break;
            case 11:
                new SpellGravityPull().ActivateAbility(iD);
                break;
            default:
                break;
        }
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature);
    }
}

public class SpellFlyingWeapon : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        PlayerManager player = DuelManager.GetIDOwner(iD);
        Card weapon = player.GetCard(iD);
        weapon.type = CardType.Creature;
        player.PlayCardOnFieldLogic(weapon);
        player.RemoveCardFromFieldLogic(iD);
    }

    public bool IsValidTarget(ID iD)
    {
        return DuelManager.GetIDOwner(iD).GetCard(iD).type.Equals(CardType.Weapon) && iD.Field.Equals(FieldEnum.Passive);
    }
}

public class SpellPlague : ISpellAbility
{
    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        DuelManager.SetupValidTargets(this, null);
        List<ID> iDs = DuelManager.GetAllValidTargets();
        PlayerManager player = DuelManager.GetNotIDOwner(iD);
        foreach (ID item in iDs)
        {
            player.ModifyCreatureLogic(item, CounterEnum.Poison, 1);
        }
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player);
    }
}

public class SpellEnchantArtifact : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        PlayerManager player = DuelManager.GetNotIDOwner(iD);
        player.ModifyPermanentLogic(iD, PassiveEnum.IsMaterial, true);
    }

    public bool IsValidTarget(ID iD)
    {
        return (iD.Field.Equals(FieldEnum.Permanent) || iD.Field.Equals(FieldEnum.Passive)) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent);
    }
}

public class SpellPurify : ISpellAbility
{
    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        DuelManager.GetIDOwner(iD).ApplyPlayerCounterLogic(CounterEnum.Purify, 2);
    }

    public bool IsValidTarget(ID iD)
    {
        return false;
    }
}

public class SpellPoison : ISpellAbility
{
    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        DuelManager.GetNotIDOwner(iD).ApplyPlayerCounterLogic(CounterEnum.Poison, 2);
    }

    public bool IsValidTarget(ID iD)
    {
        return false;
    }
}

public class SpellDeadlyPoison : ISpellAbility
{
    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        DuelManager.GetNotIDOwner(iD).ApplyPlayerCounterLogic(CounterEnum.Poison, 3);
    }

    public bool IsValidTarget(ID iD)
    {
        return false;
    }
}

public class SpellAcceleration : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        Card creature = DuelManager.GetIDOwner(iD).GetCard(iD);
        creature.description = "Acceleration: \n Gain +2 /-1 per turn";
        creature.endTurnAbility = new EndTurnAccelerate();
        creature.activeAbility = null;
        creature.onDeathAbility = null;
        creature.onPlayAbility = null;
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature);
    }
}

public class SpellAdrenaline : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        DuelManager.GetIDOwner(iD).ModifyCreatureLogic(iD, null, 0, 0, 0, PassiveEnum.hasAdrenaline, true);
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature);
    }
}

public class SpellAflatoxin : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        DuelManager.GetIDOwner(iD).ModifyCreatureLogic(iD, CounterEnum.Poison, 2, 0, 0, PassiveEnum.HasAflatoxin, true);
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature);
    }
}

public class SpellAntimatter : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        DuelManager.GetIDOwner(iD).ModifyCreatureLogic(iD, null, 0, 0, 0, PassiveEnum.IsAntimatter, true);
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player);
    }
}

public class SpellBasiliskBlood : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        DuelManager.GetIDOwner(iD).ModifyCreatureLogic(iD, CounterEnum.Delay, 6, 0, 20);
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent);
    }
}

public class SpellBlackHole : ISpellAbility
{
    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        PlayerManager victim = DuelManager.GetNotIDOwner(iD);
        int hpToRestore = 0;

        for (int i = 0; i < 12; i++)
        {
            if (victim.HasSufficientQuanta((Element)i, 3))
            {
                victim.SpendQuantaLogic((Element)i, 3);
                hpToRestore++;
            }
        }
        DuelManager.GetIDOwner(iD).ModifyHealthLogic(hpToRestore, false, true);
    }

    public bool IsValidTarget(ID iD)
    {
        return false;
    }
}

public class SpellButterflyEffect : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        Card creature = DuelManager.GetIDOwner(iD).GetCard(iD);
        creature.activeAbility = new ActiveADestroy();
        creature.description = "<sprite=6><sprite=6><sprite=6>: Destroy \n (destroy iD permanent)";
    }

    public bool IsValidTarget(ID iD)
    {
        if(iD.Field.Equals(FieldEnum.Creature) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent))
        {
            return DuelManager.GetIDOwner(iD).GetCard(iD).power < 3;
        }
        return false;
    }
}

public class SpellFractal : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        Card creature = DuelManager.GetIDOwner(iD).GetCard(iD);
        Card newCard = CardDatabase.GetCardFromResources(creature.name, "Creature", !creature.isUpgradable);
        DuelManager.GetIDOwner(iD).FillHandWith(newCard);
        int leftOverQ = DuelManager.GetIDOwner(iD).GetAllQuantaOfElement(Element.Aether);
        DuelManager.GetIDOwner(iD).SpendQuantaLogic(Element.Aether, leftOverQ - 9);
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature);
    }
}

public class SpellLiquidShadow : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        DuelManager.GetIDOwner(iD).ModifyCreatureLogic(iD, CounterEnum.Poison, 1, 0, 0, PassiveEnum.IsVampire, true);
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature);
    }
}

public class SpellMitosis : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        Card creature = DuelManager.GetIDOwner(iD).GetCard(iD);
        creature.activeAbility = new ActiveAMitosis();
        creature.description = "Mitosis: \n Generate a daughter creature";
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature);
    }
}

public class SpellNightmare : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        Card creature = DuelManager.GetIDOwner(iD).GetCard(iD);
        int damage = 7 - DuelManager.GetNotIDOwner(iD).GetHandCards().Count;
        DuelManager.GetNotIDOwner(iD).FillHandWith(creature);
        DuelManager.GetNotIDOwner(iD).ModifyHealthLogic(damage * 2, true, true);
        DuelManager.GetIDOwner(iD).ModifyHealthLogic(damage * 2, false, true);
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature);
    }
}


public class SpellNymphTear : ISpellAbility
{
    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        Card iDCard = DuelManager.GetIDOwner(iD).GetCard(iD);
        Card creature = iDCard.isUpgradable ? CardDatabase.GetRandomRegularNymph(iDCard.element) : CardDatabase.GetRandomEliteNymph(iDCard.element);
        DuelManager.GetIDOwner(iD).RemoveCardFromFieldLogic(iD);
        DuelManager.GetIDOwner(iD).PlayCardOnFieldLogic(creature);
    }
    public bool IsValidTarget(ID iD)
    {
        if (iD.Field.Equals(FieldEnum.Permanent) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent))
        {
            Card permanent = DuelManager.GetIDOwner(iD).GetCard(iD);
            return permanent.type.Equals(CardType.Pillar);
        }
        return false;
    }
}

public class SpellPrecognition : ISpellAbility
{
    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        DuelManager.GetIDOwner(iD).DrawCardFromDeckLogic();
        DuelManager.RevealOpponentsHand();
    }

    public bool IsValidTarget(ID iD)
    {
        return false;
    }
}

public class SpellPandemonium : ISpellAbility
{
    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        DuelManager.SetupValidTargets(this, null);
        foreach (ID id in DuelManager.GetAllValidTargets())
        {
            new SpellChaosSeed().ActivateAbility(id);
        }
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature);
    }
}

public class SpellElitePandemonium : ISpellAbility
{
    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        DuelManager.SetupValidTargets(this, null);
        foreach (ID id in DuelManager.GetAllValidTargets())
        {
            new SpellChaosSeed().ActivateAbility(id);
        }
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player);
    }
}


public class SpellQuintessence : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        DuelManager.GetIDOwner(iD).ModifyCreatureLogic(iD, null, 0, 0, 0, PassiveEnum.IsMaterial, true);
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent);
    }
}

public class SpellShardBravery : ISpellAbility
{
    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        int cardToDraw = DuelManager.GetOtherMarkElement().Equals(Element.Fire) ? 3 : 2;
        for (int i = 0; i < cardToDraw; i++)
        {
            DuelManager.GetIDOwner(iD).DrawCardFromDeckLogic();
            DuelManager.GetNotIDOwner(iD).DrawCardFromDeckLogic();
        }
    }

    public bool IsValidTarget(ID iD)
    {
        return false;
    }
}

public class SpellDivinity : ISpellAbility
{
    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        int maxHPBuff = DuelManager.GetOtherMarkElement().Equals(Element.Light) ? 24 : 16;
        DuelManager.GetIDOwner(iD).ModifyMaxHealthLogic(maxHPBuff, true);
    }

    public bool IsValidTarget(ID iD)
    {
        return false;
    }
}
public class SpellStoneSkin : ISpellAbility
{
    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        int maxHPBuff = DuelManager.GetIDOwner(iD).GetAllQuantaOfElement(Element.Earth);
        DuelManager.GetIDOwner(iD).ModifyMaxHealthLogic(maxHPBuff, true);
    }

    public bool IsValidTarget(ID iD)
    {
        return false;
    }
}
public class SpellSkyBlitz : ISpellAbility
{
    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        int leftOverAir = 9 - DuelManager.GetIDOwner(iD).GetAllQuantaOfElement(Element.Air);
        DuelManager.GetIDOwner(iD).SpendQuantaLogic(Element.Air, leftOverAir);

        DuelManager.SetupValidTargets(this, null);
        foreach (ID id in DuelManager.GetAllValidTargets())
        {
            DuelManager.GetIDOwner(iD).ModifyCreatureLogic(id, null, 0, 0, 0, PassiveEnum.IsDiving, true);
        }
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent);
    }
}
public class SpellSilence : ISpellAbility
{
    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        DuelManager.GetNotIDOwner(iD).ApplyPlayerCounterLogic(CounterEnum.Silence, 1);
    }

    public bool IsValidTarget(ID iD)
    {
        return false;
    }
}
public class SpellWisdomShard : ISpellAbility
{
    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        DuelManager.GetIDOwner(iD).ModifyCreatureLogic(iD, null, 0, 4, 0, PassiveEnum.IsPsion, true);
    }

    public bool IsValidTarget(ID iD)
    {
        if(iD.Field.Equals(FieldEnum.Creature))
        {
            return DuelManager.GetIDOwner(iD).GetCard(iD).cardPassives.isImmaterial;
        }
        return false;
    }
}

public class SpellReadiness : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        PlayerManager player = DuelManager.GetIDOwner(iD);
        player.ModifyCreatureLogic(iD, null, 0, 0, 0, PassiveEnum.isReady, false);
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent);
    }
}

public class SpellLuciferin : ISpellAbility
{

    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        PlayerManager player = DuelManager.GetIDOwner(iD);
        List<ID> allPlayerIDs = player.GetAllIds();
        foreach (ID cardID in allPlayerIDs)
        {
            if (cardID.Field.Equals(FieldEnum.Creature))
            {
                Card creature = player.GetCard(cardID);
                if (creature.activeAbility == null)
                {
                    creature.endTurnAbility = new EndTurnBioluminescence();
                    creature.description = "Each turn <sprite=3> is generated";
                }

            }
        }
    }

    public bool IsValidTarget(ID iD)
    {
        return false;
    }
}

public class SpellProtectArtifact : ISpellAbility
{

    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        PlayerManager player = DuelManager.GetIDOwner(iD);
        player.ModifyCreatureLogic(iD, null, 0, 0, 0, PassiveEnum.IsMaterial, true);
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Passive) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent);
    }
}

public class SpellMiracle : ISpellAbility
{

    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        PlayerManager player = DuelManager.GetIDOwner(iD);
        int maxHp = player.healthManager.GetMaxHealth();
        int currentHP = player.healthManager.GetCurrentHealth();

        int hpToHeal = maxHp - currentHP - 1;

        player.ModifyHealthLogic(hpToHeal, false, true);
        player.SpendQuantaLogic(Element.Light, 999999999);
    }

    public bool IsValidTarget(ID iD)
    {
        return false;
    }
}

public class SpellRageElixir : ISpellAbility
{

    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        PlayerManager player = DuelManager.GetIDOwner(iD);
        
        void Effect() => player.ModifyCreatureLogic(iD, null, 0, 6, -6);
        Game_AnimationManager.PlayAnimation("RagePotion", Battlefield_ObjectIDManager.shared.GetObjectFromID(iD), Effect);
    }

    public bool IsValidTarget(ID iD)
    {
        return DuelManager.GetIDOwner(iD).GetCard(iD).type.Equals(CardType.Creature);
    }
}
public class SpellRagePotion : ISpellAbility
{

    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        PlayerManager player = DuelManager.GetIDOwner(iD);
        void Effect() => player.ModifyCreatureLogic(iD, null, 0, 5, -5);
        Game_AnimationManager.PlayAnimation("RagePotion", Battlefield_ObjectIDManager.shared.GetObjectFromID(iD), Effect);
    }

    public bool IsValidTarget(ID iD)
    {
        return DuelManager.GetIDOwner(iD).GetCard(iD).type.Equals(CardType.Creature);
    }
}
public class SpellOverdrive : ISpellAbility
{
    public bool isTargetFixed => false;

    public void ActivateAbility(ID iD)
    {
        Card creature = DuelManager.GetIDOwner(iD).GetCard(iD);
        creature.description = "Overdrive: \n Gain +3 /-1 per turn";
        creature.endTurnAbility = new EndTurnOverdrive();
        creature.activeAbility = null;
        creature.onDeathAbility = null;
        creature.onPlayAbility = null;
    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature);
    }
}

public class SpellEliteShardSacrifice : ISpellAbility
{

    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        PlayerManager player = DuelManager.GetIDOwner(iD);
        player.ModifyHealthLogic(40, true, true);
        for (int i = 0; i < 12; i++)
        {
            if ((Element)i == Element.Death) { continue; }
            player.SpendQuantaLogic((Element)i, 9999999);
        }
        player.ApplyPlayerCounterLogic(CounterEnum.Sacrifice, 2);
        player.isHealSwapped = !player.isHealSwapped;
    }

    public bool IsValidTarget(ID iD)
    {
        return false;
    }
}
public class SpellShardSacrifice : ISpellAbility
{
    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        PlayerManager player = DuelManager.GetIDOwner(iD);
        player.ModifyHealthLogic(48, true, true);
        for (int i = 0; i < 12; i++)
        {
            if ((Element)i == Element.Death) { continue; }
            player.SpendQuantaLogic((Element)i, 9999999);
        }
        player.ApplyPlayerCounterLogic(CounterEnum.Sacrifice, 2);
        player.isHealSwapped = !player.isHealSwapped;
    }

    public bool IsValidTarget(ID iD)
    {
        return false;
    }
}

public class SpellEliteShardSerendipity : ISpellAbility
{
    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        PlayerManager player = DuelManager.GetIDOwner(iD);

        CardType typeToAdd = ExtensionMethods.GetSerendipityWeighted();
        Element elementToAdd = Element.Entropy;

        for (int i = 0; i < 3; i++)
        {
            player.AddCardToDeck(CardDatabase.GetRandomCardOfTypeWithElement(typeToAdd, elementToAdd, true));
            player.DrawCardFromDeckLogic(true);
            typeToAdd = ExtensionMethods.GetSerendipityWeighted();
            elementToAdd = (Element)Random.Range(0, 13);
            while (typeToAdd.Equals(CardType.Artifact) && elementToAdd.Equals(Element.Earth))
            {
                typeToAdd = ExtensionMethods.GetSerendipityWeighted();
                elementToAdd = (Element)Random.Range(0, 13);
            }
        }
    }

    public bool IsValidTarget(ID iD)
    {
        return false;
    }
}
public class SpellShardSerendipity : ISpellAbility
{

    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        PlayerManager player = DuelManager.GetIDOwner(iD);

        CardType typeToAdd = ExtensionMethods.GetSerendipityWeighted();
        Element elementToAdd = Element.Entropy;

        for (int i = 0; i < 3; i++)
        {
            player.AddCardToDeck(CardDatabase.GetRandomCardOfTypeWithElement(typeToAdd, elementToAdd, false));
            player.DrawCardFromDeckLogic(true);
            typeToAdd = ExtensionMethods.GetSerendipityWeighted();
            elementToAdd = (Element)Random.Range(0, 13);
            while(typeToAdd.Equals(CardType.Artifact) && elementToAdd.Equals(Element.Earth))
            {
                typeToAdd = ExtensionMethods.GetSerendipityWeighted();
                elementToAdd = (Element)Random.Range(0, 13);
            }
        }
    }

    public bool IsValidTarget(ID iD)
    {
        return false;
    }
}



//MARK: TODO
public class SpellShardIntegrity : ISpellAbility
{

    public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        List<int> atkModifier = new List<int> { 2, 2, 0, 1, 2, 3, 2, 2, 2, 2, 2, 2, 1 };
        List<int> hpModifier = new List<int> { 2, 2, 6, 4, 2, 0, 2, 2, 2, 2, 2, 2, 1 };
        PlayerManager player = DuelManager.GetIDOwner(iD);
        List<Card> cardsInHand = player.GetHandCards();

        int golemAtk = 1;
        int golemHp = 4;
        Element lastElement = Element.Other;
        List<QuantaObject> shardElementCounter = new List<QuantaObject>();
        foreach (Card handCard in cardsInHand)
        {
            if (handCard.name.Contains("Shard of"))
            {
                int? index = shardElementCounter.ContainsElement(handCard.element);
                if (index != null)
                {
                    shardElementCounter[(int)index].count++;
                }
                else
                {
                    shardElementCounter.Add(new QuantaObject(handCard.element, 1));
                }
                lastElement = handCard.element;
                golemAtk += atkModifier[(int)handCard.element];
                golemHp += hpModifier[(int)handCard.element];
            }
        }
        Card golem = CardDatabase.GetCardFromResources("Shard Golem", "Creature", true);
        if (lastElement == Element.Other)
        {
            player.PlayCardOnFieldLogic(golem);
            return;
        }
        QuantaObject lastQuantaObject = new QuantaObject(Element.Other, 1);
        foreach (QuantaObject item in shardElementCounter)
        {
            if (item.element == lastElement)
            {
                lastQuantaObject = item;
            }
        }
        player.PlayCardOnFieldLogic(CardDatabase.GetGolemAbility(lastQuantaObject, golem));

    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent);
    }
}
public class SpellEliteShardIntegrity : ISpellAbility
{
public bool isTargetFixed => true;

    public void ActivateAbility(ID iD)
    {
        List<int> atkModifier = new List<int> { 2, 2, 0, 1, 2, 3, 2, 2, 2, 2, 2, 2, 1 };
        List<int> hpModifier = new List<int> { 2, 2, 6, 4, 2, 0, 2, 2, 2, 2, 2, 2, 1 };
        PlayerManager player = DuelManager.GetIDOwner(iD);
        List<Card> cardsInHand = player.GetHandCards();

        int golemAtk = 2;
        int golemHp = 5;
        Element lastElement = Element.Other;
        List<QuantaObject> shardElementCounter = new List<QuantaObject>();
        foreach (Card handCard in cardsInHand)
        {
            if (handCard.name.Contains("Shard of"))
            {
                int? index = shardElementCounter.ContainsElement(handCard.element);
                if (index != null)
                {
                    shardElementCounter[(int)index].count++;
                }
                else
                {
                    shardElementCounter.Add(new QuantaObject(handCard.element, 1));
                }
                lastElement = handCard.element;
                golemAtk += atkModifier[(int)handCard.element];
                golemHp += hpModifier[(int)handCard.element];
            }
        }
        Card golem = CardDatabase.GetCardFromResources("Shard Golem", "Creature", true);
        if (lastElement == Element.Other)
        {
            player.PlayCardOnFieldLogic(golem);
            return;
        }
        QuantaObject lastQuantaObject = new QuantaObject(Element.Other, 1);
        foreach (QuantaObject item in shardElementCounter)
        {
            if (item.element == lastElement)
            {
                lastQuantaObject = item;
            }
        }
        player.PlayCardOnFieldLogic(CardDatabase.GetGolemAbility(lastQuantaObject, golem));

    }

    public bool IsValidTarget(ID iD)
    {
        return iD.Field.Equals(FieldEnum.Creature) && iD.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent);
    }
}