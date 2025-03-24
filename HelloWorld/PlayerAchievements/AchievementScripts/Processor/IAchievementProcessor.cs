using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.CloudCode.Apis;
using Unity.Services.CloudCode.Core;
using Unity.Services.CloudSave.Model;

namespace PlayerAchievements.AchievementScripts.Processor;

public interface IAchievementProcessor
{
    public Task<List<PlayerAchievement>> GetUpdatedAchievements(IExecutionContext ctx, string category);
}

public class AchievementProcessor : IAchievementProcessor
{
    private readonly string _playerAchievementKey = "PLAYER_ACHIEVEMENTS";
    private readonly string _playerSavedData = "SAVE_DATA";
    private IGameApiClient _gameApiClient;
    private AchievementList _achievementList;
    private CardDatabase _cardDatabase;
    
    public AchievementProcessor(AchievementList achievementList, CardDatabase cardDatabase, IGameApiClient gameApiClient) 
    {
        _achievementList = achievementList;
        _cardDatabase = cardDatabase;
        _gameApiClient = gameApiClient;
    }

    public async Task<List<PlayerAchievement>> GetUpdatedAchievements(IExecutionContext ctx, string category)
    {
        var achievements = _achievementList.GetAchievementByCategory();
        var response = new List<PlayerAchievement>();

        var playerAchievements = await GetPlayerAchievements(ctx);
        var playerSavedData = await GetPlayerSaveData(ctx);
        
        for (int i = 1; i <= achievements.Count; i++)
        {
            var achievement = achievements.First(x => x.Id == i);
            var completionScript = achievement.AchievementScript.GetScript<IAchievement>(category);
            var playerAchieve = completionScript.HasCompletedAchievement(achievement, playerSavedData, _cardDatabase);
            playerAchieve.Element = achievement.Element;
            playerAchieve.Category = achievement.Category;
            playerAchieve.Id = achievement.Id;
            var playerAchievement = playerAchievements.AchievementStatusList.Find(x => x.AchievementId == achievement.Id);
            if (playerAchievement is null) 
            {
                playerAchievements.AchievementStatusList.Add(new AchievementStatus
                {
                    AchievementId = achievement.Id,
                    AchievementTier = playerAchieve.TierAchieved,
                    RewardTierClaimed = -1
                });
            }
            else
            {
                playerAchievement.AchievementTier = playerAchieve.TierAchieved;
            }
            response.Add(playerAchieve);
        }

        await SavePlayerAchievements(ctx, playerAchievements);
    
        return response;
    }
    
    private async Task<PlayerAchievementStatus> GetPlayerAchievements(IExecutionContext ctx)
    {
        var cloudDataResponse = await _gameApiClient.CloudSaveData.GetItemsAsync(ctx,
            ctx.AccessToken, 
            ctx.ProjectId,
            ctx.PlayerId, 
            new List<string> { _playerAchievementKey });
        var cloudDataList = cloudDataResponse.Data.Results;
        if (cloudDataList.Count == 0)
        {
            return new();
        }

        var result = cloudDataList.First().Value.ToString();
        if (result == null) return new PlayerAchievementStatus();
        var achievementStatus =
            JsonConvert.DeserializeObject<PlayerAchievementStatus>(result);
        if (achievementStatus == null)  return new PlayerAchievementStatus();
        return achievementStatus;
    }
    private async Task<SavedData> GetPlayerSaveData(IExecutionContext ctx)
    {
        var cloudDataResponse = await _gameApiClient.CloudSaveData.GetItemsAsync(ctx,
            ctx.AccessToken, 
            ctx.ProjectId,
            ctx.PlayerId, 
            new List<string> { _playerSavedData });
        var cloudDataList = cloudDataResponse.Data.Results;
        if (cloudDataList.Count == 0)
        {
            return new();
        }

        var playerSavedData = JsonConvert.DeserializeObject<SavedData>(cloudDataList.First().Value.ToString());
        return playerSavedData ?? new();
    }
    
    
    private async Task SavePlayerAchievements(IExecutionContext ctx, PlayerAchievementStatus achievementStatusList)
    {
        await _gameApiClient.CloudSaveData.SetItemAsync(ctx,
            ctx.ServiceToken,
            ctx.ProjectId,
            ctx.PlayerId,
            new SetItemBody(_playerAchievementKey, achievementStatusList));
    }
}

public class AchievementStatus
{
    public int AchievementId { get; set; }
    public int AchievementTier { get; set; }
    public int RewardTierClaimed { get; set; }
}

public class PlayerAchievementStatus
{
    public List<AchievementStatus> AchievementStatusList { get; set; }

    public PlayerAchievementStatus()
    {
        AchievementStatusList = new List<AchievementStatus>();
    }
}