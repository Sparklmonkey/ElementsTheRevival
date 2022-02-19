using System.Collections.Generic;
using UnityEngine;

public class ActiveADeadAlive : IActivateAbility
{
    public int AbilityCost => 1;

    public Element AbilityElement => Element.Entropy;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        void Effect() => DuelManager.GetIDOwner(target).RemoveCardFromFieldLogic(target);
        DuelManager.GetIDOwner(target).PlayCardOnFieldLogic(CardDatabase.GetCardFromResources("Schrodinger's Cat", "Creature", false));
        Game_AnimationManager.PlayAnimation("DeadAndAlive", Battlefield_ObjectIDManager.shared.GetObjectFromID(target), Effect);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}

public class ActiveADejaVu : IActivateAbility
{
    public int AbilityCost => 1;

    public Element AbilityElement => Element.Time;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        Card creatureToDuplicate = DuelManager.GetIDOwner(target).GetCard(target);
        creatureToDuplicate.activeAbility = null;
        creatureToDuplicate.description = "";
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target);
        Card duplicate = CardDatabase.GetCardFromResources(creatureToDuplicate.name, creatureToDuplicate.type.FastCardTypeString(), !creatureToDuplicate.isUpgradable);
        duplicate.activeAbility = null;
        duplicate.description = "";
        DuelManager.GetIDOwner(target).PlayCardOnFieldLogic(duplicate);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}

public class ActiveADevour : IActivateAbility
{
    public int AbilityCost => 1;

    public Element AbilityElement => Element.Gravity;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(BattleVars.shared.originId).ModifyCreatureLogic(BattleVars.shared.originId, null, 0, 1, 1);
        DuelManager.GetIDOwner(target).RemoveCardFromFieldLogic(target);
    }

    public bool IsValidTarget(ID target)
    {
        if (target.Field.Equals(FieldEnum.Creature) && target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player))
        {
            Card targetCreature = DuelManager.GetIDOwner(target).GetCard(target);
            Card userCreature = DuelManager.GetIDOwner(BattleVars.shared.originId).GetCard(BattleVars.shared.originId);
            return userCreature.hp >= targetCreature.hp;
        }
        return false;
    }
}

public class ActiveAEvolve : IActivateAbility
{
    public int AbilityCost => 1;

    public Element AbilityElement => Element.Time;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        Card shrieker = CardDatabase.GetCardFromResources("Shrieker", "Creature", false);

        DuelManager.GetIDOwner(target).PlayCardOnFieldLogic(shrieker);
        DuelManager.GetIDOwner(target).RemoveCardFromFieldLogic(target);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}

public class ActiveAEvolvePlus : IActivateAbility
{
    public int AbilityCost => 1;

    public Element AbilityElement => Element.Time;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        Card shrieker = CardDatabase.GetCardFromResources("Elite Shrieker", "Creature", true);

        DuelManager.GetIDOwner(target).PlayCardOnFieldLogic(shrieker);
        DuelManager.GetIDOwner(target).RemoveCardFromFieldLogic(target);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}

public class ActiveAHeal : IActivateAbility
{
    public int AbilityCost => 1;

    public Element AbilityElement => Element.Light;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, null, 0, 0, 0, null, false, -1 * 5);
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature) && target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent);
    }
}

public class ActiveAImprovedMutation : IActivateAbility
{
    public int AbilityCost => 1;

    public Element AbilityElement => Element.Life;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetNotIDOwner(target).RemoveCardFromFieldLogic(target);
        Card mutant = CardDatabase.GetMutant();
        DuelManager.GetIDOwner(target).PlayCardOnFieldLogic(mutant);
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature);
    }
}

public class ActiveACongeal : IActivateAbility
{
    public int AbilityCost => 2;

    public Element AbilityElement => Element.Water;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, CounterEnum.Freeze, 4);
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature) && target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player);
    }
}

