using System;
using System.Collections.Generic;
using System.IO;
using Networking;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public static PlayerData Shared;
    public int id;
    public Element markElement;
    public List<string> currentDeck;
    public Element arenaT50Mark;
    public List<string> arenaT50Deck;
    public List<string> inventoryCards;
    public int electrum;
    public int gamesWon;
    public int gamesLost;
    public int arenaWins;
    public int arenaLosses;
    public int playerScore;
    public string userName = "";
    public string email = "";
    public string completedQuests = "";

    internal void ResetAccount()
    {
        markElement = Element.Aether;
        currentDeck = new();
        inventoryCards = new();
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
        DayLastOraclePlay = DateTime.Now;
        hasDefeatedLevel0 = false;
        removedCardFromDeck = false;
        hasBoughtCardBazaar = false;
        hasSoldCardBazaar = false;
        hasDefeatedLevel1 = false;
        hasDefeatedLevel2 = false;
        arenaT50Deck = new();
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
    public DateTime DayLastOraclePlay;

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
        currentDeck = new();
        inventoryCards = new();
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
        DayLastOraclePlay = DateTime.Now;
        hasDefeatedLevel0 = false;
        removedCardFromDeck = false;
        hasBoughtCardBazaar = false;
        hasSoldCardBazaar = false;
        hasDefeatedLevel1 = false;
        hasDefeatedLevel2 = false;
        arenaT50Deck = new();
        arenaT50Mark = Element.Aether;
    }

    public void ClearIllegalCards()
    {
        currentDeck.RemoveAll(x => x == "6ro");
        currentDeck.RemoveAll(x => x == "4t8");
        currentDeck.RemoveAll(x => x == "4vr");
        currentDeck.RemoveAll(x => x == "6ub");
        inventoryCards.RemoveAll(x => x == "6ro");
        inventoryCards.RemoveAll(x => x == "4t8");
        inventoryCards.RemoveAll(x => x == "4vr");
        inventoryCards.RemoveAll(x => x == "6ub");
        arenaT50Deck.RemoveAll(x => x == "6ro");
        arenaT50Deck.RemoveAll(x => x == "4t8");
        arenaT50Deck.RemoveAll(x => x == "4vr");
        arenaT50Deck.RemoveAll(x => x == "6ub");
    }
}

[Serializable]
public class DeckPreset
{
    public string deckName;
    public string deckCode;
}
