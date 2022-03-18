using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class BaseAiTurnComponent : IAiTurnComponent
{
    public void PlayPillars(PlayerManager aiManager)
    {
        //Pillars
        List<Card> cardList = aiManager.GetHandCards();
        List<ID> idList = aiManager.GetHandIds();
        for (int i = 0; i < cardList.Count; i++)
        {
            if (cardList[i].type.Equals(CardType.Pillar))
            {
                aiManager.PlayCardFromHandLogic(idList[i]);
            }
        }
    }

    public void RestOfTurn(PlayerManager aiManager)
    {
        List<Card> cardList = new List<Card>(aiManager.GetHandCards());
        List<ID> idList = new List<ID>(aiManager.GetHandIds());

        for (int i = 0; i < cardList.Count; i++)
        {
            if (!cardList[i].type.Equals(CardType.Spell) && aiManager.IsCardPlayable(cardList[i]))
            {
                aiManager.PlayCardFromHandLogic(idList[i]);
            }
        }

        //Activate Spells
        cardList = new List<Card>(aiManager.GetHandCards());
        idList = new List<ID>(aiManager.GetHandIds());

        for (int i = 0; i < cardList.Count; i++)
        {
            Card cardToCheck = cardList[i];
            //Check if card is a Spell and can be played
            if (cardToCheck.type.Equals(CardType.Spell) && aiManager.IsCardPlayable(cardToCheck))
            {
                //Setup Spell
                BattleVars.shared.originId = idList[i];
                BattleVars.shared.spellOnStandBy = cardToCheck.spellAbility;
                ID target = idList[i];

                if (!cardToCheck.spellAbility.isTargetFixed)
                {
                    //Get Target
                    DuelManager.SetupValidTargets(BattleVars.shared.spellOnStandBy, null);
                    //Get List of all Valid Targets
                    List<ID> allValidTargets = DuelManager.GetAllValidTargets();
                    //If there is no valid targets, reset and skip to next iteration
                    if (allValidTargets.Count == 0)
                    {
                        DuelManager.ResetTargeting();
                        continue;
                    }
                    //Get Random Target
                    target = allValidTargets.GetRandomValue();
                }

                aiManager.ActivateAbility(target);
            }
        }

        //Activate Creature Abilities
        cardList = new List<Card>(aiManager.playerCreatureField.GetAllCards());
        idList = new List<ID>(aiManager.playerCreatureField.GetAllIds());

        for (int i = 0; i < cardList.Count; i++)
        {
            Card creature = cardList[i];
            //Check if creature can use ability, if not skip to next iteration
            if (!aiManager.IsAbilityUsable(creature)) { continue; }

            //Setup Activate Ability
            BattleVars.shared.originId = idList[i];
            BattleVars.shared.abilityOnStandBy = creature.activeAbility;
            ID target = idList[i];
            //If a target is needed, get one
            if (creature.activeAbility.ShouldSelectTarget)
            {
                //Get Target
                DuelManager.SetupValidTargets(null, creature.activeAbility);
                //Get List of all Valid Targets
                List<ID> allValidTargets = DuelManager.GetAllValidTargets();
                //If there is no valid targets, reset and skip to next iteration
                if (allValidTargets.Count == 0)
                {
                    DuelManager.ResetTargeting();
                    continue;
                }
                //Get Random Target
                target = allValidTargets.GetRandomValue();
            }

            aiManager.ActivateAbility(target);
        }

        //Activate Artifact Abilities
        cardList = new List<Card>(aiManager.playerPermanentManager.GetAllCards());
        idList = new List<ID>(aiManager.playerPermanentManager.GetAllIds());

        for (int i = 0; i < cardList.Count; i++)
        {
            Card permanent = cardList[i];
            //Check if creature can use ability, if not skip to next iteration
            if (!aiManager.IsAbilityUsable(permanent)) { continue; }

            //Setup Activate Ability
            BattleVars.shared.originId = idList[i];
            BattleVars.shared.abilityOnStandBy = permanent.activeAbility;
            ID target = idList[i];
            //If a target is needed, get one
            if (permanent.activeAbility.ShouldSelectTarget)
            {
                //Get Target
                DuelManager.SetupValidTargets(null, permanent.activeAbility);
                //Get List of all Valid Targets
                List<ID> allValidTargets = DuelManager.GetAllValidTargets();
                //If there is no valid targets, reset and skip to next iteration
                if (allValidTargets.Count == 0)
                {
                    DuelManager.ResetTargeting();
                    continue;
                }
                //Get Random Target
                target = allValidTargets.GetRandomValue();
            }

            aiManager.ActivateAbility(target);
        }
    }
}