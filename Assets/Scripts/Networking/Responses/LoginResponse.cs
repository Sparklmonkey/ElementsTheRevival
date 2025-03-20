using System;
using System.Collections.Generic;

namespace Networking
{
    [Serializable]
    public class LoginResponse
    {
        public string sername;
        public string accessToken;
        public string emailAddress;
        public PlayerDataLegacy savedData;
        public ErrorCases ErrorMessage;
        public string token;
        public string accountId;
    }
}

[Serializable]
public class PlayerDataLegacy
{
    public static PlayerDataLegacy shared;
    public int id;
    public Element markElement;
    public string currentDeck;
    public Element arenaT50Mark;
    public string arenaT50Deck;
    public string inventoryCards;
    public int electrum;
    public int gamesWon;
    public int gamesLost;
    public int arenaWins;
    public int arenaLosses;
    public int playerScore;
    public string username = "";
    public string email = "";
    public string completedQuests = "";
    public int currentQuestIndex = 0;
    public List<string> savedDecks;
    public string nextFalseGod;
    public string petName;
    public int petCount;
    public bool playedOracleToday;
    public string oracleLastPlayed;
    public int lastOracleDay;
    public bool hasDefeatedLevel0;
    public bool removedCardFromDeck;
    public bool hasBoughtCardBazaar;
    public bool hasSoldCardBazaar;
    public bool hasDefeatedLevel1;
    public bool hasDefeatedLevel2;

    public PlayerDataLegacy()
    {
        id = 0;
        markElement = Element.Aether;
        currentDeck = "X";
        inventoryCards = "X";
        savedDecks = new();
        electrum = 0;
        gamesWon = 0;
        gamesLost = 0;
        playerScore = 0;
        currentQuestIndex = 0;
        nextFalseGod = "";
        petName = "";
        completedQuests = "";
        petCount = 0;
        playedOracleToday = false; //2024-02-03T00:21:49.2289075-03:00
        oracleLastPlayed = DateTime.Today.ToString("yyyy-MM-dd'T'HH:mm:ss.fffffffzzz");
        lastOracleDay = 0;
        hasDefeatedLevel0 = false;
        removedCardFromDeck = false;
        hasBoughtCardBazaar = false;
        hasSoldCardBazaar = false;
        hasDefeatedLevel1 = false;
        hasDefeatedLevel2 = false;
        arenaT50Deck = "";
        arenaT50Mark = Element.Aether;
    }
}