public class ActiveAFreeze : IActivateAbility
{
    public int AbilityCost => 2;

    public Element AbilityElement => Element.Water;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, CounterEnum.Freeze, 3);
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature) && target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player);
    }
}

public class ActiveAInfect : IActivateAbility
{
    public int AbilityCost => 0;

    public Element AbilityElement => Element.Other;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, CounterEnum.Poison, 1);
        DuelManager.GetIDOwner(BattleVars.shared.originId).RemoveCardFromFieldLogic(BattleVars.shared.originId);
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature) && target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player);
    }
}


public class ActiveALobotomizer : IActivateAbility
{
    public int AbilityCost => 2;

    public Element AbilityElement => Element.Aether;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        Card targetCreature = DuelManager.GetIDOwner(target).GetCard(target);
        targetCreature.activeAbility = null;
        targetCreature.description = "";
    }

    public bool IsValidTarget(ID target)
    {
        if (target.Field.Equals(FieldEnum.Creature) && target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player))
        {
            Card targetCreature = DuelManager.GetIDOwner(target).GetCard(target);
            return targetCreature.activeAbility != null;
        }
        return false;
    }
}

public class ActiveALobotomizerPlus : IActivateAbility
{
    public int AbilityCost => 1;

    public Element AbilityElement => Element.Aether;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        Card targetCreature = DuelManager.GetIDOwner(target).GetCard(target);
        targetCreature.activeAbility = null;
        targetCreature.description = "";
    }

    public bool IsValidTarget(ID target)
    {
        if (target.Field.Equals(FieldEnum.Creature) && target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player))
        {
            Card targetCreature = DuelManager.GetIDOwner(target).GetCard(target);
            return targetCreature.activeAbility != null;
        }
        return false;
    }
}

public class ActiveAMutation : IActivateAbility
{
    public int AbilityCost => 1;

    public Element AbilityElement => Element.Life;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetNotIDOwner(target).RemoveCardFromFieldLogic(target);
        switch (GetMutationResult())
        {
            case MutationEnum.Kill:
                break;
            case MutationEnum.Mutate:
                Card mutant = CardDatabase.GetMutant();
                 DuelManager.GetIDOwner(target).PlayCardOnFieldLogic(mutant);
                break;
            case MutationEnum.Abomination:
                Card abomination = CardDatabase.GetCardFromResources("Abomination", "Creature", false);
                DuelManager.GetIDOwner(target).PlayCardOnFieldLogic(abomination);
                break;
            default:
                break;
        }
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature);
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

public class ActiveAParadox : IActivateAbility
{
    public int AbilityCost => 2;

    public Element AbilityElement => Element.Entropy;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).RemoveCardFromFieldLogic(target);
    }

    public bool IsValidTarget(ID target)
    {
        if (target.Field.Equals(FieldEnum.Creature) && target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player))
        {
            Card targetCreature = DuelManager.GetIDOwner(target).GetCard(target);
            return targetCreature.power > targetCreature.hp;
        }
        return false;
    }
}

public class ActiveAPlague : IActivateAbility
{
    public int AbilityCost => 0;

    public Element AbilityElement => Element.Other;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        DuelManager.SetupValidTargets(null, this);
        List<ID> creatures = DuelManager.GetAllValidTargets();
        foreach (ID creature in creatures)
        {
            DuelManager.ApplyCounter(CounterEnum.Poison, 1, creature);
        }
        DuelManager.GetIDOwner(target).RemoveCardFromFieldLogic(target);
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature) && target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player);
    }
}

public class ActiveAReverseTime : IActivateAbility
{
    public int AbilityCost => 3;

