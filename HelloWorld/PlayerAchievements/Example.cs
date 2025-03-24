using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PlayerAchievements.AchievementScripts;
using PlayerAchievements.AchievementScripts.Processor;
using Unity.Services.CloudCode.Apis;
using Unity.Services.CloudCode.Core;
using Unity.Services.Lobby.Model;

namespace PlayerAchievements;

public class MyModule
{
    public MyModule(IAchievementProcessor achievementProcessor)
    {
        _achievementProcessor = achievementProcessor;
    }
    private IAchievementProcessor _achievementProcessor;
    
    [CloudCodeFunction("SayHello")]
    public string Hello(string name)
    {
        return $"Hello, {name}!";
    }

    [CloudCodeFunction("GetAchievementsByCategory")]
    public async Task<string> GetAchievementsByCategory(string category, IExecutionContext context)
    {
        var response = await _achievementProcessor.GetUpdatedAchievements(context, category);
        return JsonConvert.SerializeObject(response);
    }
}


