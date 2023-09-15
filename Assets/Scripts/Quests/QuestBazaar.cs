public class QuestBazaar : QuestBiolerplate
{
    public override int QuestIndex => 2;

    public override string QuestName => "Quest 3 : The Bazaar";

    public override string QuestDescription => "Now that you have something in your storage you can sell it, get some money for it, and buy the cards you are looking for. You can buy almost any card at the bazaar, only the most rare cards are not for sale, you can get those only with a spin after winning against a level 1 or higher elemental.";

    public override string QuestObjective => "Objective: Click on the 'Bazaar' button. Sell at least one card and buy at least one card.";

    public override string QuestReward => "Reward: 30";

    public override bool RequirementCheck() => PlayerData.Shared.hasSoldCardBazaar && PlayerData.Shared.hasBoughtCardBazaar;
    public override void RedeemQuest()
    {
        PlayerData.Shared.electrum += 30;
        PlayerData.Shared.completedQuests += "_2";
    }
}