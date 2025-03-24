using Microsoft.Extensions.DependencyInjection;
using PlayerAchievements.AchievementScripts.Processor;
using Unity.Services.CloudCode.Apis;
using Unity.Services.CloudCode.Core;

namespace PlayerAchievements.AchievementScripts;


public class CloudCodeSetup : ICloudCodeSetup
{
    public void Setup(ICloudCodeConfig config)
    {
        config.Dependencies.AddSingleton(new CardDatabase());
        config.Dependencies.AddSingleton(new AchievementList());
        config.Dependencies.AddSingleton(GameApiClient.Create());
        config.Dependencies.AddSingleton<IAchievementProcessor, AchievementProcessor>();
    }
}