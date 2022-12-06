using System.Collections;
using System.Collections.Generic;

public class PermanentAbilities
{
    public IEnumerator ActivateHourGlass(PlayerManager aiManager)
    {
        List<Card> cardList = new List<Card>(aiManager.playerPermanentManager.GetAllCards());
        List<ID> idList = new List<ID>(aiManager.playerPermanentManager.GetAllIds());
        List<Card> hourGlassCards = new List<Card>();
        List<ID> hourGlassIds = new List<ID>();

        for (int i = 0; i < cardList.Count; i++)
        {
            switch (cardList[i].cardName)
            {
                case "Electrum Hourglass":
                    hourGlassCards.Add(cardList[i]);
                    hourGlassIds.Add(idList[i]);
                    break;
                case "Golden Hourglass":
                    hourGlassCards.Add(cardList[i]);
                    hourGlassIds.Add(idList[i]);
                    break;
                default:
                    break;
            }
        }

        if (hourGlassCards.Count == 0) { yield break; }

        for (int i = 0; i < hourGlassCards.Count; i++)
        {
            if (!aiManager.IsAbilityUsable(hourGlassCards[i])) { continue; }
            BattleVars.shared.originId = hourGlassIds[i];
            BattleVars.shared.cardOnStandBy = hourGlassCards[i];

            ID target = new ID(OwnerEnum.Opponent, FieldEnum.Player, 1);
            yield return aiManager.StartCoroutine(aiManager.ActivateAbility(target));
        }
    }
}