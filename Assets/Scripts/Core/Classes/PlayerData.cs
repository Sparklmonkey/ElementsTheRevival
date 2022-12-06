using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Globalization;

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
    public int currentQuestIndex = 0;

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

    public List<string> redeemedCodes = new List<string>();

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
        currentDeck = new List<string>();
        cardInventory = new List<string>();
        electrum = 0;
        gamesWon = 0;
        gamesLost = 0;
        playerScore = 0;
        currentQuestIndex = 0;
        nextFalseGod = "";
        petName = "";
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

public class GameStats
{
    public int level1;
    public int level2;
    public int level3;
    public HalfBlood Aether;
    public HalfBlood Air;
    public HalfBlood Darkness;
    public HalfBlood Death;
    public HalfBlood Earth;
    public HalfBlood Entropy;
    public HalfBlood Gravity;
    public HalfBlood Fire;
    public HalfBlood Life;
    public HalfBlood Light;
    public HalfBlood Time;
    public HalfBlood Water;
    //Half Blood



    //False Gods
}

[Serializable]
public class FalseGodsStats
{

    public int DivineGlory = 0;
    public int Serket = 0;
    public int Morte = 0;
    public int Lionheart = 0;
    public int Incarnate = 0;
    public int FireQueen = 0;
    public int Seism = 0;
    public int Miracle = 0;
    public int Graviton = 0;
    public int Paradox = 0;
    public int Akebono = 0;
    public int Neptune = 0;
    public int Scorpio = 0;
    public int Osiris = 0;
    public int Octane = 0;
    public int Rainbow = 0;
    public int Obliterator = 0;
    public int Gemini = 0;
    public int ChaosLord = 0;
    public int DarkMatter = 0;
    public int Decay = 0;
    public int Destiny = 0;
    public int DreamCatcher = 0;
    public int Elidnis = 0;
    public int EternalPhoenix = 0;
    public int Ferox = 0;
    public int Hecate = 0;
    public int Hermes = 0;
    public int Jezebel = 0;

    public int GetAgregateScore()
    {
        return DivineGlory + Serket + Morte
    + Lionheart + Incarnate + FireQueen + Seism + Miracle + Graviton + Paradox + Akebono + Neptune + Scorpio
    + Osiris + Octane + Rainbow + Obliterator + Gemini + ChaosLord + DarkMatter + Decay + Destiny
    + DreamCatcher + Elidnis + EternalPhoenix + Ferox + Hecate + Hermes + Jezebel;
    }
}

[Serializable]
public class HalfBlood
{
    public int AetherWins = 0;
    public int AirWins = 0;
    public int DarknessWins = 0;
    public int DeathWins = 0;
    public int EarthWins = 0;
    public int EntropyWins = 0;
    public int GravityWins = 0;
    public int FireWins = 0;
    public int LifeWins = 0;
    public int LightWins = 0;
    public int TimeWins = 0;
    public int WaterWins = 0;
    public int AetherLoses = 0;
    public int AirLoses = 0;
    public int DarknessLoses = 0;
    public int DeathLoses = 0;
    public int EarthLoses = 0;
    public int EntropyLoses = 0;
    public int GravityLoses = 0;
    public int FireLoses = 0;
    public int LifeLoses = 0;
    public int LightLoses = 0;
    public int TimeLoses = 0;
    public int WaterLoses = 0;
}