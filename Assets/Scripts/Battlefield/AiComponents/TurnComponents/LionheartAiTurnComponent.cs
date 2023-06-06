using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LionheartAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{
    private IEnumerator ActivateCrusaders(PlayerManager aiManager)
    {
        List<Card> cardList = new List<Card>(aiManager.playerCreatureField.GetAllCards());
        List<ID> idList = new List<ID>(aiManager.playerCreatureField.GetAllIds());
        List<Card> crusaderCards = new List<Card>();
        List<ID> crusaderIds = new List<ID>();

        for (int i = 0; i < cardList.Count; i++)
        {
            switch (cardList[i].cardName)
            {
                case "Elite Crusader":
                    crusaderCards.Add(cardList[i]);
                    crusaderIds.Add(idList[i]);
                    break;
                default:
                    break;
            }
        }

        if (crusaderCards.Count == 0) { yield break; }

        for (int i = 0; i < crusaderCards.Count; i++)
        {
            if (!aiManager.IsAbilityUsable(crusaderCards[i])) { continue; }
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

    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {
        //PlayWeapon
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Elite Eternity"));

        //Play Creatures in order of priority
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Crusader"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Anubis"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Electrum Hourglass"));

        //Play Shield
        yield return aiManager.StartCoroutine(PlayShield(aiManager, "Turtle Shield"));

        //Play Spells
        yield return aiManager.StartCoroutine(spellManager.PlayAnimateWeapon(aiManager));

        //Activate Abilities
        yield return aiManager.StartCoroutine(ActivateRepeatAbilityNoTarget(aiManager, CardType.Artifact, "Golden Hourglass", "Electrum Hourglass"));
        yield return aiManager.StartCoroutine(ActivateCrusaders(aiManager));
        yield return aiManager.StartCoroutine(ActivateRepeatAbilityWithTarget(aiManager, CardType.Artifact, "Anubis", "Elite Anubis", x => x.Owner.Equals(OwnerEnum.Opponent)));
        yield return aiManager.StartCoroutine(spellManager.ActivateQuintessence(aiManager));
        yield return aiManager.StartCoroutine(ActivateRepeatAbilityWithTarget(aiManager, CardType.Artifact, "Eternity", "Elite Eternity", x => x.Owner.Equals(OwnerEnum.Player)));

    }
}
