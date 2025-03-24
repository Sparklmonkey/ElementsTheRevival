using System.Linq;

namespace PlayerAchievements.AchievementScripts.Achievements.Collection;

public class AchievementCollectWater : IAchievement
{
    public PlayerAchievement HasCompletedAchievement(NewAchievement baseAchievement, SavedData playerData,
        CardDatabase cardDatabase)
    {
        var returnAchievement = new PlayerAchievement();
        if (SetupTierInfo(ref returnAchievement, baseAchievement.BronzeTier, playerData, cardDatabase, IsBronzeAchieved, 0))
        {
            if (SetupTierInfo(ref returnAchievement, baseAchievement.GoldTier, playerData, cardDatabase, IsGoldAchieved, 1))
            {
                _ = SetupTierInfo(ref returnAchievement, baseAchievement.PlatinumTier, playerData, cardDatabase,
                    IsPlatinumAchieved, 2);
            }
        }
        return returnAchievement;
    }

    public bool SetupTierInfo(ref PlayerAchievement playerAchievement, AchievementTier tier, 
        SavedData playerData, CardDatabase cardDatabase, TierCheckDelegate tierCheckDelegate, int tierValue)
    {
        var completionPercent = tierCheckDelegate(playerData, cardDatabase);
        if (completionPercent == 100)
        {
            return true;
        }
        playerAchievement.TierAchieved = tierValue;
        playerAchievement.CompletionPercent = completionPercent;
        playerAchievement.Description = tier.Description;
        playerAchievement.Name = tier.Title;
        playerAchievement.CardReward = tier.CardReward.Split(" ").ToList();
        playerAchievement.GoldReward = tier.GoldReward;
        return false;
    }

    public int IsBronzeAchieved(SavedData playerData, CardDatabase cardDatabase)
    {
        var fullCardList = playerData.GetFullCardList();
        return cardDatabase.HasCardSingleCollection(fullCardList, Element.Water);
    }

    public int IsGoldAchieved(SavedData playerData, CardDatabase cardDatabase)
    {
        var fullCardList = playerData.GetFullCardList();
        return cardDatabase.HasCardHalfCollection(fullCardList, Element.Water);
    }

    public int IsPlatinumAchieved(SavedData playerData, CardDatabase cardDatabase)
    {
        var fullCardList = playerData.GetFullCardList();
        return cardDatabase.HasCardCompleteCollection(fullCardList, Element.Water);
    }
}