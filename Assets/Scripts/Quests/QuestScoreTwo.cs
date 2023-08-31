using UnityEngine;

public class QuestScoreTwo : QuestBiolerplate
{
    public override int QuestIndex => 6;

    public override string QuestName => "Quest 7 : Rare Cards";

    public override string QuestDescription => "Rare cards are extremely hard to find, although they are not necessary for a winning strategy, they are often a nice addition for your deck. Get an even higher score and I will reward you with a rare card.";

    public override string QuestObjective => "Objective: Reach a score of at least 500";

    public override string QuestReward => "Reward: A rare card";

    public override bool RequirementCheck() => PlayerData.shared.playerScore > 500;

    public override void RedeemQuest()
    {
        PlayerPrefs.SetFloat("ShouldShowRareCard", 1);
        PlayerData.shared.completedQuests += "_6";
    }
}
