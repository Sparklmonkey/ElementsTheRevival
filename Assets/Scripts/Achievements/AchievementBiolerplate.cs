using System.Reflection;

public abstract class AchievementBiolerplate
{
    public abstract int AchievementId { get; }
    public abstract string AchievementProperty { get; }
    public abstract string AchievementName { get; }
    public abstract string AchievementDescription { get; }

    public void RedeemAchievement(int aiDifficulty)
    {
        PropertyInfo pinfo = typeof(int).GetProperty(AchievementProperty);
        object value = pinfo.GetValue(AchievementManager.Instance.achievements, null);

        if (value is int castValue)
        {
            if (castValue >= aiDifficulty) { return; }
            int coinsToAdd = (aiDifficulty - castValue) * 50;
            pinfo.SetValue(AchievementManager.Instance.achievements, aiDifficulty);
            PlayerData.shared.electrum += coinsToAdd;
        }
    }
}