    public Element AbilityElement => Element.Time;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        PlayerManager targetOwner = DuelManager.GetIDOwner(target);
        Card creature = targetOwner.GetCard(target);
        if (creature.cardPassives.isMummy)
        {
            Card pharoah = CardDatabase.GetCardFromResources(creature.isUpgradable ? "Pharaoh" : "Elite Pharaoh", "Creature", !creature.isUpgradable);
            targetOwner.PlayCardOnFieldLogic(pharoah);
        }
        else if (creature.cardPassives.isUndead)
        {
            Card rndCreature = creature.isUpgradable ? CardDatabase.GetRandomCreature() : CardDatabase.GetRandomEliteCreature();
            targetOwner.PlayCardOnFieldLogic(rndCreature);
        }
        else
        {
            targetOwner.AddCardToDeck(creature);
        }
        targetOwner.RemoveCardFromFieldLogic(target);
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature);
    }
}

public class ActiveAShatter : IActivateAbility
{
    public int AbilityCost => 3;

    public Element AbilityElement => Element.Gravity;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).RemoveCardFromFieldLogic(target);
    }

    public bool IsValidTarget(ID target)
    {
        if (target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player))
        {
            return !target.Field.Equals(FieldEnum.Hand);
        }
        return false;
    }
}

public class ActiveADestroy : IActivateAbility
{
    public int AbilityCost => 3;

    public Element AbilityElement => Element.Entropy;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).RemoveCardFromFieldLogic(target);
    }

    public bool IsValidTarget(ID target)
    {
        if (target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player))
        {
            return target.Field.Equals(FieldEnum.Permanent);
        }
        return false;
    }
}
public class ActiveAShatterPlus : IActivateAbility
{
    public int AbilityCost => 2;

    public Element AbilityElement => Element.Gravity;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).RemoveCardFromFieldLogic(target);
    }

    public bool IsValidTarget(ID target)
    {
        if (target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player))
        {
            return !target.Field.Equals(FieldEnum.Hand);
        }
        return false;
    }
}

public class ActiveATsunami : IActivateAbility
{
    public int AbilityCost => 3;

    public Element AbilityElement => Element.Earth;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).RemoveCardFromFieldLogic(target, 3);
    }

    public bool IsValidTarget(ID target)
    {
        if (target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player))
        {
            return DuelManager.GetOpponentCard(target).type.Equals(CardType.Pillar);
        }
        return false;
    }
}

public class ActiveATsunamiPlus : IActivateAbility
{
    public int AbilityCost => 2;

    public Element AbilityElement => Element.Earth;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).RemoveCardFromFieldLogic(target, 3);
    }

    public bool IsValidTarget(ID target)
    {
        if (target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player))
        {
            return DuelManager.GetOpponentCard(target).type.Equals(CardType.Pillar);
        }
        return false;
    }
}

public class ActiveAAblaze : IActivateAbility
{
    public int AbilityCost => 1;

    public Element AbilityElement => Element.Fire;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, null, 0, 2, 0);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}

public class ActiveAWeb : IActivateAbility
{

    public int AbilityCost => 1;

    public Element AbilityElement => Element.Air;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        void Effect() => DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, null, 0, 0, 0, PassiveEnum.IsAirborne, false);
        Game_AnimationManager.PlayAnimation("Web", Battlefield_ObjectIDManager.shared.GetObjectFromID(target), Effect);
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature) && target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player);
    }
}

public class ActiveALycanthropy : IActivateAbility
{

    public int AbilityCost => 2;

    public Element AbilityElement => Element.Darkness;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, null, 0, 5, 5);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}

public class ActiveALycanthropyPlus : IActivateAbility
{

    public int AbilityCost => 1;

    public Element AbilityElement => Element.Darkness;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, null, 0, 5, 5);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}

public class ActiveAGravity : IActivateAbility
{

    public int AbilityCost => 1;

    public Element AbilityElement => Element.Gravity;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, null, 0, 0, 0, PassiveEnum.HasGravity, true);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}

public class ActiveARebirth : IActivateAbility
{

    public int AbilityCost => 1;

