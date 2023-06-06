using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JezebelAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{
    private int cloakCount = 0;

    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {
        cloakCount--;
        //Play Arsenic if none in play
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Vampire Dagger"));
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Permafrost Shield"));

        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Nymph's Tear", "Elite Nymph's Tear", x => x.Owner.Equals(OwnerEnum.Opponent)));
        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Siphon Life", "Siphon Life", x => x.Owner.Equals(OwnerEnum.Player)));
        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Improved Steal", "Improved Steal", x => x.Owner.Equals(OwnerEnum.Player)));

        yield return aiManager.StartCoroutine(ActivateAbilities(aiManager, CardType.Creature));
        //Activate Cloak if no cloak active
        if (cloakCount <= 0)
        {
            yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Cloak"));
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

            if (BattleVars.shared.isSelectingTarget)
            {
                //Get Target
                SkillManager.Instance.SetupTargetHighlights(aiManager, DuelManager.Instance.player, BattleVars.shared.cardOnStandBy);
                //Get List of all Valid Targets
                List<ID> allValidTargets = DuelManager.GetAllValidTargets();
                //If there is no valid targets, reset and skip to next iteration
                if (allValidTargets.Count == 0)
                {
                    DuelManager.Instance.ResetTargeting();
                    cardIndex = cardList.FindIndex(x => aiManager.IsAbilityUsable(x) && x.cardName != cardToCheck.cardName);
                    continue;
                }
                //Get Random Target
                System.Random rnd = new System.Random();
                target = allValidTargets.OrderBy(x => rnd.Next())
                                  .First();

            }

            yield return aiManager.StartCoroutine(aiManager.ActivateAbility(target));
            cardIndex = cardList.FindIndex(x => aiManager.IsAbilityUsable(x));
        }
    }

}
