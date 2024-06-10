using System;
using System.Collections.Generic;
using System.IO;
using Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class PlayerData
{
    public static PlayerData Shared;
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

    internal void ResetAccount()
    {
        markElement = Element.Aether;
        currentDeck = "";
        inventoryCards = "";
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

    public int currentQuestIndex = 0;
    //Saved Decks
    public List<string> savedDecks;
    //Oracle Vars
    public string nextFalseGod;
    public string petName;
    public int petCount;
    //public string username;
    public bool playedOracleToday;
    
    public string oracleLastPlayed;
    public int lastOracleDay;

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

    public static void LoadFromApi(PlayerData playerData)
    {
        Shared = playerData;
    }
    
    public List<string> GetDeck() => currentDeck.ConvertCardCodeToList();
    public List<string> GetInventory() => inventoryCards.ConvertCardCodeToList();
    public void SetInventory(List<string> cardList) => inventoryCards = cardList.ConvertListToCardCode();
    public void SetDeck(List<string> cardList) => currentDeck = cardList.ConvertListToCardCode();
    public List<string> GetArenaTFifty() => arenaT50Deck.ConvertCardCodeToList();
    public void SetArenaTFifty(List<string> cardList) => arenaT50Deck = cardList.ConvertListToCardCode();
    

    public static bool LoadData()
    {
        if (!PlayerPrefs.HasKey("SaveData")) return false;
        Shared = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("SaveData"));
        return true;
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
        if (ApiManager.IsTrainer) { return; }
        PlayerPrefs.SetString("SaveData", JsonUtility.ToJson(Shared));
    }

    public PlayerData()
    {
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

[Serializable]
public class DeckPreset
{
    public string deckName;
    public string deckCode;
}

public class CustomDateTimeConverter : IsoDateTimeConverter
{
    public CustomDateTimeConverter()
    {
        base.DateTimeFormat = "dd/MM/yyyy";
    }
}
