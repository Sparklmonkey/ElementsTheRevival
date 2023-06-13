using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager
{
    private static readonly SkillManager instance = new SkillManager();

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
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
        var ability = idCard.card.skill.GetScriptFromName<AbilityEffect>();
        return ability.NeedsTarget();
    }

    public void SkillRoutineNoTarget(PlayerManager owner, IDCardPair idCard)
    {
        var ability = idCard.card.skill.GetScriptFromName<AbilityEffect>();
        ability.Owner = owner;
        ability.Activate(idCard);
    }

    public void SetupTargetHighlights(PlayerManager owner, IDCardPair card)
    {
        var ability = card.card.skill.GetScriptFromName<AbilityEffect>();
        ability.Owner = owner;
        var enemy = DuelManager.GetNotIDOwner(owner.playerID.id);
        DuelManager.SetupHighlights(ability.GetPossibleTargets(enemy));
    }

    public void SkillRoutineWithTarget(IDCardPair iDCard)
    {
        var ability = BattleVars.shared.abilityOrigin.card.skill.GetScriptFromName<AbilityEffect>();
        PlayerManager originOwner = DuelManager.GetIDOwner(BattleVars.shared.abilityOrigin.id);
        ability.Owner = originOwner;

        ability.Activate(iDCard);
    }

}