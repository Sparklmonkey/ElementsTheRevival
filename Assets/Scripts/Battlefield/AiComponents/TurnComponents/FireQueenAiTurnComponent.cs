using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireQueenAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{

    private IEnumerator ActivateEliteQueen(PlayerManager aiManager)
    {
        List<IDCardPair> creaturelist = new (aiManager.playerCreatureField.GetAllValidCardIds());
        List<IDCardPair> queenCards = new();

        foreach (var creature in creaturelist)
        {
            switch (creature.card.cardName)
            {
                case "Elite Queen":
                    queenCards.Add(creature);
                    break;
                default:
                    break;
            }
        }

        if (queenCards.Count == 0) { yield break; }

        foreach (var queen in queenCards)
        {
            if (!aiManager.IsAbilityUsable(queen)) { continue; }
            BattleVars.shared.abilityOrigin = queen;
            aiManager.ActivateAbility(queen);
        }
    }


    private IEnumerator ActivateWeapons(PlayerManager aiManager)
    {
        var weapon = aiManager.playerPassiveManager.GetWeapon();
        if (weapon.card.cardName == "Weapon") { yield break; }
        if (aiManager.IsAbilityUsable(weapon)) { yield break; }
        var target = SkillManager.Instance.GetRandomTarget(aiManager, weapon);
        if(target == null) { yield break; }

        BattleVars.shared.abilityOrigin = weapon;

        aiManager.ActivateAbility(target);
    }

    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {
        //Play Weapon if none in play
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Eagle's Eye"));
        yield return aiManager.StartCoroutine(spellManager.PlayAnimateWeapon(aiManager));
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Elite Fahrenheit"));
        yield return aiManager.StartCoroutine(spellManager.PlayAnimateWeapon(aiManager));
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Eagle's Eye"));
        yield return aiManager.StartCoroutine(spellManager.PlayAnimateWeapon(aiManager));
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Elite Fahrenheit"));
        yield return aiManager.StartCoroutine(spellManager.PlayAnimateWeapon(aiManager));

        //Play Creatures
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Queen"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Feral Bond"));

        //Activate Elite Queen
        yield return aiManager.StartCoroutine(ActivateEliteQueen(aiManager));
        //Activate Weapons
        yield return aiManager.StartCoroutine(ActivateWeapons(aiManager));
    }
}