    public Element AbilityElement => Element.Fire;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).PlayCardOnFieldLogic(CardDatabase.GetCardFromResources("Phoenix", "Creature", false));
        DuelManager.GetIDOwner(target).RemoveCardFromFieldLogic(target);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}

public class ActiveAEliteRebirth : IActivateAbility
{

    public int AbilityCost => 1;

    public Element AbilityElement => Element.Fire;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).PlayCardOnFieldLogic(CardDatabase.GetCardFromResources("Minor Phoenix", "Creature", true));
        DuelManager.GetIDOwner(target).RemoveCardFromFieldLogic(target);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}

public class ActiveAPetrify : IActivateAbility
{

    public int AbilityCost => 3;

    public Element AbilityElement => Element.Earth;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, CounterEnum.Delay, 6, 0, 20, null, false);
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature);
    }
}

public class ActiveALiquidShadow : IActivateAbility
{

    public int AbilityCost => 2;

    public Element AbilityElement => Element.Darkness;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, CounterEnum.Poison, 1, 0, 0, PassiveEnum.IsVampire, true);
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature);
    }
}
public class ActiveASteal : IActivateAbility
{

    public int AbilityCost => 3;

    public Element AbilityElement => Element.Darkness;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        new SpellSteal().ActivateAbility(target);
    }

    public bool IsValidTarget(ID target)
    {
        return new SpellSteal().IsValidTarget(target);
    }
}
public class ActiveAReverse : IActivateAbility
{

    public int AbilityCost => 3;

    public Element AbilityElement => Element.Time;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        new SpellReverseTime().ActivateAbility(target);
    }

    public bool IsValidTarget(ID target)
    {
        return new SpellReverseTime().IsValidTarget(target);
    }
}
public class ActiveAAntimatter : IActivateAbility
{
    public int AbilityCost => 4;

    public Element AbilityElement => Element.Entropy;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, null, 0, 0, 0, PassiveEnum.IsAntimatter, true);
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature) && target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player);
    }
}

public class ActiveABurrow : IActivateAbility
{
    public int AbilityCost => 1;

    public Element AbilityElement => Element.Earth;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, null, 0, 0, 0, PassiveEnum.IsBurrowed, true);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}

public class ActiveASpawnFirefly : IActivateAbility
{
    public int AbilityCost => 2;

    public Element AbilityElement => Element.Life;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        Card fireFly = CardDatabase.GetCardFromResources("Firefly", "Creature",false);
        DuelManager.GetIDOwner(target).PlayCardOnFieldLogic(fireFly);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}

public class ActiveASpawnEliteFirefly : IActivateAbility
{
    public int AbilityCost => 2;

    public Element AbilityElement => Element.Life;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        Card fireFly = CardDatabase.GetCardFromResources("Elite Firefly", "Creature", true);
        DuelManager.GetIDOwner(target).PlayCardOnFieldLogic(fireFly);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}

public class ActiveAPhotosynthesis : IActivateAbility
{
    public int AbilityCost => 1;

    public Element AbilityElement => Element.Light;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).GenerateQuantaLogic(Element.Life, 2);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}

public class ActiveAImmortalityThree : IActivateAbility
{
    public int AbilityCost => 3;

    public Element AbilityElement => Element.Aether;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, null, 0, 0, 0, PassiveEnum.IsMaterial, true);
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature) && target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent);
    }
}
public class ActiveAImmortality : IActivateAbility
{
    public int AbilityCost => 2;

    public Element AbilityElement => Element.Aether;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, null, 0, 0, 0, PassiveEnum.IsMaterial, true);
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature) && target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent);
    }
}

public class ActiveAImmortalityPlus : IActivateAbility
{
    public int AbilityCost => 1;

    public Element AbilityElement => Element.Aether;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, null, 0, 0, 0, PassiveEnum.IsMaterial, true);
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature) && target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent);
    }
}

public class ActiveASpawnUnstableGas : IActivateAbility
{
    public int AbilityCost => 3;

