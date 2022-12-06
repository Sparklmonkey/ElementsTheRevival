using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpellAbilities
{
    public IEnumerator PlayMiracle(PlayerManager aiManager)
    {
        if (DuelManager.GetPossibleDamage(false) + 20 < aiManager.healthManager.GetCurrentHealth()) { yield break; }
        //Get Hand Cards
        List<Card> cardList = new List<Card>(aiManager.GetHandCards());
        List<ID> idList = new List<ID>(aiManager.GetHandIds());

        int cardIndex = cardList.FindIndex(x => x.cardName == "Miracle" || x.cardName == "Improved Miracle");

        if (cardIndex == -1) { yield break; }
        if (aiManager.playerQuantaManager.HasEnoughQuanta(cardList[cardIndex].costElement, cardList[cardIndex].cost))
        {
            BattleVars.shared.originId = idList[cardIndex];
            BattleVars.shared.cardOnStandBy = cardList[cardIndex];
            ID target = new ID(OwnerEnum.Opponent, FieldEnum.Player, 1);

            yield return aiManager.StartCoroutine(aiManager.ActivateAbility(target));
        }
    }

    public IEnumerator PlayBlessings(PlayerManager aiManager)
    {
        //Get Hand Cards
        List<Card> cardList = new List<Card>(aiManager.GetHandCards());
        List<ID> idList = new List<ID>(aiManager.GetHandIds());

        int cardIndex = cardList.FindIndex(x => x.cardName == "Blessing" || x.cardName == "Improved Blessing");

        if (cardIndex == -1) { yield break; }

        int loopbreak = 0;
        while (cardIndex != -1 && loopbreak < 7)
        {
            loopbreak++;
            if (aiManager.playerQuantaManager.HasEnoughQuanta(cardList[cardIndex].costElement, cardList[cardIndex].cost))
            {
                BattleVars.shared.originId = idList[cardIndex];
                BattleVars.shared.cardOnStandBy = cardList[cardIndex];

                SkillManager.Instance.SetupTargetHighlights(aiManager, DuelManager.player, BattleVars.shared.cardOnStandBy);
                ID target = DuelManager.GetAllValidTargets().Find(x => x.Owner.Equals(OwnerEnum.Opponent));

                yield return aiManager.StartCoroutine(aiManager.ActivateAbility(target));

                cardList = new List<Card>(aiManager.GetHandCards());
                idList = new List<ID>(aiManager.GetHandIds());
                cardIndex = cardList.FindIndex(x => x.cardName == "Blessing" || x.cardName == "Improved Blessing");
            }
            else
            {
                yield break;
            }
        }
    }

    public IEnumerator PlayAnimateWeapon(PlayerManager aiManager)
    {
        if (aiManager.playerPassiveManager.GetWeapon() == null) { yield break; }
        if (aiManager.playerPassiveManager.GetWeapon().cardName == "Weapon") { yield break; }
        //Get Hand Cards
        List<Card> cardList = new List<Card>(aiManager.GetHandCards());
        List<ID> idList = new List<ID>(aiManager.GetHandIds());


        int cardIndex = cardList.FindIndex(x => x.cardName == "Animate Weapon" || x.cardName == "Flying Weapon");

        if (cardIndex == -1) { yield break; }
        if (aiManager.playerQuantaManager.HasEnoughQuanta(cardList[cardIndex].costElement, cardList[cardIndex].cost))
        {
            BattleVars.shared.originId = idList[cardIndex];
            BattleVars.shared.cardOnStandBy = cardList[cardIndex];
            ID target = aiManager.playerPassiveManager.GetWeaponID();

            yield return aiManager.StartCoroutine(aiManager.ActivateAbility(target));
        }
    }

    public IEnumerator ActivateRewind(PlayerManager aiManager)
    {
        List<Card> cardList = new List<Card>(aiManager.GetHandCards());
        List<ID> idList = new List<ID>(aiManager.GetHandIds());

        if (cardList.Count == 0) { yield break; }

        int cardIndex = cardList.FindIndex(x => x.cardName == "Reverse Time" || x.cardName == "Rewind");
        if (cardIndex == -1) { yield break; }

        int loopbreak = 0;
        while (cardIndex != -1 && loopbreak < 7)
        {
            loopbreak++;
            //Play Spell
            if (!aiManager.IsCardPlayable(cardList[cardIndex])) { continue; }
            BattleVars.shared.originId = idList[cardIndex];
            BattleVars.shared.cardOnStandBy = cardList[cardIndex];
            yield return aiManager.StartCoroutine(aiManager.ActivateAbility(idList[cardIndex]));

            SkillManager.Instance.SetupTargetHighlights(aiManager, DuelManager.player, BattleVars.shared.cardOnStandBy);

            List<ID> opCreatureIds = DuelManager.GetAllValidTargets();
            opCreatureIds = opCreatureIds.FindAll(x => x.Owner.Equals(OwnerEnum.Player));
            if (opCreatureIds.Count == 0) { yield break; }
            System.Random rnd = new System.Random();
            ID target = opCreatureIds.OrderBy(x => rnd.Next())
                              .First();

            yield return aiManager.StartCoroutine(aiManager.ActivateAbility(target));

            cardList = new List<Card>(aiManager.GetHandCards());
            idList = new List<ID>(aiManager.GetHandIds());
            //Check if still has explosion in hand, repeat loop
            cardIndex = cardList.FindIndex(x => x.cardName == "Reverse Time" || x.cardName == "Rewind");
        }
    }

    public IEnumerator ActivateQuicksand(PlayerManager aiManager)
    {
        List<Card> cardList = new List<Card>(aiManager.GetHandCards());
        List<ID> idList = new List<ID>(aiManager.GetHandIds());

        if (cardList.Count == 0) { yield break; }

        List<Card> rewindCards = new List<Card>();
        List<ID> rewindIds = new List<ID>();

        for (int i = 0; i < cardList.Count; i++)
        {
            switch (cardList[i].cardName)
            {
                case "Quicksand":
                    rewindCards.Add(cardList[i]);
                    rewindIds.Add(idList[i]);
                    break;
                case "Earthquake":
                    rewindCards.Add(cardList[i]);
                    rewindIds.Add(idList[i]);
                    break;
                default:
                    break;
            }
        }

        if (rewindCards.Count == 0) { yield break; }

        for (int i = 0; i < rewindCards.Count; i++)
        {
            if (!aiManager.IsCardPlayable(rewindCards[i])) { continue; }
            BattleVars.shared.originId = rewindIds[i];
            BattleVars.shared.cardOnStandBy = rewindCards[i];


            SkillManager.Instance.SetupTargetHighlights(aiManager, DuelManager.player, BattleVars.shared.cardOnStandBy);

            List<ID> opCreatureIds = DuelManager.GetAllValidTargets();
            if (opCreatureIds.Count == 0) { yield break; }
            System.Random rnd = new System.Random();
            ID target = opCreatureIds.OrderBy(x => rnd.Next())
                              .First();

            yield return aiManager.StartCoroutine(aiManager.ActivateAbility(target));
        }
    }

    public IEnumerator ActivateQuintessence(PlayerManager aiManager)
    {
        List<Card> cardList = new List<Card>(aiManager.playerHand.GetAllCards());
        List<ID> idList = new List<ID>(aiManager.playerHand.GetAllIds());

        List<Card> quintCards = new List<Card>();
        List<ID> quintIds = new List<ID>();

        for (int i = 0; i < cardList.Count; i++)
        {
            switch (cardList[i].cardName)
            {
                case "Elite Quintessence":
                    quintCards.Add(cardList[i]);
                    quintIds.Add(idList[i]);
                    break;
                case "Quintessence":
                    quintCards.Add(cardList[i]);
                    quintIds.Add(idList[i]);
                    break;
                default:
                    break;
            }
        }
        if (quintCards.Count == 0) { yield break; }


        List<Card> creatureCardList = new List<Card>(aiManager.playerCreatureField.GetAllCards());
        List<ID> creatureIdList = new List<ID>(aiManager.playerCreatureField.GetAllIds());

        for (int i = 0; i < quintCards.Count; i++)
        {
            if (!aiManager.IsAbilityUsable(quintCards[i])) { continue; }

            int index = creatureCardList.FindIndex(x => !x.passive.Contains("immaterial"));
            if (index == -1) { continue; }
            BattleVars.shared.originId = quintIds[i];
            BattleVars.shared.cardOnStandBy = quintCards[i];
            ID target = quintIds[index];

            yield return aiManager.StartCoroutine(aiManager.ActivateAbility(target));
        }

    }
}