using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Unity.Services.CloudSave;
using UnityEngine;

namespace Networking.Networking
{
    public class CloudSaveManager : ICloudSaveManager
    {
        private readonly ICloudCodeManager _cloudCodeManager;

        public CloudSaveManager(ICloudCodeManager cloudCodeManager)
        {
            _cloudCodeManager = cloudCodeManager;
        }
        public async Task<PlayerData> LoadPlayerData()
        {
            var savedData =
                await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "SAVE_DATA" });

            if (savedData.TryGetValue("SAVE_DATA", out var value))
            {
                var data = value.Value;
                var score = await _cloudCodeManager.UpdateScore(0);
                SessionManager.Instance.PlayerScore = score;
                return data.GetAs<PlayerData>();
            }
            return null;
        }

        public async Task SavePlayerData(PlayerData playerData)
        {
            var data = new Dictionary<string, object> { { "SAVE_DATA", playerData } };
            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
        }

        public async Task<PlayerData> ResetPlayerData()
        {
            await SavePlayerData(new PlayerData());
            var points = await _cloudCodeManager.UpdateScore(0);
            SessionManager.Instance.PlayerScore = points;
            return await LoadPlayerData();
        }
        
    }
}