using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public static PlayerData shared;
    public int id;
    public Element markElement;
    public List<string> currentDeck;
    public Element arenaT50Mark;
    public List<string> arenaT50Deck;
    public List<string> cardInventory;
    public int electrum;
    public int gamesWon;
    public int gamesLost;
    public int arenaWins;
    public int arenaLosses;
    public int playerScore;
    public string completedQuests = "";

    internal void ResetAccount()
    {
        markElement = Element.Aether;
        currentDeck = new();
        cardInventory = new();
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
        playedOracleToday = false;
        dayLastOraclePlay = DateTime.Now;
        hasDefeatedLevel0 = false;
        removedCardFromDeck = false;
        hasBoughtCardBazaar = false;
        hasSoldCardBazaar = false;
        hasDefeatedLevel1 = false;
        hasDefeatedLevel2 = false;
    }

    public int currentQuestIndex = 0;
    //Saved Decks
    public List<string> savedDecks;
    //Oracle Vars
    public string nextFalseGod;
    public string petName;
    public int petCount;
    //public string username;
    public bool playedOracleToday;
    public DateTime dayLastOraclePlay;

    //Quest 1 Flag
    public bool hasDefeatedLevel0;

    //Quest 2 Flag
    public bool removedCardFromDeck;

    //Quest 3 Flags
    public bool hasBoughtCardBazaar;


    public bool hasSoldCardBazaar;

    //Quest 4 Flag
    public bool hasDefeatedLevel1;

    //Quest 5 Flag
    public bool hasDefeatedLevel2;

    public List<string> redeemedCodes = new();

    public int gameStatsId;

    public static void LoadFromApi(PlayerData playerData)
    {
        shared = playerData;
    }

    public static bool HasSaveFile()
    {
        return Directory.Exists(Application.persistentDataPath + "/game_save");
    }

    public static void DeleteSaveData()
    {
        File.Delete(Application.persistentDataPath + "/game_save/player.fun");
    }


    public static void SaveData()
    {
        if(ApiManager.isTrainer) { return; }
        PlayerPrefs.SetString("SaveData", JsonUtility.ToJson(shared));
    }

    public static bool LoadData()
    {
        if (PlayerPrefs.HasKey("SaveData"))
        {
            shared = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("SaveData"));
            return true;
        }
        return false;
    }

    public PlayerData()
    {

        markElement = Element.Aether;
        currentDeck = new();
        cardInventory = new();
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
        playedOracleToday = false;
        dayLastOraclePlay = DateTime.Now;
        hasDefeatedLevel0 = false;
        removedCardFromDeck = false;
        hasBoughtCardBazaar = false;
        hasSoldCardBazaar = false;
        hasDefeatedLevel1 = false;
        hasDefeatedLevel2 = false;
    }

}

[Serializable]
public class DeckPreset
{
    public string deckName;
    public string deckCode;
}
