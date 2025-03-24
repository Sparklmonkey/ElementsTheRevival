using System;
using System.Collections.Generic;
using System.IO;
using Networking;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public static PlayerData Shared;
    public int Id;
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
    internal void ResetAccount()
    {
        Id = 0;
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

    public int CurrentQuestIndex = 0;
    //Saved Decks
    public List<string> SavedDecks;
    //Oracle Vars
    public string NextFalseGod;
    public string PetName;
    public int PetCount;
    //public string username;
    public bool PlayedOracleToday;
    
    public string OracleLastPlayed;
    public int LastOracleDay;

    //Quest 1 Flag
    public bool HasDefeatedLevel0;

    //Quest 2 Flag
    public bool RemovedCardFromDeck;

    //Quest 3 Flags
    public bool HasBoughtCardBazaar;

    public bool HasSoldCardBazaar;

    //Quest 4 Flag
    public bool HasDefeatedLevel1;

    //Quest 5 Flag
    public bool HasDefeatedLevel2;

    public static void LoadFromApi(PlayerData playerData)
    {
        Shared = playerData;
    }
    
    public static void LoadFromApi(PlayerDataLegacy playerData)
    {
        Shared = new PlayerData(playerData);
    }

    public PlayerData(PlayerDataLegacy legacy)
    {
        Id = legacy.id;
        MarkElement = legacy.markElement;
        CurrentDeck = legacy.currentDeck;
        InventoryCards = legacy.inventoryCards;
        SavedDecks = legacy.savedDecks;
        Electrum = legacy.electrum;
        GamesWon = legacy.gamesWon;
        GamesLost = legacy.gamesLost;
        PlayerScore = legacy.playerScore;
        CurrentQuestIndex = legacy.currentQuestIndex;
        NextFalseGod = legacy.nextFalseGod;
        PetName = legacy.petName;
        CompletedQuests = legacy.completedQuests;
        PetCount = legacy.petCount;
        PlayedOracleToday = false; //2024-02-03T00:21:49.2289075-03:00
        OracleLastPlayed = DateTime.Today.ToString("yyyy-MM-dd'T'HH:mm:ss.fffffffzzz");
        LastOracleDay = legacy.lastOracleDay;
        HasDefeatedLevel0 = legacy.hasDefeatedLevel0;
        RemovedCardFromDeck = legacy.removedCardFromDeck;
        HasBoughtCardBazaar = legacy.hasBoughtCardBazaar;
        HasSoldCardBazaar = legacy.hasSoldCardBazaar;
        HasDefeatedLevel1 = legacy.hasDefeatedLevel1;
        HasDefeatedLevel2 = legacy.hasDefeatedLevel2;
        ArenaT50Deck = legacy.arenaT50Deck;
        ArenaT50Mark = legacy.arenaT50Mark;
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
        Id = 0;
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