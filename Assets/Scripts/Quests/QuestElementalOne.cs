public class QuestElementalOne : QuestBiolerplate
{
    public override int QuestIndex => 3;
    public override string QuestName => "Quest 4 : Level 1 elementals";

    public override string QuestDescription => "You are now ready for a more challenging fight. Level 1 elementals are not very well organized, their decks often lack a strategy, but sometimes they get lucky and they can fight hard. Winning against a level 1 elemental will grant you a spin at the wheel, get three cards of the same type and that card is yours.";

    public override string QuestObjective => "Objective: Defeat a Level 1 elemental. Click the 'Level 1' button to start the duel.";

    public override string QuestReward => "Reward: 35";

    public override bool RequirementCheck() => PlayerData.shared.hasDefeatedLevel1;
    public override void RedeemQuest()
    {
        PlayerData.shared.electrum += 35;
        PlayerData.shared.completedQuests += "_3";
    }
}