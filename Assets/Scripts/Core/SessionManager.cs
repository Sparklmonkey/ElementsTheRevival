using System.Collections.Generic;
using System.Threading.Tasks;
using Achievements;
using Networking;
using Newtonsoft.Json;
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
            var response = await _playerAchievementsBindings.GetAchievementsByCategory("Collection", saveData);
            Achievements = JsonConvert.DeserializeObject<List<PlayerAchievement>>(response);
        }
        
        public async void SetPlayerAchievementModule(PlayerAchievementsBindings playerAchievementsBindings)
        {
            _playerAchievementsBindings = playerAchievementsBindings;
            var result = await _playerAchievementsBindings.SayHello("World");
            Debug.Log(result);
        }
    }
}