public class SkillManager
{
    private static readonly SkillManager instance = new();

    static SkillManager()
    {
    }

    private SkillManager()
    {
    }

    public static SkillManager Instance
    {
        get
        {
            return instance;
        }
    }

    public bool ShouldAskForTarget(IDCardPair idCard)
    {
        var ability = idCard.card.skill.GetSkillScript<AbilityEffect>();
        return ability.NeedsTarget();
    }

    public void SkillRoutineNoTarget(PlayerManager owner, IDCardPair idCard)
    {
        var ability = idCard.card.skill.GetSkillScript<AbilityEffect>();
        ability.Owner = owner;
        ability.Activate(idCard);
    }

    public void SetupTargetHighlights(PlayerManager owner, IDCardPair card)
    {
        var ability = card.card.skill.GetSkillScript<AbilityEffect>();
        ability.Owner = owner;
        var enemy = DuelManager.GetNotIDOwner(owner.playerID.id);
        DuelManager.SetupHighlights(ability.GetPossibleTargets(enemy));
    }

    public void SkillRoutineWithTarget(PlayerManager owner, IDCardPair iDCard)
    {
        var ability = BattleVars.shared.abilityOrigin.card.skill.GetSkillScript<AbilityEffect>();
        ability.Owner = owner;

        ability.Activate(iDCard);
    }

    public IDCardPair GetRandomTarget(PlayerManager owner, IDCardPair iDCard)
    {
        var ability = iDCard.card.skill.GetSkillScript<AbilityEffect>();
        ability.Owner = owner;

        return ability.SelectRandomTarget(ability.GetPossibleTargets(DuelManager.GetIDOwner(owner.playerID.id)));
    }

}