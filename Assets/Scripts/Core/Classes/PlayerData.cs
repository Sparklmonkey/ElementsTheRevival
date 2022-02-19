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
    public string id;
    public Element markElement;
    public List<CardObject> currentDeck;
    public List<CardObject> cardInventory;
    public int electrum;
    public int gamesWon;
    public int gamesLost;
    public int playerScore;
    public int currentQuestIndex = 0;

    //Oracle Vars
    public string nextFalseGod;
    public string petName;
    public string username;
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
        //string path = Application.persistentDataPath + "/game_save/player.fun";
        //if (File.Exists(path))
        //{
        //    BinaryFormatter formatter = new BinaryFormatter();
        //    FileStream fileStream = new FileStream(path, FileMode.Open);

        //    shared = formatter.Deserialize(fileStream) as PlayerData;
        //    fileStream.Close();

        //    return true;
        //}
        //return false;
    }

    public static void UpdateOracleDay()
    {
        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.microsoft.com");
        var response = myHttpWebRequest.GetResponse();
        string todaysDates = response.Headers["date"];
        DateTime currentDate = DateTime.ParseExact(todaysDates,
                                   "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                                   CultureInfo.InvariantCulture.DateTimeFormat,
                                   DateTimeStyles.AssumeUniversal);
        Debug.Log(currentDate.Day);
        Debug.Log(shared.dayLastOraclePlay.Day);
        if(currentDate.Day > shared.dayLastOraclePlay.Day)
        {
            shared.playedOracleToday = false;
            shared.dayLastOraclePlay = currentDate;
        }

    }
}
