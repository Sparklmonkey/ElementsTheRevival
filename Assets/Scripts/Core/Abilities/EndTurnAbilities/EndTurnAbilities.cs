
using System.Collections.Generic;
using UnityEngine;

public class EndTurnAirGenerate : IEndTurnAbility
{
    public void ActivateAbility(ID owner)
    {
        void Effect() => DuelManager.GetIDOwner(owner).GenerateQuantaLogic(Element.Air, 1);
        Game_AnimationManager.PlayAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(owner), Effect, Element.Air);
    }
}

public class EndTurnBioluminescence : IEndTurnAbility
{
    public void ActivateAbility(ID owner)
    {
        void Effect() => DuelManager.GetIDOwner(owner).GenerateQuantaLogic(Element.Light, 1);
        Game_AnimationManager.PlayAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(owner), Effect, Element.Light);
        
    }
}

public class EndTurnAccelerate : IEndTurnAbility
{
    public void ActivateAbility(ID owner)
    {
        DuelManager.GetIDOwner(owner).ModifyCreatureLogic(owner, null, 0, 2,-1);
    }
}
public class EndTurnOverdrive : IEndTurnAbility
{
    public void ActivateAbility(ID owner)
    {
        DuelManager.GetIDOwner(owner).ModifyCreatureLogic(owner, null, 0, 3, -1);
    }
}

public class EndTurnDevourer : IEndTurnAbility
{
    public void ActivateAbility(ID owner)
    {
        void Effect() => DuelManager.GetIDOwner(owner).GenerateQuantaLogic(Element.Darkness, 1);
        Game_AnimationManager.PlayAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(owner), Effect, Element.Darkness);
        if (DuelManager.GetNotIDOwner(owner).GetAllQuantaOfElement(Element.Other) == 0) { return; }
        DuelManager.GetNotIDOwner(owner).SpendQuantaLogic(Element.Other, 1);
    }
}

public class EndTurnUpdateSwarm : IEndTurnAbility
{
    public void ActivateAbility(ID owner)
    {
        int scarabCount = DuelManager.GetIDOwner(owner).GetScarabDiff();
        DuelManager.GetIDOwner(owner).ModifyCreatureLogic(owner, null, 0, scarabCount, scarabCount);
    }
}

public class EndTurnFlooding : IEndTurnAbility
{
    public void ActivateAbility(ID owner)
    {
        PlayerManager player = DuelManager.GetIDOwner(owner);
        if (player.GetAllQuantaOfElement(Element.Water) == 0) 
        {
            player.RemoveCardFromFieldLogic(owner);
            return;
        }
        player.SpendQuantaLogic(Element.Water, 1);
        
        player.ClearFloodedArea(new List<int> { 11, 12, 13, 14, 15, 16 });
    }
}

public class EndTurnBuffPatience : IEndTurnAbility
{
    public void ActivateAbility(ID owner)
    {
        bool isFlooded = DuelManager.floodCount > 0;
        PlayerManager player = DuelManager.GetIDOwner(owner);
        List<ID> allIds = player.GetAllIds();
        foreach (ID cardId in allIds)
        {
            if(!cardId.Field.Equals(FieldEnum.Creature)) { continue; }
            if (player.GetCard(cardId).element.Equals(Element.Water))
            {
                player.ModifyCreatureLogic(cardId, null, 0, isFlooded ? 4 : 2, isFlooded ? 4 : 2);
            }
            player.ModifyCreatureLogic(cardId, null, 0, 1, 0);
        }
    }
}


public class EndTurnEarthGenerate : IEndTurnAbility
{
    public void ActivateAbility(ID owner)
    {
        void Effect() => DuelManager.GetIDOwner(owner).GenerateQuantaLogic(Element.Earth, 1);
        Game_AnimationManager.PlayAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(owner), Effect, Element.Earth);
    }
}

public class EndTurnSingularity : IEndTurnAbility
{
    public void ActivateAbility(ID owner)
    {
        int singCase = Random.Range(0, 9);

        switch (singCase)
        {
            case 0:
                //AntiMatter
                DuelManager.GetIDOwner(owner).GetCard(owner).cardPassives.isAntimatter = true;
                break;
            case 1:
                //Quintessence 
                DuelManager.GetIDOwner(owner).GetCard(owner).cardPassives.isImmaterial = true;
                break;
            case 2:
                //Copy
                new SpellParallelUniverse().ActivateAbility(owner);
                break;
            case 3:
                //Vampire 
                DuelManager.GetIDOwner(owner).GetCard(owner).cardPassives.isVampire = true;
                break;
            case 4:
                new SpellChaosPower().ActivateAbility(owner);
                break;
            case 5:
                //Nova
                new SpellNova().ActivateAbility(owner);
                break;
            case 6:
                //Adrenaline
                DuelManager.GetIDOwner(owner).GetCard(owner).cardPassives.hasAdrenaline = true;
                break;
            default:
                //Immolation no Quanta
                DuelManager.GetIDOwner(owner).RemoveCardFromFieldLogic(owner);
                break;
        }
    }
}

public class EndTurnEmpathicBond : IEndTurnAbility
{
    public void ActivateAbility(ID owner)
    {
        List<ID> allIds = DuelManager.GetIDOwner(owner).GetAllIds();
        int creatureCount = 0;

        foreach (ID iD in allIds)
        {
            if (iD.Field.Equals(FieldEnum.Creature))
            {
                creatureCount++;
            }
        }
        DuelManager.GetIDOwner(owner).ModifyHealthLogic(creatureCount, false, false);
    }
}

public class EndTurnFireGenerate : IEndTurnAbility
{
    public void ActivateAbility(ID owner)
    {
        void Effect() => DuelManager.GetIDOwner(owner).GenerateQuantaLogic(Element.Fire, 1);
        Game_AnimationManager.PlayAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(owner), Effect, Element.Fire);
    }
}

public class EndTurnInfest : IEndTurnAbility
{
    public void ActivateAbility(ID owner)
    {
        DuelManager.GetIDOwner(owner).PlayCardOnFieldLogic(CardDatabase.GetCardFromResources("Malignant Cell", "Creature", true));
    }
}

public class EndTurnDamageVoid : IEndTurnAbility
{
    public void ActivateAbility(ID owner)
    {
        DuelManager.GetNotIDOwner(owner).ModifyMaxHealthLogic(DuelManager.GetOtherMarkElement().Equals(Element.Darkness) ? 3 : 2, false);
    }
}

public class EndTurnHealGratitude : IEndTurnAbility
{
    public void ActivateAbility(ID owner)
    {
        DuelManager.GetIDOwner(owner).ModifyHealthLogic(DuelManager.GetOtherMarkElement().Equals(Element.Life) ? 5 : 3, false, true);
    }
}

public class EndTurnHealFour : IEndTurnAbility
{
    public void ActivateAbility(ID owner)
    {
        DuelManager.GetIDOwner(owner).ModifyHealthLogic(4, false, true);
    }
}
