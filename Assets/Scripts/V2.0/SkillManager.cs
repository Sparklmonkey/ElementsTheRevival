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
        ActionManager.AddAbilityActivatedAction(owner.isPlayer, idCard, owner.playerID);
        ability.Owner = owner;
        ability.Activate(idCard);
    }

    public bool HasEnoughTargets(PlayerManager owner, IDCardPair card)
    {
        var ability = card.card.skill.GetSkillScript<AbilityEffect>();
        ability.Owner = owner;
        ability.Origin = card;
        var enemy = DuelManager.GetNotIDOwner(owner.playerID.id);
        var targets = ability.GetPossibleTargets(enemy);
        if (ability.NeedsTarget())
        {
            return targets.Count > 0;
        }
        return true;
    }

    public void SetupTargetHighlights(PlayerManager owner, IDCardPair card)
    {
        var ability = card.card.skill.GetSkillScript<AbilityEffect>();
        ability.Owner = owner;
        ability.Origin = card;
        var enemy = DuelManager.GetNotIDOwner(owner.playerID.id);
        DuelManager.SetupHighlights(ability.GetPossibleTargets(enemy));
    }

    public void SkillRoutineWithTarget(PlayerManager owner, IDCardPair target)
    {
        var ability = BattleVars.shared.abilityOrigin.card.skill.GetSkillScript<AbilityEffect>();
        ActionManager.AddAbilityActivatedAction(owner.isPlayer, BattleVars.shared.abilityOrigin, target);
        ability.Owner = owner;
        ability.Origin = BattleVars.shared.abilityOrigin;

        ability.Activate(target);
    }

    public IDCardPair GetRandomTarget(PlayerManager owner, IDCardPair iDCard)
    {
        var ability = iDCard.card.skill.GetSkillScript<AbilityEffect>();
        ability.Owner = owner;
        ability.Origin = iDCard;
        var targets = ability.GetPossibleTargets(DuelManager.GetIDOwner(owner.playerID.id));
        if(targets.Count == 0) { return null; }
        return ability.SelectRandomTarget(targets);
    }

}