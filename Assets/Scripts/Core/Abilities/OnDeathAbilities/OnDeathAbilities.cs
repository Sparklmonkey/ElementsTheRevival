public class OnDeathBoneShield : IOnDeathAbility
{
    public void ActivateAction(ID owner)
    {
        DuelManager.GetIDOwner(owner).ApplyPlayerCounterLogic(CounterEnum.Bone, 2);
    }
}


public class OnDeathScavenger : IOnDeathAbility
{
    public void ActivateAction(ID owner)
    {
        DuelManager.GetIDOwner(owner).ModifyCreatureLogic(owner, null, 0, 1, 1);
    }
}


public class OnDeathSoulCatcher : IOnDeathAbility
{
    public void ActivateAction(ID owner)
    {
        void Effect() => DuelManager.GetIDOwner(owner).GenerateQuantaLogic(Element.Death, 2);
        Game_AnimationManager.PlayAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(owner), Effect, Element.Death);
    }
}


public class OnDeathSoulCatcherPlus : IOnDeathAbility
{
    public void ActivateAction(ID owner)
    {
        void Effect() => DuelManager.GetIDOwner(owner).GenerateQuantaLogic(Element.Death, 3);
        Game_AnimationManager.PlayAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(owner), Effect, Element.Death);
    }
}

public class OnDeathSpawnEliteSkeleton : IOnDeathAbility
{
    public void ActivateAction(ID owner)
    {
        DuelManager.GetIDOwner(owner).PlayCardOnFieldLogic(CardDatabase.GetCardFromResources("Elite Skeleton", "Creature", true));
    }
}


public class OnDeathSpawnSkeleton : IOnDeathAbility
{
    public void ActivateAction(ID owner)
    {
        DuelManager.GetIDOwner(owner).PlayCardOnFieldLogic(CardDatabase.GetCardFromResources("Skeleton", "Creature", false));
    }
}