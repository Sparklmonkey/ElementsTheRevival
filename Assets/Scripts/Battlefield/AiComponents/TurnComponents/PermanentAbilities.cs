using System.Collections;
using System.Collections.Generic;

public class PermanentAbilities
{
    public IEnumerator ActivateHourGlass(PlayerManager aiManager)
    {
        var idCardList = aiManager.playerHand.GetAllValidCardIds();

        int cardIndex = idCardList.FindIndex(x => x.card.iD == "5rl" || x.card.cardName == "7q5");

        if (cardIndex == -1) { yield break; }
        if (aiManager.playerQuantaManager.HasEnoughQuanta(idCardList[cardIndex].card.costElement, idCardList[cardIndex].card.cost))
        {
            BattleVars.shared.abilityOrigin = idCardList[cardIndex];

            aiManager.ActivateAbility(idCardList[cardIndex]);
        }
    }
}