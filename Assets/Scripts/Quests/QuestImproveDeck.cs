public class QuestImproveDeck : QuestBiolerplate
{
    public override int QuestIndex => 1;

    public override string QuestName => "Quest 2 : Improve Your Deck";

    public override string QuestDescription => "There are 40 cards in your starter deck; a deck can have between 30 and 60 cards. Get rid of some of the cards you do not like/use, the cards removed from your deck are still yours and will remain in your storage.";

    public override string QuestObjective => "Objective: Remove at least one card from your deck. Click 'Your Deck' and click on the card(s) you want to remove.";

    public override string QuestReward => "Reward: 20";


    public override bool RequirementCheck() => PlayerData.shared.removedCardFromDeck;

    public override void RedeemQuest()
    {
        PlayerData.shared.electrum += 20;
        PlayerData.shared.completedQuests += "_1";
    }
}