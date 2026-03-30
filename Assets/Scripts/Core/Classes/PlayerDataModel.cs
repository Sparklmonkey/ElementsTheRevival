using System.Collections.Generic;


public class PlayerGameStatsDataModel
{
    public int GamesWon;
    public int GamesLost;
    public int OverallScore;
    public int CurrentSeasonScore;
}

public class PlayerArenaDataModel
{
    public Deck ArenaT50Deck;
    public int ArenaWins;
    public int ArenaLosses;
}

public class PlayerOracleDataModel
{
    public string NextFalseGod;
    public string PetId;
    public int PetCount;
    public string OracleLastPlayed;
}

public class PlayerQuestDataModel
{
    public string CompletedQuests = "";
    public int CurrentQuestIndex = 0;
    //Quest Specific Flags
    //TODO: Move to API calls
    public bool HasDefeatedLevel0;
    public bool HasDefeatedLevel1;
    public bool HasDefeatedLevel2;
    public bool RemovedCardFromDeck;
    public bool HasBoughtCardBazaar;
    public bool HasSoldCardBazaar;
}

public class PlayerAchievementDataModel
{
    
}

public class PlayerDataModel
{
    public string CardInventory;
    public Deck CurrentDeck;
    public int Electrum;
    public List<Deck> SavedDecks;
    
    
    public string Username = "";
    public string Email = "";
    
}

public class Deck
{
    public Element Mark;
    public string CardString;
}