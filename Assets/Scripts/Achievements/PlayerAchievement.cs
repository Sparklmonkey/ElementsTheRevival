using System;

namespace Achievements
{
    [Serializable]
    public class PlayerAchievement
    {
        public string id;
        public string name;
        public string condition;
        public string description;
        public int completionPercent;
        public bool isAchieved;
        public int rarity;
        public int element;
    }
}