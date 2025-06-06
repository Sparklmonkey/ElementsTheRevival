using System.Threading.Tasks;

namespace Networking.Networking
{
    public interface ICloudSaveManager
    {
        Task<PlayerData> LoadPlayerData();
        Task SavePlayerData(PlayerData playerData);
        Task<PlayerData> ResetPlayerData();
    }
}