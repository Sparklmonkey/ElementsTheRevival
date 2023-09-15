using System;
using UnityEngine.Serialization;

[Serializable]
public class PlayerAchievements
{
    // -1 => Not Complete, 0 => Ai level 0, 1 => Ai Level 1, etc

    [FormerlySerializedAs("IdleYet")] public int idleYet;

    [FormerlySerializedAs("CreatureDomination")] public int creatureDomination;

    [FormerlySerializedAs("Creatureless")] public int creatureless;

    [FormerlySerializedAs("Deckout")] public int deckout;

    [FormerlySerializedAs("DoubleKill")] public int doubleKill;

    [FormerlySerializedAs("HandOverload")] public int handOverload;

    [FormerlySerializedAs("FeatherHands")] public int featherHands;

    [FormerlySerializedAs("QuantaOverload")] public int quantaOverload;

}