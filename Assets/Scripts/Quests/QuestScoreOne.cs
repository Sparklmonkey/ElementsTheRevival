public class QuestScoreOne : QuestBiolerplate
{
    public override int QuestIndex => 5;

    public override string QuestName => "Quest 6 : Your score";

    public override string QuestDescription => "Every time you win a match you gain some score points, if you lose a game, you lose some. \n Your score determines the level of Arena Decks that you create. Increase your score and come back for a juicy reward.";

    public override string QuestObjective => "Objective: Reach a score of at least 150";

    public override string QuestReward => "Reward: 150";

    public override bool RequirementCheck() => PlayerData.shared.playerScore > 150;

    public override void RedeemQuest()
    {
        PlayerData.shared.electrum += 150;
        PlayerData.shared.completedQuests += "_5";
    }
}
