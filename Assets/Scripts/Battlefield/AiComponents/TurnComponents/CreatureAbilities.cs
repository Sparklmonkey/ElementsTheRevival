using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreatureAbilities
{
    public IEnumerator ActivateAllCreatureAbility(PlayerManager aiManager)
    {
        List<IDCardPair> cardList = new(aiManager.playerCreatureField.GetAllValidCardIds());

        foreach (var creature in cardList)
        {
            if (!aiManager.IsAbilityUsable(creature.card)) { continue; }
            if (SkillManager.Instance.ShouldAskForTarget(creature))
            {
                var target = SkillManager.Instance.GetRandomTarget(aiManager, creature);
                if(target == null) { continue; }

                BattleVars.shared.abilityOrigin = creature;
                SkillManager.Instance.SkillRoutineWithTarget(aiManager, target);
            }
            else
            {
                BattleVars.shared.abilityOrigin = creature;
                SkillManager.Instance.SkillRoutineNoTarget(aiManager, creature);
            }
            yield return null;
        }
    }

    public IEnumerator ActivatePegasus(PlayerManager aiManager)
    {
        List<IDCardPair> cardList = new(aiManager.playerCreatureField.GetAllValidCardIds());
        List<IDCardPair> pegasusList = cardList.GetIDCardPairsWithCardId(new() { "5lb", "7jr" });

        if (pegasusList.Count == 0) { yield break; }

        foreach (var pegasus in pegasusList)
        {
            if(!aiManager.IsAbilityUsable(pegasus.card)) { continue; }
            BattleVars.shared.abilityOrigin = pegasus;
            aiManager.ActivateAbility(pegasus);
        }
    }

    public IEnumerator ActivateRetroVirus(PlayerManager aiManager)
    {
        var possibleTargets = DuelManager.GetOtherPlayer().playerCreatureField.GetAllValidCardIds();
        if (possibleTargets.Count < 2) { yield break; }

        List<IDCardPair> cardList = new(aiManager.playerCreatureField.GetAllValidCardIds());
        List<IDCardPair> virusList = cardList.GetIDCardPairsWithCardId(new() { "52i", "712" });

        if (virusList.Count == 0) { yield break; }

        foreach (var virus in virusList)
        {
            if (!aiManager.IsAbilityUsable(virus.card)) { continue; }
            BattleVars.shared.abilityOrigin = virus;
            var target = possibleTargets.Aggregate((i1, i2) => i1.card.DefNow > i2.card.DefNow ? i1 : i2);
            aiManager.ActivateAbility(target);
        }

    }

    public IEnumerator ActivateGraboid(PlayerManager aiManager)
    {
        List<IDCardPair> cardList = new(aiManager.playerCreatureField.GetAllValidCardIds());
        List<IDCardPair> graboidList = cardList.GetIDCardPairsWithCardId(new() { "590", "77g" });

        if (graboidList.Count == 0) { yield break; }

        foreach (var graboid in graboidList)
        {
            if (!aiManager.IsAbilityUsable(graboid.card)) { continue; }
            BattleVars.shared.abilityOrigin = graboid;
            aiManager.ActivateAbility(graboid);
        }
    }

    public IEnumerator ActivateFleshRecluse(PlayerManager aiManager)
    {
        List<IDCardPair> cardList = new(aiManager.playerCreatureField.GetAllValidCardIds());
        List<IDCardPair> fleshSpider = cardList.GetIDCardPairsWithCardId(new() { "52j", "713" });

        if (fleshSpider.Count == 0) { yield break; }

        foreach (var spider in fleshSpider)
        {
            if (!aiManager.IsAbilityUsable(spider.card)) { continue; }
            var target = SkillManager.Instance.GetRandomTarget(aiManager, spider);
            if(target == null) { continue; }
            BattleVars.shared.abilityOrigin = spider;
            aiManager.ActivateAbility(target);
        }
    }

    public IEnumerator ActivateBloodsucker(PlayerManager aiManager)
    {
        var possibleTargets = DuelManager.GetOtherPlayer().playerCreatureField.GetAllValidCardIds();
        if (possibleTargets.Count < 1) { yield break; }

        List<IDCardPair> cardList = new(aiManager.playerCreatureField.GetAllValidCardIds());
        List<IDCardPair> bloodsuckerList = cardList.GetIDCardPairsWithCardId(new() { "5un", "7t7" });

        if (bloodsuckerList.Count == 0) { yield break; }

        foreach (var bloodSucker in bloodsuckerList)
        {
            if (!aiManager.IsAbilityUsable(bloodSucker.card)) { continue; }
            BattleVars.shared.abilityOrigin = bloodSucker;
            var target = possibleTargets.Aggregate((i1, i2) => i1.card.DefNow > i2.card.DefNow ? i1 : i2);
            aiManager.ActivateAbility(target);
        }
    }

    public IEnumerator ActivateCrusaders(PlayerManager aiManager)
    {
        List<IDCardPair> cardList = new(aiManager.playerCreatureField.GetAllValidCardIds());
        List<IDCardPair> creatureList = cardList.GetIDCardPairsWithCardId(new() { "5un", "5ll" });

        if (creatureList.Count == 0) { yield break; }

        foreach (var creature in creatureList)
        {
            if (!aiManager.IsAbilityUsable(creature.card)) { continue; }

        }

        for (int i = 0; i < creatureList.Count; i++)
        {
            if(!aiManager.IsAbilityUsable(creatureList[i].card)) { continue; }
            if (creatureList[i].card.skill == "endow")
            {
                if (aiManager.playerPassiveManager.GetWeapon() == null) { continue; }
                if (aiManager.playerPassiveManager.GetWeapon().card.cardName == "Weapon") { continue; }

                BattleVars.shared.abilityOrigin = creatureList[i];
                var target = aiManager.playerPassiveManager.GetWeapon();
                aiManager.ActivateAbility(target);
            }
            else
            {
                BattleVars.shared.abilityOrigin = creatureList[i];
                var target = SkillManager.Instance.GetRandomTarget(aiManager, creatureList[i]);
                if (target == null)
                {
                    BattleVars.shared.abilityOrigin = null;
                    continue;
                }
                aiManager.ActivateAbility(target);
            }

        }

    }
}