    public Element AbilityElement => Element.Air;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).PlayCardOnFieldLogic(CardDatabase.GetCardFromResources("Unstable Gas", "Artifact", false));
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}

public class ActiveABlackHole : IActivateAbility
{
    public int AbilityCost => 3;

    public Element AbilityElement => Element.Gravity;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        PlayerManager victim = DuelManager.GetNotIDOwner(target);
        int hpToRestore = 0;

        for (int i = 0; i < 12; i++)
        {
            if (victim.HasSufficientQuanta((Element)i, 3))
            {
                victim.SpendQuantaLogic((Element)i, 3);
                hpToRestore++;
            }
        }
        DuelManager.GetIDOwner(target).ModifyHealthLogic(hpToRestore, false, false);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}

public class ActiveAAflatoxin : IActivateAbility
{
    public int AbilityCost => 1;

    public Element AbilityElement => Element.Death;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, CounterEnum.Poison, 2, 0, 0, PassiveEnum.HasAflatoxin, true);
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature);
    }
}

public class ActiveAGrowth : IActivateAbility
{
    public int AbilityCost => 1;

    public Element AbilityElement => Element.Water;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, null, 0, 2, 2);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}

public class ActiveAPoison : IActivateAbility
{
    public int AbilityCost => 1;

    public Element AbilityElement => Element.Death;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetNotIDOwner(target).ApplyPlayerCounterLogic(CounterEnum.Poison, 1);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}

public class ActiveADive : IActivateAbility
{
    public int AbilityCost => 1;

    public Element AbilityElement => Element.Air;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        void Effect() => DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, null, 0, 0, 0, PassiveEnum.IsDiving, true);
        Game_AnimationManager.PlayAnimation("Dive", Battlefield_ObjectIDManager.shared.GetObjectFromID(target), Effect);
        
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}

public class ActiveADiveTwo : IActivateAbility
{
    public int AbilityCost => 2;

    public Element AbilityElement => Element.Air;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        void Effect() => DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, null, 0, 0, 0, PassiveEnum.IsDiving, true);
        Game_AnimationManager.PlayAnimation("Dive", Battlefield_ObjectIDManager.shared.GetObjectFromID(target), Effect);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}

public class ActiveAHasteLight : IActivateAbility
{
    public int AbilityCost => 1;

    public Element AbilityElement => Element.Light;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).DrawCardFromDeckLogic();
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}

public class ActiveAHasteTime : IActivateAbility
{
    public int AbilityCost => 2;

    public Element AbilityElement => Element.Time;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).DrawCardFromDeckLogic();
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}

public class ActiveAHasteTimePlus : IActivateAbility
{
    public int AbilityCost => 1;

    public Element AbilityElement => Element.Time;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).DrawCardFromDeckLogic();
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}

public class ActiveAInfection : IActivateAbility
{
    public int AbilityCost => 1;

    public Element AbilityElement => Element.Death;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, CounterEnum.Poison, 1);
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature) && target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player);
    }
}

public class ActiveAHatch : IActivateAbility
{
    public int AbilityCost => 1;

    public Element AbilityElement => Element.Time;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        Card rndCreature = CardDatabase.GetRandomCreature();
        DuelManager.GetIDOwner(target).PlayCardOnFieldLogic(rndCreature);
        DuelManager.GetIDOwner(target).RemoveCardFromFieldLogic(target);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}

public class ActiveAHatchPlus : IActivateAbility
{
    public int AbilityCost => 1;

    public Element AbilityElement => Element.Time;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        Card rndCreature = CardDatabase.GetRandomEliteCreature();
        DuelManager.GetIDOwner(target).PlayCardOnFieldLogic(rndCreature);
        DuelManager.GetIDOwner(target).RemoveCardFromFieldLogic(target);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}

public class ActiveASniper : IActivateAbility
{
    public int AbilityCost => 2;

