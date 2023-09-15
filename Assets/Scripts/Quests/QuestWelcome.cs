public class QuestWelcome : QuestBiolerplate
{
    public override int QuestIndex => 0;

    public override string QuestName => "Quest 1 : Welcome!";

    public override string QuestDescription => "This is Element's main menu. You just received 40 cards for your starter deck; are you ready for your first duel?";

    public override string QuestObjective => "Objective: Defeat a level 0 elemental, click on the 'Quests' button AFTER your victory to get your reward. \n Click the 'Level 0' button to start your first duel.";

    public override string QuestReward => "Reward: 10";

    public override void RedeemQuest()
    {
        PlayerData.Shared.electrum += 10;
        PlayerData.Shared.completedQuests += "_0";
    }

    public override bool RequirementCheck() => PlayerData.Shared.hasDefeatedLevel0;

}