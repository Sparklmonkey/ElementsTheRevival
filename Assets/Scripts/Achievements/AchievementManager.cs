using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager
{
    private static readonly AchievementManager instance = new();

    public PlayerAchievements achievements;

    static AchievementManager()
    {
    }

    private AchievementManager()
    {
    }

    public static AchievementManager Instance
    {
        get
        {
            return instance;
        }
    }



}


public class AchievementIdleYet : AchievementBiolerplate
{
    public override string AchievementProperty => "AchievementIdleYet";

    public override string AchievementName => "Are we idle yet?";

    public override string AchievementDescription => "Take longer than 3 minutes to win";

}

public class AchievementCreatureDomination : AchievementBiolerplate
{
    public override string AchievementProperty => "AchievementCreatureDomination";

    public override string AchievementName => "Creature Domination";

    public override string AchievementDescription => "More than twice as many creatures than foe";
}

public class AchievementCreatureless : AchievementBiolerplate
{
    public override string AchievementProperty => "AchievementCreatureless";

    public override string AchievementName => "Creatureless";

    public override string AchievementDescription => "Never play a creature";
}

public class AchievementDeckout : AchievementBiolerplate
{
    public override string AchievementProperty => "AchievementDeckout";

    public override string AchievementName => "Deckout";

    public override string AchievementDescription => "Win through deckout";
}

public class AchievementDoubleKill: AchievementBiolerplate
{
    public override string AchievementProperty => "AchievementDoubleKill";

    public override string AchievementName => "Double Kill";

    public override string AchievementDescription => "Foe lost with as much negative hp as maxhp";
}

public class AchievementHandOverload : AchievementBiolerplate
{
    public override string AchievementProperty => "AchievementHandOverload";

    public override string AchievementName => "Hand Overload";

    public override string AchievementDescription => "Discard more than 5 times in a game";
}

public class AchievementFeatherHands : AchievementBiolerplate
{
    public override string AchievementProperty => "AchievementFeatherHands";

    public override string AchievementName => "Feather Hands";

    public override string AchievementDescription => "End 5 turns with 0 cards in hand";
}

public class AchievementQuantaOverload : AchievementBiolerplate
{
    public override string AchievementProperty => "AchievementQuantaOverload";

    public override string AchievementName => "Quanta Overload";

    public override string AchievementDescription => "Gain Quanta in an element that already has 75 quanta";
}

public class AchievementChromaExcess : AchievementBiolerplate
{
    public override string AchievementProperty => "AchievementChromaExcess";

    public override string AchievementName => "Chroma Excess";

    public override string AchievementDescription => "Have a full quanta pool";
}