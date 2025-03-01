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
    public int Id;
    [FormerlySerializedAs("markElement")] public Element MarkElement;
    [FormerlySerializedAs("currentDeck")] public string CurrentDeck;
    [FormerlySerializedAs("arenaT50Mark")] public Element ArenaT50Mark;
    [FormerlySerializedAs("arenaT50Deck")] public string ArenaT50Deck;
    [FormerlySerializedAs("inventoryCards")] public string InventoryCards;
    [FormerlySerializedAs("electrum")] public int Electrum;
    [FormerlySerializedAs("gamesWon")] public int GamesWon;
    [FormerlySerializedAs("gamesLost")] public int GamesLost;
    [FormerlySerializedAs("arenaWins")] public int ArenaWins;
    [FormerlySerializedAs("arenaLosses")] public int ArenaLosses;
    [FormerlySerializedAs("playerScore")] public int PlayerScore;
    [FormerlySerializedAs("username")] public string Username = "";
    [FormerlySerializedAs("email")] public string Email = "";
    public string CompletedQuests = "";
    internal void ResetAccount()
    {
        MarkElement = Element.Aether;
        CurrentDeck = "";
        InventoryCards = "";
        SavedDecks = new();
        Electrum = 0;
        GamesWon = 0;
        GamesLost = 0;
        PlayerScore = 0;
        CurrentQuestIndex = 0;
        NextFalseGod = "";
        PetName = "";
        CompletedQuests = "";
        PetCount = 0;
        PlayedOracleToday = false;
        OracleLastPlayed = DateTime.Today.ToString("yyyy-MM-dd'T'HH:mm:ss.fffffffzzz");
        LastOracleDay = 0;
        HasDefeatedLevel0 = false;
        RemovedCardFromDeck = false;
        HasBoughtCardBazaar = false;
        HasSoldCardBazaar = false;
        HasDefeatedLevel1 = false;
        HasDefeatedLevel2 = false;
        ArenaT50Deck = "";
        ArenaT50Mark = Element.Aether;
    }

    [FormerlySerializedAs("currentQuestIndex")] public int CurrentQuestIndex = 0;
    //Saved Decks
    [FormerlySerializedAs("savedDecks")] public List<string> SavedDecks;
    //Oracle Vars
    [FormerlySerializedAs("nextFalseGod")] public string NextFalseGod;
    [FormerlySerializedAs("petName")] public string PetName;
    [FormerlySerializedAs("petCount")] public int PetCount;
    //public string username;
    [FormerlySerializedAs("playedOracleToday")] public bool PlayedOracleToday;
    
    [FormerlySerializedAs("oracleLastPlayed")] public string OracleLastPlayed;
    [FormerlySerializedAs("lastOracleDay")] public int LastOracleDay;

    //Quest 1 Flag
    [FormerlySerializedAs("hasDefeatedLevel0")] public bool HasDefeatedLevel0;

    //Quest 2 Flag
    [FormerlySerializedAs("removedCardFromDeck")] public bool RemovedCardFromDeck;

    //Quest 3 Flags
    [FormerlySerializedAs("hasBoughtCardBazaar")] public bool HasBoughtCardBazaar;

    [FormerlySerializedAs("hasSoldCardBazaar")] public bool HasSoldCardBazaar;

    //Quest 4 Flag
    [FormerlySerializedAs("hasDefeatedLevel1")] public bool HasDefeatedLevel1;

    //Quest 5 Flag
    [FormerlySerializedAs("hasDefeatedLevel2")] public bool HasDefeatedLevel2;

    public static void LoadFromApi(PlayerData playerData)
    {
        Shared = playerData;
    }
    
    public List<string> GetDeck() => CurrentDeck.ConvertCardCodeToList();
    public List<string> GetInventory() => InventoryCards.ConvertCardCodeToList();
    public void SetInventory(List<string> cardList) => InventoryCards = cardList.ConvertListToCardCode();
    public void SetDeck(List<string> cardList) => CurrentDeck = cardList.ConvertListToCardCode();
    public List<string> GetArenaTFifty() => ArenaT50Deck.ConvertCardCodeToList();
    public void SetArenaTFifty(List<string> cardList) => ArenaT50Deck = cardList.ConvertListToCardCode();
    

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
        MarkElement = Element.Aether;
        CurrentDeck = "X";
        InventoryCards = "X";
        SavedDecks = new();
        Electrum = 0;
        GamesWon = 0;
        GamesLost = 0;
        PlayerScore = 0;
        CurrentQuestIndex = 0;
        NextFalseGod = "";
        PetName = "";
        CompletedQuests = "";
        PetCount = 0;
        PlayedOracleToday = false; //2024-02-03T00:21:49.2289075-03:00
        OracleLastPlayed = DateTime.Today.ToString("yyyy-MM-dd'T'HH:mm:ss.fffffffzzz");
        LastOracleDay = 0;
        HasDefeatedLevel0 = false;
        RemovedCardFromDeck = false;
        HasBoughtCardBazaar = false;
        HasSoldCardBazaar = false;
        HasDefeatedLevel1 = false;
        HasDefeatedLevel2 = false;
        ArenaT50Deck = "";
        ArenaT50Mark = Element.Aether;
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
