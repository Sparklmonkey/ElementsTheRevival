public abstract class QuestBiolerplate
{
    public abstract string QuestName { get; }
    public abstract string QuestDescription { get; }
    public abstract string QuestObjective { get; }
    public abstract string QuestReward { get; }

    public abstract bool RequirementCheck();
    public abstract void RedeemQuest();
}
