public abstract class AchievementBiolerplate
{
    public abstract int AchievementId { get; }
    public abstract string AchievementProperty { get; }
    public abstract string AchievementName { get; }
    public abstract string AchievementDescription { get; }

    public void RedeemAchievement(int aiDifficulty)
    {
        var pinfo = typeof(int).GetProperty(AchievementProperty);
        var value = pinfo.GetValue(AchievementManager.Instance.Achievements, null);

        if (value is int castValue)
        {
            if (castValue >= aiDifficulty) { return; }
            var coinsToAdd = (aiDifficulty - castValue) * 50;
            pinfo.SetValue(AchievementManager.Instance.Achievements, aiDifficulty);
            PlayerData.Shared.Electrum += coinsToAdd;
        }
    }
}