using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UnityEngine;


public class MediumAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{

    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {

        Debug.Log("Play Creatures");
        //Play Creatures
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, CardType.Creature));
        //Play Artifacts
        Debug.Log("Play Artifacts");
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, CardType.Artifact));
        //Play Weapon
        Debug.Log("Play Weapon");
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, CardType.Weapon));
        //Play Shield
        Debug.Log("Play Shield");
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, CardType.Shield));

        //Activate Spells
        Debug.Log("Play Spells");
        yield return aiManager.StartCoroutine(ActivateSpells(aiManager));


        //Activate Creature Abilities
        Debug.Log("Activate Creatures");
        yield return aiManager.StartCoroutine(ActivateAbilities(aiManager, CardType.Creature));
        Debug.Log("Activate Artifacts");
        yield return aiManager.StartCoroutine(ActivateAbilities(aiManager, CardType.Artifact));
        Debug.Log("Activate Weapon");
        yield return aiManager.StartCoroutine(ActivateAbilities(aiManager, CardType.Weapon));

        yield break;
    }
    private Dictionary<ID, int> threatList;
    private IEnumerator PlayPermanent(PlayerManager aiManager, CardType cardType)
    {
        if (cardType.Equals(CardType.Shield))
        {
            if(aiManager.playerPassiveManager.GetShield() != null)
            {
                yield break;
            }
        }
        if (cardType.Equals(CardType.Weapon))
        {
            if (aiManager.playerPassiveManager.GetWeapon() != null)
            {
                yield break;
            }
        }

        //Get Card list
        List<Card> cardList = new List<Card>(aiManager.GetHandCards());
        List<ID> idList = new List<ID>(aiManager.GetHandIds());

        if (!cardList.Exists(x => x.cardType.Equals(cardType))) { yield break; }
        int cardIndex = cardList.FindIndex(x => x.cardType.Equals(cardType) && aiManager.IsCardPlayable(x));
        if (cardIndex == -1) { yield break; }
        int loopBreak = 0;
        while (cardIndex != -1 && loopBreak < 7)
        {
            loopBreak++;

            if (DuelManager.IsSundialInPlay() && (cardList[cardIndex].iD == "5rp" || cardList[cardIndex].iD == "7q9"))
            {
                continue;
            }

            aiManager.PlayCardFromHandLogic(idList[cardIndex]);

            cardList = new List<Card>(aiManager.GetHandCards());
            idList = new List<ID>(aiManager.GetHandIds());
            cardIndex = cardList.FindIndex(x => x.cardType.Equals(cardType) && aiManager.IsCardPlayable(x));
            yield return new WaitForSeconds(BattleVars.shared.aiPlaySpeed);
        }
        yield return new WaitForSeconds(BattleVars.shared.aiPlaySpeed);
    }

    private IEnumerator ActivateSpells(PlayerManager aiManager)
    {
        List<Card> cardList = new List<Card>(aiManager.GetHandCards());
        List<ID> idList = new List<ID>(aiManager.GetHandIds());

        if (!cardList.Exists(x => x.cardType.Equals(CardType.Spell))) { yield break; }

        int cardIndex = cardList.FindIndex(x => x.cardType.Equals(CardType.Spell) && aiManager.IsCardPlayable(x));

        if(cardIndex == -1) { yield break; }
        int loopBreak = 0;
        while(cardIndex != -1 && loopBreak < 8)
        {
            loopBreak++;
            Card cardToCheck = cardList[cardIndex];
            //Setup Spell
            BattleVars.shared.originId = idList[cardIndex];
            BattleVars.shared.cardOnStandBy = cardToCheck;
            ID target = idList[cardIndex];

            if (BattleVars.shared.isSelectingTarget)
            {
                //Get Target
                SkillManager.Instance.SetupTargetHighlights(aiManager, DuelManager.player, BattleVars.shared.cardOnStandBy);
                //Get List of all Valid Targets
                List<ID> allValidTargets = DuelManager.GetAllValidTargets();
                //If there is no valid targets, reset and skip to next iteration
                if (allValidTargets.Count == 0)
                {
                    DuelManager.ResetTargeting();
                    cardList = new List<Card>(aiManager.GetHandCards());
                    idList = new List<ID>(aiManager.GetHandIds());
                    cardIndex = cardList.FindIndex(x => x.cardType.Equals(CardType.Spell) && aiManager.IsCardPlayable(x) && x.cardName != cardToCheck.cardName);
                    continue;
                }
                //Get Random Target
                target = GetPriorityTarget(allValidTargets, BattleVars.shared.cardOnStandBy.skill, aiManager);
                if (target == null)
                {
                    DuelManager.ResetTargeting();
                    cardIndex = cardList.FindIndex(x => aiManager.IsAbilityUsable(x) && x.cardName != cardToCheck.cardName);
                    continue;
                }

            }

            yield return aiManager.StartCoroutine(aiManager.ActivateAbility(target));

            yield return new WaitForSeconds(BattleVars.shared.aiPlaySpeed);
            cardList = new List<Card>(aiManager.GetHandCards());
            idList = new List<ID>(aiManager.GetHandIds());
            cardIndex = cardList.FindIndex(x => x.cardType.Equals(CardType.Spell) && aiManager.IsCardPlayable(x));
        }
    }

    private IEnumerator ActivateAbilities(PlayerManager aiManager, CardType cardType)
    {
        List<Card> cardList = new List<Card>();
        List<ID> idList = new List<ID>();

        switch (cardType)
        {
            case CardType.Creature:
                cardList = aiManager.playerCreatureField.GetAllCards();
                idList = aiManager.playerCreatureField.GetAllIds();
                if (cardList.Count == 0) { yield break; }
                break;
            case CardType.Artifact:
                cardList = aiManager.playerPermanentManager.GetAllCards();
                idList = aiManager.playerPermanentManager.GetAllIds();
                if (cardList.Count == 0) { yield break; }
                break;
            case CardType.Weapon:
                if (aiManager.playerPassiveManager.GetWeapon() == null) { yield break; }
                if (aiManager.playerPassiveManager.GetWeapon().cardName == "Weapon") { yield break; }
                cardList = new List<Card> { aiManager.playerPassiveManager.GetWeapon() };
                idList = new List<ID> { aiManager.playerPassiveManager.GetWeaponID() };
                break;
            default:
                break;
        }


        int cardIndex = cardList.FindIndex(x => aiManager.IsAbilityUsable(x));

        if (cardIndex == -1) { yield break; }

        int loopBreak = 0;
        while (cardIndex != -1 && loopBreak < 10)
        {
            loopBreak++;
            Card cardToCheck = cardList[cardIndex];
            //Setup Spell
            BattleVars.shared.originId = idList[cardIndex];
            BattleVars.shared.cardOnStandBy = cardToCheck;
            ID target = BattleVars.shared.originId;

            if (!BattleVars.shared.IsFixedTarget())
            {
                //Get Target
                SkillManager.Instance.SetupTargetHighlights(aiManager, DuelManager.player, BattleVars.shared.cardOnStandBy);
                //Get List of all Valid Targets
                List<ID> allValidTargets = DuelManager.GetAllValidTargets();
                //If there is no valid targets, reset and skip to next iteration
                if (allValidTargets.Count == 0)
                {
                    DuelManager.ResetTargeting();
                    cardIndex = cardList.FindIndex(x => aiManager.IsAbilityUsable(x) && x.cardName != cardToCheck.cardName);
                    continue;
                }
                //Get Random Target
                target = GetPriorityTarget(allValidTargets, BattleVars.shared.cardOnStandBy.skill, aiManager);
                if (target == null)
                {
                    DuelManager.ResetTargeting();
                    cardIndex = cardList.FindIndex(x => aiManager.IsAbilityUsable(x) && x.cardName != cardToCheck.cardName);
                    continue;
                }

            }

            yield return aiManager.StartCoroutine(aiManager.ActivateAbility(target));
            cardIndex = cardList.FindIndex(x => aiManager.IsAbilityUsable(x));
        }
    }
    
    List<string> skillsTargetSelf = new List<string> { "mitosiss", "liquid shadow", "immortality", "nymph", "heal", "chaos power", "overdrive", "readiness", "wisdom",
                                                        "butterfly", "momentum", "acceleration", "armor", "enchant", "adrenaline", "immolate", "blessing", "heavy armor"};

    List<string> skillsTargetOpponent = new List<string> { "sniper", "destroy", "web", "accretion", "devour", "infect", "congeal", "lobotomize", "paradox", "freeze", "infection",
                                                        "aflatoxin", "guard", "chaos", "earthquake", "fire bolt", "icebolt", "shockwave", "steal", "drain life", "gravity pull",
                                                        "lightning", "cremation", "antimatter" };

    public List<string> skillsWithTarget = new List<string> { "petrify"};

    public ID GetPriorityTarget(List<ID> iDs, string skill, PlayerManager aiManager)
    {
        System.Random rnd = new System.Random();
        if (skillsTargetSelf.Contains(skill))
        {
            List<ID> targets = iDs.FindAll(PredicateManager.Instance.rndSelfPred);
            if (targets == null) { return null; }
            if (targets.Count == 0) { return null; }
            return new List<ID>(targets.OrderBy(x => rnd.Next())).First();
        }
        if (skillsTargetOpponent.Contains(skill))
        {
            List<ID> targets = iDs.FindAll(PredicateManager.Instance.rndOpponentPred);
            if (targets == null) { return null; }
            if (targets.Count == 0) { return null; }
            return new List<ID>(targets.OrderBy(x => rnd.Next())).First();
        }

        if(skill == "endow")
        {
            List<ID> targets = iDs.FindAll(PredicateManager.Instance.endowPred);
            if (targets == null) { return null; }
            if (targets.Count == 0) { return null; }
            return new List<ID>(targets.OrderBy(x => rnd.Next())).First();
        }
        if (skill == "rage")
        {
            List<ID> targets = iDs.FindAll(PredicateManager.Instance.ragePred);
            if (targets == null) { return null; }
            if (targets.Count == 0) { return null; }
            return new List<ID>(targets.OrderBy(x => rnd.Next())).First();
        }

        if (skill == "holy light")
        {
            List<ID> targets = iDs.FindAll(PredicateManager.Instance.holyLightAtkPred);
            if (targets == null)
            {
                targets = iDs.FindAll(PredicateManager.Instance.holyLightDefPred);
                if (targets == null) { return null; }
                if (targets.Count == 0) { return null; }
                return new List<ID>(targets.OrderBy(x => rnd.Next())).First();
            }
            if (targets.Count == 0)
            {
                targets = iDs.FindAll(PredicateManager.Instance.holyLightDefPred);
                if (targets == null) { return null; }
                if (targets.Count == 0) { return null; }
                return new List<ID>(targets.OrderBy(x => rnd.Next())).First();
            }
            return new List<ID>(targets.OrderBy(x => rnd.Next())).First();
        }

        if(skill == "parallel universe")
        {
            List<ID> target = iDs.OrderBy(x => DuelManager.GetCard(x).AtkNow).ToList();
            return target[0];
        }

        if(skill == "nightmare")
        {
            List<ID> targets = iDs.FindAll(PredicateManager.Instance.nightmarePred);
            if (targets == null) { return null; }
            if (targets.Count == 0) { return null; }
            return new List<ID>(targets.OrderBy(x => rnd.Next())).First();
        }

        if (skill == "fractal")
        {
            List<ID> targets = iDs.FindAll(PredicateManager.Instance.fractalPred);
            if (targets == null) { return null; }
            if (targets.Count == 0) { return null; }
            return new List<ID>(targets.OrderBy(x => rnd.Next())).First();
        }

        if (skill == "purify")
        {
            if(aiManager.playerCounters.poison > 0)
            {
                return aiManager.playerDisplayer.GetObjectID();
            }
            List<ID> targets = iDs.FindAll(PredicateManager.Instance.purifyPred);
            if (targets == null) { return null; }
            if (targets.Count == 0) { return null; }
            return new List<ID>(targets.OrderBy(x => rnd.Next())).First();
        }

        if (skill == "beserk")
        {
            List<ID> targets = iDs.FindAll(PredicateManager.Instance.beserkPred);
            if (targets == null) { return null; }
            if (targets.Count == 0) { return null; }
            return new List<ID>(targets.OrderBy(x => rnd.Next())).First();
        }

        if (skill == "improve" || skill == "mutation")
        {
            List<ID> targets = iDs.FindAll(PredicateManager.Instance.mutationsPred);
            if (targets == null) 
            {
                targets = iDs.FindAll(PredicateManager.Instance.rndOpponentPred);
                if (targets == null) { return null; }
                if (targets.Count == 0) { return null; }
                return new List<ID>(targets.OrderBy(x => rnd.Next())).First();
            }
            if (targets.Count == 0)
            {
                targets = iDs.FindAll(PredicateManager.Instance.rndOpponentPred);
                if (targets == null) { return null; }
                if (targets.Count == 0) { return null; }
                return new List<ID>(targets.OrderBy(x => rnd.Next())).First();
            }
            return new List<ID>(targets.OrderBy(x => rnd.Next())).First();
        }
        if (skill == "reverse time")
        {
            List<ID> targets = iDs.FindAll(PredicateManager.Instance.mutationsPred);
            if (targets == null)
            {
                targets = iDs.FindAll(PredicateManager.Instance.rndOpponentPred);
                if (targets == null) { return null; }
                if (targets.Count == 0) { return null; }
                return new List<ID>(targets.OrderBy(x => rnd.Next())).First();
            }
            if (targets.Count == 0)
            {
                targets = iDs.FindAll(PredicateManager.Instance.rndOpponentPred);
                if (targets == null) { return null; }
                if (targets.Count == 0) { return null; }
                return new List<ID>(targets.OrderBy(x => rnd.Next())).First();
            }
            return new List<ID>(targets.OrderBy(x => rnd.Next())).First();
        }
        if (skill == "petrify")
        {
            List<ID> targets = iDs.FindAll(PredicateManager.Instance.petrifyPred);
            if (targets == null)
            {
                targets = iDs.FindAll(PredicateManager.Instance.rndOpponentPred);
                if (targets == null) { return null; }
                if (targets.Count == 0) { return null; }
                return new List<ID>(targets.OrderBy(x => rnd.Next())).First();
            }
            if (targets.Count == 0)
            {
                targets = iDs.FindAll(PredicateManager.Instance.rndOpponentPred);
                if (targets == null) { return null; }
                if (targets.Count == 0) { return null; }
                return new List<ID>(targets.OrderBy(x => rnd.Next())).First();
            }
            return new List<ID>(targets.OrderBy(x => rnd.Next())).First();
        }
        return null;
    }
}