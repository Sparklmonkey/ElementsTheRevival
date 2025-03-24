using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Achievements;
using Networking;
using Unity.Services.CloudCode.GeneratedBindings;
using UnityEngine;

namespace Core
{
    public class SessionManager
    {
        public static SessionManager Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new SessionManager();
                }

                return _instance;
            }
        }

        private static SessionManager _instance;

        public bool ShouldHideConfirm = false;
        public List<GameNews> GameNews = new();
        public List<PlayerAchievement> Achievements = new();
        public ScoreUpdateResponse PlayerScore;
        private PlayerAchievementsBindings _playerAchievementsBindings;

        public async Task GetPlayerAchievements()
        {
            var saveData = JsonUtility.ToJson(PlayerData.Shared);
            try
            {
                var response = await _playerAchievementsBindings.GetAchievementsByCategory("Collection", saveData);
                Achievements = JsonHelper.FromJson<PlayerAchievement>(response.FixJson()).ToList();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        
        public async void SetPlayerAchievementModule(PlayerAchievementsBindings playerAchievementsBindings)
        {
            _playerAchievementsBindings = playerAchievementsBindings;
            var result = await _playerAchievementsBindings.SayHello("World");
            Debug.Log(result);
        }
    }
    
    
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}