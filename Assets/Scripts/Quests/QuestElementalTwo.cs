public class QuestElementalTwo : QuestBiolerplate
{
    public override int QuestIndex => 4;

    public override string QuestName => "Quest 5 : Level 2 elementals";

    public override string QuestDescription => "It is time to try an even harder fight; Level 2 elementals have well organized decks, but if you defeat one you'll get even better rewards. Good Luck.";

    public override string QuestObjective => "Objective: Defeat a Level 2 elemental. Click the 'Level 2' button to start the duel.";

    public override string QuestReward => "Reward: 40";

    public override bool RequirementCheck() => PlayerData.Shared.HasDefeatedLevel2;

    public override void RedeemQuest()
    {
        PlayerData.Shared.Electrum += 40;
       // PlayerData.Shared.completedQuests += "_4";
    }
}
