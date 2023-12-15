using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CreatureAbilities
{
    public IEnumerator ActivateAllCreatureAbility(PlayerManager aiManager)
    {
        List<IDCardPair> cardList = new(aiManager.playerCreatureField.GetAllValidCardIds());

        foreach (var creature in cardList)
        {
            if (!aiManager.IsAbilityUsable(creature)) { continue; }
            if (SkillManager.Instance.ShouldAskForTarget(creature))
            {
                var target = SkillManager.Instance.GetRandomTarget(aiManager, creature);
                if (target == null) { continue; }

                BattleVars.Shared.AbilityOrigin = creature;
                SkillManager.Instance.SkillRoutineWithTarget(aiManager, target);
            }
            else
            {
                BattleVars.Shared.AbilityOrigin = creature;
                SkillManager.Instance.SkillRoutineNoTarget(aiManager, creature);
            }
            yield return null;
        }
    }

    public IEnumerator ActivatePegasus(PlayerManager aiManager)
    {
        List<IDCardPair> cardList = new(aiManager.playerCreatureField.GetAllValidCardIds());
        var pegasusList = cardList.GetIDCardPairsWithCardId(new() { "5lb", "7jr" });

        if (pegasusList.Count == 0) { yield break; }

        foreach (var pegasus in pegasusList)
        {
            if (!aiManager.IsAbilityUsable(pegasus)) { continue; }
            BattleVars.Shared.AbilityOrigin = pegasus;
            aiManager.ActivateAbility(pegasus);
        }
    }

    public IEnumerator ActivateRetroVirus(PlayerManager aiManager)
    {
        var possibleTargets = DuelManager.Instance.player.playerCreatureField.GetAllValidCardIds();
        if (possibleTargets.Count < 2) { yield break; }

        List<IDCardPair> cardList = new(aiManager.playerCreatureField.GetAllValidCardIds());
        var virusList = cardList.GetIDCardPairsWithCardId(new() { "52i", "712" });

        if (virusList.Count == 0) { yield break; }

        foreach (var virus in virusList)
        {
            if (!aiManager.IsAbilityUsable(virus)) { continue; }
            BattleVars.Shared.AbilityOrigin = virus;
            var target = possibleTargets.Aggregate((i1, i2) => i1.card.DefNow >= i2.card.DefNow ? i1 : i2);
            aiManager.ActivateAbility(target);
        }

    }

    public IEnumerator ActivateGraboid(PlayerManager aiManager)
    {
        List<IDCardPair> cardList = new(aiManager.playerCreatureField.GetAllValidCardIds());
        var graboidList = cardList.GetIDCardPairsWithCardId(new() { "590", "77g" });

        if (graboidList.Count == 0) { yield break; }

        foreach (var graboid in graboidList)
        {
            if (!aiManager.IsAbilityUsable(graboid)) { continue; }
            BattleVars.Shared.AbilityOrigin = graboid;
            aiManager.ActivateAbility(graboid);
        }
    }

    public IEnumerator ActivateFleshRecluse(PlayerManager aiManager)
    {
        List<IDCardPair> cardList = new(aiManager.playerCreatureField.GetAllValidCardIds());
        var fleshSpider = cardList.GetIDCardPairsWithCardId(new() { "52j", "713" });

        if (fleshSpider.Count == 0) { yield break; }

        foreach (var spider in fleshSpider)
        {
            if (!aiManager.IsAbilityUsable(spider)) { continue; }
            var target = SkillManager.Instance.GetRandomTarget(aiManager, spider);
            if (target == null) { continue; }
            BattleVars.Shared.AbilityOrigin = spider;
            aiManager.ActivateAbility(target);
        }
    }

    public IEnumerator ActivateBloodsucker(PlayerManager aiManager)
    {
        var possibleTargets = DuelManager.Instance.player.playerCreatureField.GetAllValidCardIds();
        if (possibleTargets.Count < 1) { yield break; }

        List<IDCardPair> cardList = new(aiManager.playerCreatureField.GetAllValidCardIds());
        var bloodsuckerList = cardList.GetIDCardPairsWithCardId(new() { "5un", "7t7" });

        if (bloodsuckerList.Count == 0) { yield break; }

        foreach (var bloodSucker in bloodsuckerList)
        {
            if (!aiManager.IsAbilityUsable(bloodSucker)) { continue; }
            BattleVars.Shared.AbilityOrigin = bloodSucker;
            var target = possibleTargets.Aggregate((i1, i2) => i1.card.DefNow >= i2.card.DefNow ? i1 : i2);
            aiManager.ActivateAbility(target);
        }
    }

    public IEnumerator ActivateCrusaders(PlayerManager aiManager)
    {
        List<IDCardPair> cardList = new(aiManager.playerCreatureField.GetAllValidCardIds());
        var creatureList = cardList.GetIDCardPairsWithCardId(new() { "5un", "5ll" });

        if (creatureList.Count == 0) { yield break; }

        foreach (var creature in creatureList)
        {
            if (!aiManager.IsAbilityUsable(creature)) { continue; }

        }

        for (var i = 0; i < creatureList.Count; i++)
        {
            if (!aiManager.IsAbilityUsable(creatureList[i])) { continue; }
            if (creatureList[i].card.skill == "endow")
            {
                if (aiManager.playerPassiveManager.GetWeapon() == null) { continue; }
                if (aiManager.playerPassiveManager.GetWeapon().card.cardName == "Weapon") { continue; }

                BattleVars.Shared.AbilityOrigin = creatureList[i];
                var target = aiManager.playerPassiveManager.GetWeapon();
                aiManager.ActivateAbility(target);
            }
            else
            {
                BattleVars.Shared.AbilityOrigin = creatureList[i];
                var target = SkillManager.Instance.GetRandomTarget(aiManager, creatureList[i]);
                if (target == null)
                {
                    BattleVars.Shared.AbilityOrigin = null;
                    continue;
                }
                aiManager.ActivateAbility(target);
            }

        }

    }
}
