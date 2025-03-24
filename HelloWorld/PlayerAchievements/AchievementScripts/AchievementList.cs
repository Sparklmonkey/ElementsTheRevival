using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace PlayerAchievements.AchievementScripts;

public class AchievementList
{
  private string _collectionAchievements = @"{ ""AchievementList"": [
    {
        ""Id"": 2,
        ""BronzeTier"": {
            ""Title"": ""Aeolic Oenophile"",
            ""Description"": ""Collect 1 of each card of the Element of Air. Upgraded cards are counted separately."",
            ""Rarity"": 1,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""GoldTier"": {
            ""Title"": ""Aeolic Connoisseur"",
            ""Description"": ""Collect 3 of each card of the Element of Air. Upgraded cards are counted separately."",
            ""Rarity"": 2,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""PlatinumTier"": {
            ""Title"": ""Aeolic Sommelier"",
            ""Description"": ""Collect 6 of each card of the Element of Air. Upgraded cards are counted separately."",
            ""Rarity"": 2,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""Element"": 1,
        ""AchievementScript"": ""AchievementCollectAir"",
        ""Category"": 0
    },
    {
        ""Id"": 3,
        ""BronzeTier"": {
            ""Title"": ""Shadow Oenophile"",
            ""Description"": ""Collect 1 of each card of the Element of Darkness. Upgraded cards are counted separately."",
            ""Rarity"": 1,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""GoldTier"": {
            ""Title"": ""Shadow Connoisseur"",
            ""Description"": ""Collect 3 of each card of the Element of Darkness. Upgraded cards are counted separately."",
            ""Rarity"": 2,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""PlatinumTier"": {
            ""Title"": ""Shadow Sommelier"",
            ""Description"": ""Collect 6 of each card of the Element of Darkness. Upgraded cards are counted separately."",
            ""Rarity"": 2,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""Element"": 2,
        ""AchievementScript"": ""AchievementCollectDarkness"",
        ""Category"": 0
    },
    {
        ""Id"": 4,
        ""BronzeTier"": {
            ""Title"": ""Holy Oenophile"",
            ""Description"": ""Collect 1 of each card of the Element of Light. Upgraded cards are counted separately."",
            ""Rarity"": 1,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""GoldTier"": {
            ""Title"": ""Holy Connoisseur"",
            ""Description"": ""Collect 3 of each card of the Element of Light. Upgraded cards are counted separately."",
            ""Rarity"": 2,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""PlatinumTier"": {
            ""Title"": ""Holy Sommelier"",
            ""Description"": ""Collect 6 of each card of the Element of Light. Upgraded cards are counted separately."",
            ""Rarity"": 2,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""Element"": 3,
        ""AchievementScript"": ""AchievementCollectLight"",
        ""Category"": 0
    },
    {
        ""Id"": 5,
        ""BronzeTier"": {
            ""Title"": ""Undead Oenophile"",
            ""Description"": ""Collect 1 of each card of the Element of Death. Upgraded cards are counted separately."",
            ""Rarity"": 1,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""GoldTier"": {
            ""Title"": ""Undead Connoisseur"",
            ""Description"": ""Collect 3 of each card of the Element of Death. Upgraded cards are counted separately."",
            ""Rarity"": 2,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""PlatinumTier"": {
            ""Title"": ""Undead Sommelier"",
            ""Description"": ""Collect 6 of each card of the Element of Death. Upgraded cards are counted separately."",
            ""Rarity"": 2,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""Element"": 4,
        ""AchievementScript"": ""AchievementCollectDeath"",
        ""Category"": 0
    },
    {
        ""Id"": 6,
        ""BronzeTier"": {
            ""Title"": ""Dirt Oenophile"",
            ""Description"": ""Collect 1 of each card of the Element of Earth. Upgraded cards are counted separately."",
            ""Rarity"": 1,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""GoldTier"": {
            ""Title"": ""Dirt Connoisseur"",
            ""Description"": ""Collect 3 of each card of the Element of Earth. Upgraded cards are counted separately."",
            ""Rarity"": 2,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""PlatinumTier"": {
            ""Title"": ""Dirt Sommelier"",
            ""Description"": ""Collect 6 of each card of the Element of Earth. Upgraded cards are counted separately."",
            ""Rarity"": 2,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""Element"": 5,
        ""AchievementScript"": ""AchievementCollectEarth"",
        ""Category"": 0
    },
    {
        ""Id"": 7,
        ""BronzeTier"": {
            ""Title"": ""Chaos Oenophile"",
            ""Description"": ""Collect 1 of each card of the Element of Entropy. Upgraded cards are counted separately."",
            ""Rarity"": 1,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""GoldTier"": {
            ""Title"": ""Chaos Connoisseur"",
            ""Description"": ""Collect 3 of each card of the Element of Entropy. Upgraded cards are counted separately."",
            ""Rarity"": 2,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""PlatinumTier"": {
            ""Title"": ""Chaos Sommelier"",
            ""Description"": ""Collect 6 of each card of the Element of Entropy. Upgraded cards are counted separately."",
            ""Rarity"": 2,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""Element"": 6,
        ""AchievementScript"": ""AchievementCollectEntropy"",
        ""Category"": 0
    },
    {
        ""Id"": 8,
        ""BronzeTier"": {
            ""Title"": ""Temporal Oenophile"",
            ""Description"": ""Collect 1 of each card of the Element of Time. Upgraded cards are counted separately."",
            ""Rarity"": 1,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""GoldTier"": {
            ""Title"": ""Temporal Connoisseur"",
            ""Description"": ""Collect 3 of each card of the Element of Time. Upgraded cards are counted separately."",
            ""Rarity"": 2,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""PlatinumTier"": {
            ""Title"": ""Temporal Sommelier"",
            ""Description"": ""Collect 6 of each card of the Element of Time. Upgraded cards are counted separately."",
            ""Rarity"": 2,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""Element"": 7,
        ""AchievementScript"": ""AchievementCollectTime"",
        ""Category"": 0
    },
    {
        ""Id"": 10,
        ""BronzeTier"": {
            ""Title"": ""Mass Oenophile"",
            ""Description"": ""Collect 1 of each card of the Element of Gravity. Upgraded cards are counted separately."",
            ""Rarity"": 1,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""GoldTier"": {
            ""Title"": ""Mass Connoisseur"",
            ""Description"": ""Collect 3 of each card of the Element of Gravity. Upgraded cards are counted separately."",
            ""Rarity"": 2,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""PlatinumTier"": {
            ""Title"": ""Mass Sommelier"",
            ""Description"": ""Collect 6 of each card of the Element of Gravity. Upgraded cards are counted separately."",
            ""Rarity"": 2,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""Element"": 9,
        ""AchievementScript"": ""AchievementCollectGravity"",
        ""Category"": 0
    },
    {
        ""Id"": 11,
        ""BronzeTier"": {
            ""Title"": ""Life Oenophile"",
            ""Description"": ""Collect 1 of each card of the Element of Life. Upgraded cards are counted separately."",
            ""Rarity"": 1,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""GoldTier"": {
            ""Title"": ""Life Connoisseur"",
            ""Description"": ""Collect 3 of each card of the Element of Life. Upgraded cards are counted separately."",
            ""Rarity"": 2,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""PlatinumTier"": {
            ""Title"": ""Life Sommelier"",
            ""Description"": ""Collect 6 of each card of the Element of Life. Upgraded cards are counted separately."",
            ""Rarity"": 2,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""Element"": 10,
        ""AchievementScript"": ""AchievementCollectLife"",
        ""Category"": 0
    },
    {
        ""Id"": 12,
        ""BronzeTier"": {
            ""Title"": ""Aqua Oenophile"",
            ""Description"": ""Collect 1 of each card of the Element of Water. Upgraded cards are counted separately."",
            ""Rarity"": 1,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""GoldTier"": {
            ""Title"": ""Aqua Connoisseur"",
            ""Description"": ""Collect 3 of each card of the Element of Water. Upgraded cards are counted separately."",
            ""Rarity"": 2,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""PlatinumTier"": {
            ""Title"": ""Aqua Sommelier"",
            ""Description"": ""Collect 6 of each card of the Element of Water. Upgraded cards are counted separately."",
            ""Rarity"": 2,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""Element"": 11,
        ""AchievementScript"": ""AchievementCollectWater"",
        ""Category"": 0
    },
    {
        ""Id"": 13,
        ""BronzeTier"": {
            ""Title"": ""Chromatic Oenophile"",
            ""Description"": ""Collect 1 of each card of the Element of Chroma. Upgraded cards are counted separately."",
            ""Rarity"": 1,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""GoldTier"": {
            ""Title"": ""Chromatic Connoisseur"",
            ""Description"": ""Collect 3 of each card of the Element of Chroma. Upgraded cards are counted separately."",
            ""Rarity"": 2,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""PlatinumTier"": {
            ""Title"": ""Chromatic Sommelier"",
            ""Description"": ""Collect 6 of each card of the Element of Chroma. Upgraded cards are counted separately."",
            ""Rarity"": 2,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""Element"": 12,
        ""AchievementScript"": ""AchievementCollectOther"",
        ""Category"": 0
    },
    {
        ""Id"": 1,
        ""BronzeTier"": {
            ""Title"": ""Creation Oenophile"",
            ""Description"": ""Collect 1 of each card of the Element of Aether. Upgraded cards are counted separately."",
            ""Rarity"": 1,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""GoldTier"": {
            ""Title"": ""Creation Connoisseur"",
            ""Description"": ""Collect 3 of each card of the Element of Aether. Upgraded cards are counted separately."",
            ""Rarity"": 2,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""PlatinumTier"": {
            ""Title"": ""Creation Sommelier"",
            ""Description"": ""Collect 6 of each card of the Element of Aether. Upgraded cards are counted separately."",
            ""Rarity"": 2,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""Element"": 0,
        ""AchievementScript"": ""AchievementCollectAether"",
        ""Category"": 0
    },
    {
        ""Id"": 9,
        ""BronzeTier"": {
            ""Title"": ""Crispy Oenophile"",
            ""Description"": ""Collect 1 of each card of the Element of Fire. Upgraded cards are counted separately."",
            ""Rarity"": 1,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""GoldTier"": {
            ""Title"": ""Crispy Connoisseur"",
            ""Description"": ""Collect 3 of each card of the Element of Fire. Upgraded cards are counted separately."",
            ""Rarity"": 2,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""PlatinumTier"": {
            ""Title"": ""Crispy Sommelier"",
            ""Description"": ""Collect 6 of each card of the Element of Fire. Upgraded cards are counted separately."",
            ""Rarity"": 2,
            ""GoldReward"": 250,
            ""CardReward"": """"
        },
        ""Element"": 8,
        ""AchievementScript"": ""AchievementCollectFire"",
        ""Category"": 0
    }
]}";

  public List<NewAchievement> GetAchievementByCategory()
  {
    var achievementList = JsonConvert.DeserializeObject<AchievementListObject>(_collectionAchievements);
    return achievementList != null ? achievementList.AchievementList.OrderBy(x => x.Id).ToList() : new List<NewAchievement>();
  }
}


public class AchievementListObject
{
    public List<NewAchievement> AchievementList { get; set; }
}
