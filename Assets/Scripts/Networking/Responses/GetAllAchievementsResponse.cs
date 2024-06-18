using System;
using System.Collections.Generic;
using Achievements;

namespace Networking
{
    [Serializable]
    public class GetAchievementsResponse
    {
        public List<PlayerAchievement> achievements;
    }
}