    public Element AbilityElement => Element.Air;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        void Effect() => DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, null, 0, 0, 0, null, false, 3);
        Game_AnimationManager.PlayAnimation("Sniper", Battlefield_ObjectIDManager.shared.GetObjectFromID(target), Effect);
        
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature) && target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player);
    }
}

public class ActiveAEndowThree : IActivateAbility
{

    public int AbilityCost => 3;

    public Element AbilityElement => Element.Light;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        Card weapon = DuelManager.GetIDOwner(target).GetCard(new ID(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent, FieldEnum.Passive, 1));
        if(weapon != null)
        {
            Card creature = DuelManager.GetIDOwner(target).GetCard(target);
            creature.activeAbility = weapon.activeAbility;
            DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, null, 0, weapon.power, 2);
        }
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}
public class ActiveAEndowTwo : IActivateAbility
{

    public int AbilityCost => 2;

    public Element AbilityElement => Element.Light;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        Card weapon = DuelManager.GetIDOwner(target).GetCard(new ID(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent, FieldEnum.Passive, 1));
        if (weapon != null)
        {
            Card creature = DuelManager.GetIDOwner(target).GetCard(target);
            creature.activeAbility = weapon.activeAbility;
            DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, null, 0, weapon.power, 2);
        }
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}
public class ActiveAStoneForm : IActivateAbility
{

    public int AbilityCost => 1;

    public Element AbilityElement => Element.Earth;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, null, 0, 0, 20);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}
public class ActiveAPrecognition : IActivateAbility
{

    public int AbilityCost => 2;

    public Element AbilityElement => Element.Time;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).DrawCardFromDeckLogic();
        DuelManager.RevealOpponentsHand();
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}
public class ActiveASpawnScarab : IActivateAbility
{

    public int AbilityCost => 2;

    public Element AbilityElement => Element.Time;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).PlayCardOnFieldLogic(CardDatabase.GetCardFromResources("Scarab", "Creature", false));
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}
public class ActiveASpawnEliteScarab : IActivateAbility
{

    public int AbilityCost => 2;

    public Element AbilityElement => Element.Time;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).PlayCardOnFieldLogic(CardDatabase.GetCardFromResources("Elite Scarab", "Creature", true));
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}
public class ActiveAEliteDeadAlive : IActivateAbility
{

    public int AbilityCost => 1;

    public Element AbilityElement => Element.Entropy;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        void Effect() => DuelManager.GetIDOwner(target).RemoveCardFromFieldLogic(target);
        DuelManager.GetIDOwner(target).PlayCardOnFieldLogic(CardDatabase.GetCardFromResources("Elite Schr?dinger's Cat", "Creature", true));
        Game_AnimationManager.PlayAnimation("DeadAndAlive", Battlefield_ObjectIDManager.shared.GetObjectFromID(target), Effect);

    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}
public class ActiveADivineShield : IActivateAbility
{

    public int AbilityCost => 1;

    public Element AbilityElement => Element.Light;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, CounterEnum.Invisible, 2);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}
public class ActiveAAccretion : IActivateAbility
{

    public int AbilityCost => 0;

    public Element AbilityElement => Element.Gravity;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        Card shard = DuelManager.GetIDOwner(target).GetCard(target);
        if(shard.hp + 15 >= 45)
        {
            new ActiveABlackHole().ActivateAbility(null);
            DuelManager.GetIDOwner(target).RemoveCardFromFieldLogic(target);
            return;
        }
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, null, 0, 0, 15);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}
public class ActiveASteam : IActivateAbility
{

    public int AbilityCost => 2;

    public Element AbilityElement => Element.Fire;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, CounterEnum.Charge, 5, 5);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}
public class ActiveARageFour : IActivateAbility
{

    public int AbilityCost => 4;

    public Element AbilityElement => Element.Fire;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, null, 0, 6, -6);
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature);
    }
}
public class ActiveARageThree : IActivateAbility
{

