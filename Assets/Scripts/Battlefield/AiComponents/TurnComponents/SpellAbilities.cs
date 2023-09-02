using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpellAbilities
{
    public IEnumerator PlayMiracle(PlayerManager aiManager)
    {
        if (DuelManager.Instance.GetPossibleDamage(false) + 20 < aiManager.healthManager.GetCurrentHealth()) { yield break; }
        //Get Hand Cards
        var idCardList = aiManager.playerHand.GetAllValidCardIds();

        int cardIndex = idCardList.FindIndex(x => x.card.iD == "5li" || x.card.cardName == "7k2");

        if (cardIndex == -1) { yield break; }
        if (aiManager.playerQuantaManager.HasEnoughQuanta(idCardList[cardIndex].card.costElement, idCardList[cardIndex].card.cost))
        {
            BattleVars.shared.abilityOrigin = idCardList[cardIndex];

            aiManager.ActivateAbility(idCardList[cardIndex]);
        }
    }

    public IEnumerator PlayBlessings(PlayerManager aiManager)
    {
        //Get Hand Cards
        var idCardList = aiManager.playerHand.GetAllValidCardIds();
        int cardIndex = idCardList.FindIndex(x => x.card.iD == "5lf" || x.card.iD == "7jv");

        if (cardIndex == -1) { yield break; }

        for (int i = 0; i < 7; i++)
        {
            if (cardIndex == -1) { yield break; }
            if (!aiManager.playerQuantaManager.HasEnoughQuanta(idCardList[cardIndex].card.costElement, idCardList[cardIndex].card.cost)) { yield break; }

            BattleVars.shared.abilityOrigin = idCardList[cardIndex];

            var target = SkillManager.Instance.GetRandomTarget(aiManager, idCardList[cardIndex]);
            if (target == null) { yield break; }

            aiManager.ActivateAbility(target);

            idCardList = aiManager.playerHand.GetAllValidCardIds();
            cardIndex = idCardList.FindIndex(x => x.card.iD == "5lf" || x.card.iD == "7jv");
        }
    }

    public IEnumerator PlayAnimateWeapon(PlayerManager aiManager)
    {
        if (!aiManager.playerPassiveManager.GetWeapon().HasCard()) { yield break; }
        if (aiManager.playerPassiveManager.GetWeapon().card.cardName == "Weapon") { yield break; }
        //Get Hand Cards
        var idCardList = aiManager.playerHand.GetAllValidCardIds();
        int cardIndex = idCardList.FindIndex(x => x.card.iD == "7n2" || x.card.iD == "5oi");

        if (cardIndex == -1) { yield break; }
        if (aiManager.playerQuantaManager.HasEnoughQuanta(idCardList[cardIndex].card.costElement, idCardList[cardIndex].card.cost))
        {
            BattleVars.shared.abilityOrigin = idCardList[cardIndex];
            aiManager.ActivateAbility(aiManager.playerPassiveManager.GetWeapon());
        }
    }

    public IEnumerator ActivateRewind(PlayerManager aiManager)
    {
        var idCardList = aiManager.playerHand.GetAllValidCardIds();

        if (idCardList.Count == 0) { yield break; }

        int cardIndex = idCardList.FindIndex(x => x.card.iD == "5rk" || x.card.iD == "7q4");

        if (cardIndex == -1) { yield break; }

        for (int i = 0; i < 7; i++)
        {
            if (cardIndex == -1) { yield break; }
            if (!aiManager.IsCardPlayable(idCardList[cardIndex].card)) { yield break; }
            BattleVars.shared.abilityOrigin = idCardList[cardIndex];

            var target = SkillManager.Instance.GetRandomTarget(aiManager, idCardList[cardIndex]);
            if (target == null) { continue; }

            aiManager.ActivateAbility(target);
            idCardList = aiManager.playerHand.GetAllValidCardIds();
            //Check if still has explosion in hand, repeat loop
            if (idCardList.Count == 0) { yield break; }
            cardIndex = idCardList.FindIndex(x => x.card.iD == "5rk" || x.card.iD == "7q4");
        }
    }

    public IEnumerator ActivateQuicksand(PlayerManager aiManager)
    {
        var idCardList = aiManager.playerHand.GetAllValidCardIds();

        if (idCardList.Count == 0) { yield break; }
        int cardIndex = idCardList.FindIndex(x => x.card.iD == "593" || x.card.iD == "77j");

        if (cardIndex == -1) { yield break; }

        for (int i = 0; i < 7; i++)
        {
            if (cardIndex == -1) { yield break; }
            if (!aiManager.IsCardPlayable(idCardList[cardIndex].card)) { yield break; }
            BattleVars.shared.abilityOrigin = idCardList[cardIndex];

            var target = SkillManager.Instance.GetRandomTarget(aiManager, idCardList[cardIndex]);
            if (target == null) { continue; }

            aiManager.ActivateAbility(target);
            idCardList = aiManager.playerHand.GetAllValidCardIds();
            cardIndex = idCardList.FindIndex(x => x.card.iD == "593" || x.card.iD == "77j");
        }
    }

    public IEnumerator ActivateQuintessence(PlayerManager aiManager)
    {

        var idCardList = aiManager.playerHand.GetAllValidCardIds();

        if (idCardList.Count == 0) { yield break; }
        int cardIndex = idCardList.FindIndex(x => x.card.iD == "621" || x.card.iD == "80h");

        if (cardIndex == -1) { yield break; }

        for (int i = 0; i < 7; i++)
        {
            if (cardIndex == -1) { yield break; }
            if (!aiManager.IsCardPlayable(idCardList[cardIndex].card)) { yield break; }
            BattleVars.shared.abilityOrigin = idCardList[cardIndex];

            var target = SkillManager.Instance.GetRandomTarget(aiManager, idCardList[cardIndex]);
            if (target == null) { continue; }

            aiManager.ActivateAbility(target);
            idCardList = aiManager.playerHand.GetAllValidCardIds();
            cardIndex = idCardList.FindIndex(x => x.card.iD == "621" || x.card.iD == "80h");
        }
    }
}