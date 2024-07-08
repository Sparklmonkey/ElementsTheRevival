using System;
using System.Collections.Generic;

namespace Achievements
{
    [Serializable]
    public class PlayerAchievement
    {
        public int id;
        public string name;
        public string condition;
        public string description;
        public int completionPercent;
        public int tierAchieved;
        public int element;
        public int goldReward;
        public List<string> cardReward;
        public bool isSelectCardReward;
    }
}