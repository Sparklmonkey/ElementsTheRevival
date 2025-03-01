namespace PlayerAchievements.AchievementScripts.Achievements.Combat.Easy;

// public class DefeatAllAiFiveEasy : IAchievement
// {
//     public PlayerAchievement HasCompletedAchievement(Achievement baseAchievement, PlayerData playerData,
//         CardDatabase cardDatabase)
//     {
//         var returnAchievement = new PlayerAchievement(baseAchievement);
//         
//         var aiZeroStats = playerData.GameStats.AiLevel5;
//         var completeCount = 0;
//         foreach (var stat in aiZeroStats)
//         {
//             if (stat.Wins > 0)
//             {
//                 completeCount += 1;
//             }
//         }
//         returnAchievement.IsAchieved = completeCount == aiZeroStats.Count;
//         returnAchievement.CompletionPercent = Convert.ToInt32(Convert.ToSingle(completeCount) / Convert.ToSingle(aiZeroStats.Count) * 100);
//         return returnAchievement;
//     }
// }