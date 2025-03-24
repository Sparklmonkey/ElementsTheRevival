using System.Collections.Generic;

namespace PlayerAchievements.AchievementScripts;

public class PlayerAchievement
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Condition { get; set; }
    public string Description { get; set; }
    public int CompletionPercent { get; set; }
    public int TierAchieved { get; set; }
    public int Element { get; set; }
    public int Category { get; set; }
    public int GoldReward { get; set; }
    public List<string> CardReward { get; set; }
}