    public int AbilityCost => 3;

    public Element AbilityElement => Element.Fire;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, null, 0, 5, -5);
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature);
    }
}
public class ActiveAAdrenaline : IActivateAbility
{

    public int AbilityCost => 2;

    public Element AbilityElement => Element.Life;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, null, 0, 0, 0, PassiveEnum.hasAdrenaline, true);
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature);
    }
}
public class ActiveAMitosis : IActivateAbility
{

    public int AbilityCost => 0;

    public Element AbilityElement => Element.Life;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        Card parent = DuelManager.GetIDOwner(target).GetCard(target);
        Card daughterCard = CardDatabase.GetCardFromResources(parent.name, parent.type.FastCardTypeString(), !parent.isUpgradable);
        DuelManager.GetIDOwner(target).PlayCardOnFieldLogic(daughterCard);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}
public class ActiveAGuard : IActivateAbility
{

    public int AbilityCost => 1;

    public Element AbilityElement => Element.Earth;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        int damage = DuelManager.GetIDOwner(target).GetCard(target).power;
        DuelManager.GetIDOwner(target).ModifyCreatureLogic(target, CounterEnum.Delay, 1, 0, 0, null, false, damage);
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature);
    }
}
public class ActiveALuciferinThree : IActivateAbility
{

    public int AbilityCost => 3;

    public Element AbilityElement => Element.Light;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        DuelManager.SetupValidTargets(null, this);
        foreach (ID id in DuelManager.GetAllValidTargets())
        {
            Card creature = DuelManager.GetIDOwner(target).GetCard(id);
            if (creature.activeAbility == null)
            {
                creature.endTurnAbility = new EndTurnBioluminescence();
                creature.description = "Bioluminescence : \n Each turn <sprite=3> is generated";
            }
        }
        DuelManager.GetIDOwner(target).ModifyHealthLogic(10, false, false);
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature) && target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent);
    }
}
public class ActiveAPsionicWave : IActivateAbility
{

    public int AbilityCost => 3;

    public Element AbilityElement => Element.Light;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        Card creature = DuelManager.GetNotIDOwner(target).GetCard(target);
        creature.activeAbility = null;
        creature.description = "";
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature) && target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Opponent : OwnerEnum.Player);
    }
}
public class ActiveANymphTear : IActivateAbility
{

    public int AbilityCost => 3;

    public Element AbilityElement => Element.Water;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        Card targetCard = DuelManager.GetIDOwner(target).GetCard(target);
        Card creature = targetCard.isUpgradable ? CardDatabase.GetRandomRegularNymph(targetCard.element) : CardDatabase.GetRandomEliteNymph(targetCard.element);
        DuelManager.GetIDOwner(target).RemoveCardFromFieldLogic(target);
        DuelManager.GetIDOwner(target).PlayCardOnFieldLogic(creature);
    }

    public bool IsValidTarget(ID target)
    {
        if(target.Field.Equals(FieldEnum.Permanent) && target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent))
        {
            Card permanent = DuelManager.GetIDOwner(target).GetCard(target);
            return permanent.type.Equals(CardType.Pillar);
        }
        return false;
    }
}

public class ActiveANymphTearFour : IActivateAbility
{

    public int AbilityCost => 4;

    public Element AbilityElement => Element.Water;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        Card targetCard = DuelManager.GetIDOwner(target).GetCard(target);
        Card creature = targetCard.isUpgradable ? CardDatabase.GetRandomRegularNymph(targetCard.element) : CardDatabase.GetRandomEliteNymph(targetCard.element);
        DuelManager.GetIDOwner(target).RemoveCardFromFieldLogic(target);
        DuelManager.GetIDOwner(target).PlayCardOnFieldLogic(creature);
    }

    public bool IsValidTarget(ID target)
    {
        if (target.Field.Equals(FieldEnum.Permanent) && target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent))
        {
            Card permanent = DuelManager.GetIDOwner(target).GetCard(target);
            return permanent.type.Equals(CardType.Pillar);
        }
        return false;
    }
}
public class ActiveALuciferinFour : IActivateAbility
{

