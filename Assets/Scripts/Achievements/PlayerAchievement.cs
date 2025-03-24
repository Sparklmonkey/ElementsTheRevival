using System;
using System.Collections.Generic;

namespace Achievements
{
    [Serializable]
    public class PlayerAchievement
    {
        public int Id;
        public string Name;
        public string Condition;
        public string Description;
        public int CompletionPercent;
        public int TierAchieved;
        public int Element;
        public int Category;
        public int GoldReward;
        public List<string> CardReward;
    }
}