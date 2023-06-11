using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreatureAbilities
{
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

        for (int i = 0; i < crusaderCards.Count; i++)
        {
            if (crusaderCards[i].skill == "endow")
            {
                if (aiManager.playerPassiveManager.GetWeapon() == null) { continue; }
                if (aiManager.playerPassiveManager.GetWeapon().cardName == "Weapon") { continue; }

                BattleVars.shared.originId = crusaderIds[i];
                BattleVars.shared.cardOnStandBy = crusaderCards[i];
                ID target = aiManager.playerPassiveManager.GetWeaponID();
                yield return aiManager.StartCoroutine(aiManager.ActivateAbility(target));
                continue;
            }
            else if (crusaderCards[i].skill == "reverse")
            {
                BattleVars.shared.originId = crusaderIds[i];
                BattleVars.shared.cardOnStandBy = crusaderCards[i];
                SkillManager.Instance.SetupTargetHighlights(aiManager, DuelManager.Instance.player, BattleVars.shared.cardOnStandBy);
                List<ID> opCreatureIds = DuelManager.GetAllValidTargets();
                if (opCreatureIds.Count == 0) { continue; }
                System.Random rnd = new System.Random();
                ID target = opCreatureIds.OrderBy(x => rnd.Next())
                                  .First();

                yield return aiManager.StartCoroutine(aiManager.ActivateAbility(target));
            }

        }

    }
}
