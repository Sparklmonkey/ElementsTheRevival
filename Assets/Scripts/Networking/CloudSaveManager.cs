using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;

namespace Networking.Networking
{
    public class CloudSaveManager : ICloudSaveManager
    {
        public async Task<PlayerData> LoadPlayerData()
        {
            var savedData =
                await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "SAVE_DATA" });
            
            var data = savedData["SAVE_DATA"].Value;
            return data.GetAs<PlayerData>();
        }

        public async Task SavePlayerData(PlayerData playerData)
        {
            var data = new Dictionary<string, object> { { "SAVE_DATA", playerData } };
            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
        }

        public async Task<PlayerData> ResetPlayerData()
        {
            await SavePlayerData(new PlayerData());
            return await LoadPlayerData();
        }
        
    }
}