    public int AbilityCost => 4;

    public Element AbilityElement => Element.Light;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        DuelManager.SetupValidTargets(null, this);
        foreach (ID id in DuelManager.GetAllValidTargets())
        {
            Card creature = DuelManager.GetIDOwner(target).GetCard(id);
            if (creature.activeAbility == null)
            {
                creature.endTurnAbility = new EndTurnBioluminescence();
                creature.description = "Bioluminescence : \n Each turn <sprite=3> is generated";
            }
        }
        DuelManager.GetIDOwner(target).ModifyHealthLogic(10, false, false);
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature) && target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent);
    }
}
public class ActiveACatapult : IActivateAbility
{

    public int AbilityCost => 2;

    public Element AbilityElement => Element.Gravity;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        Card creature = DuelManager.GetIDOwner(target).GetCard(target);
        int damage = 100 * creature.hp / (100 + creature.hp);
        if(creature.cardCounters.freeze > 0)
        {
            damage += (int)(damage * 0.5f);
        }
        DuelManager.GetIDOwner(target).RemoveCardFromFieldLogic(target);
        DuelManager.GetNotIDOwner(target).ModifyHealthLogic(damage, true, false);
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature) && target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent);
    }
}
public class ActiveATrebuchet : IActivateAbility
{

    public int AbilityCost => 1;

    public Element AbilityElement => Element.Gravity;

    public bool ShouldSelectTarget => true;

    public void ActivateAbility(ID target)
    {
        Card creature = DuelManager.GetIDOwner(target).GetCard(target);
        int damage = 100 * creature.hp / (100 + creature.hp);
        if (creature.cardCounters.freeze > 0)
        {
            damage += (int)(damage * 0.5f);
        }
        DuelManager.GetNotIDOwner(target).ModifyHealthLogic(damage, true, false);
        DuelManager.GetIDOwner(target).RemoveCardFromFieldLogic(target);
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature) && target.Owner.Equals(BattleVars.shared.isPlayerTurn ? OwnerEnum.Player : OwnerEnum.Opponent);
    }
}
public class ActiveADuality : IActivateAbility
{

    public int AbilityCost => 2;

    public Element AbilityElement => Element.Aether;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        Card cardToAdd = DuelManager.GetNotIDOwner(target).deckManager.GetTopCard();
        Card cardCopy = CardDatabase.GetCardFromResources(cardToAdd.name, cardToAdd.type.FastCardTypeString(), !cardToAdd.isUpgradable);
        DuelManager.GetIDOwner(target).playerHand.AddCardToHand(cardCopy);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}
public class ActiveAPatience : IActivateAbility
{

    public int AbilityCost => 0;

    public Element AbilityElement => Element.Water;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).RemoveCardFromFieldLogic(target);
    }

    public bool IsValidTarget(ID target)
    {
        return false;
    }
}
public class ActiveAUnstableGas : IActivateAbility
{

    public int AbilityCost => 1;

    public Element AbilityElement => Element.Fire;

    public bool ShouldSelectTarget => false;

    public void ActivateAbility(ID target)
    {
        DuelManager.GetIDOwner(target).RemoveCardFromFieldLogic(target);
        DuelManager.GetNotIDOwner(target).ModifyHealthLogic(20, true, false);

        DuelManager.SetupValidTargets(null, this);
        foreach (ID id in DuelManager.GetAllValidTargets())
        {
            DuelManager.GetNotIDOwner(target).ModifyCreatureLogic(id, null, 0, 0, 0, null, false, 1);
        }
    }

    public bool IsValidTarget(ID target)
    {
        return target.Field.Equals(FieldEnum.Creature);
    }
}