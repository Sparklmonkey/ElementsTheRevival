using System.Collections;

public class PermanentAbilities
{
    public IEnumerator ActivateHourGlass(PlayerManager aiManager)
    {
        var idCardList = aiManager.playerHand.GetAllValidCardIds();

        var cardIndex = idCardList.FindIndex(x => x.card.iD == "5rl" || x.card.cardName == "7q5");

        if (cardIndex == -1) { yield break; }
        if (aiManager.PlayerQuantaManager.HasEnoughQuanta(idCardList[cardIndex].card.costElement, idCardList[cardIndex].card.cost))
        {
            BattleVars.Shared.AbilityOrigin = idCardList[cardIndex];

            aiManager.ActivateAbility(idCardList[cardIndex]);
        }
    }
}