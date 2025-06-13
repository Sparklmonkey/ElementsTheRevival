using System.Collections.Generic;

public class PlayerDataModel
{
    public Element MarkElement;
    public string CurrentDeck;
    public Element ArenaT50Mark;
    public string ArenaT50Deck;
    public string InventoryCards;
    public int Electrum;
    public int GamesWon;
    public int GamesLost;
    public int ArenaWins;
    public int ArenaLosses;
    public int PlayerScore;
    public string Username = "";
    public string Email = "";
    public string CompletedQuests = "";
    public int CurrentQuestIndex = 0;
    public List<string> SavedDecks;
    public string NextFalseGod;
    public string PetName;
    public int PetCount;
    public bool PlayedOracleToday;
    public string OracleLastPlayed;
    public int LastOracleDay;
    
    //Quest Specific Flags
    //TODO: Move to API calls
    public bool HasDefeatedLevel0;
    public bool HasDefeatedLevel1;
    public bool HasDefeatedLevel2;
    public bool RemovedCardFromDeck;
    public bool HasBoughtCardBazaar;
    public bool HasSoldCardBazaar;
}