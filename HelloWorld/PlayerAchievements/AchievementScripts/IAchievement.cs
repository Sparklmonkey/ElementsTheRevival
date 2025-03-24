using System;
using System.Collections.Generic;
using PlayerAchievements.AchievementScripts.Achievements;

namespace PlayerAchievements.AchievementScripts;

    public interface IAchievement
    {
        public PlayerAchievement HasCompletedAchievement(NewAchievement baseAchievement, SavedData playerData,
            CardDatabase cardDatabase);
    
        public int IsBronzeAchieved(SavedData playerData, CardDatabase cardDatabase);
        public int IsGoldAchieved(SavedData playerData, CardDatabase cardDatabase);
        public int IsPlatinumAchieved(SavedData playerData, CardDatabase cardDatabase);
    }
    
public class NewAchievement
{
    public int Id { get; set; }
    public AchievementTier BronzeTier { get; set; }
    public AchievementTier GoldTier { get; set; }
    public AchievementTier PlatinumTier { get; set; }
    public string AchievementScript { get; set; }
    public int Element { get; set; }
    public int Category { get; set; }
}

public class SavedData
{ 
    public int MarkElement { get; set; }
    public string CurrentDeck { get; set; }
    public string InventoryCards { get; set; }
    public int Electrum { get; set; }
    public int GamesWon { get; set; }
    public int GamesLost { get; set; }
    public int PlayerScore { get; set; }
    public int CurrentQuestIndex { get; set; }
    public bool PlayedOracleToday { get; set; }
    public bool HasDefeatedLevel0 { get; set; }
    public bool HasDefeatedLevel1 { get; set; }
    public bool HasDefeatedLevel2 { get; set; }
    public bool RemovedCardFromDeck { get; set; }
    public bool HasBoughtCardBazaar { get; set; }
    public bool HasSoldCardBazaar { get; set; }
    public string OracleLastPlayed { get; set; }
    public string ArenaT50Deck { get; set; }
    public int ArenaT50Mark { get; set; }
    public int ArenaWins { get; set; }
    public int ArenaLosses { get; set; }


    public List<string> GetFullCardList()
    {
        var cardList = InventoryCards.DeserializeDeck();
        cardList.AddRange(CurrentDeck.DeserializeDeck());
        return cardList;
    }
}