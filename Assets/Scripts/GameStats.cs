using System;
using System.Collections.Generic;

[Serializable]
class Stat
{
    public int wins;
    public int loses;
}
[Serializable]
internal class GameStats
{
    public static GameStats shared;

    public Dictionary<string, Stat> aiLevel0;
    public Dictionary<string, Stat> aiLevel1;
    public Dictionary<string, Stat> aiLevel2;
    public Dictionary<string, Stat> aiLevel3;
    public Dictionary<string, Stat> aiLevel4;
    public Dictionary<string, Stat> aiLevel5;
    public Dictionary<string, Stat> ArenaT50;
    public Dictionary<string, Stat> PvpOne;
    public Dictionary<string, Stat> PvpTwo;
    public GameStats()
    {
        aiLevel0 = new();
        aiLevel1 = new();
        aiLevel2 = new();
        aiLevel3 = new();
        aiLevel4 = new();
        aiLevel5 = new();
        PvpOne = new() { { "Sparklmonkey", new() } };
        PvpTwo = new() { { "Sparklmonkey", new() } };
        ArenaT50 = new() { { "Sparklmonkey", new() } };
        foreach (var item in StringHelper.ElementStringList)
        {
            aiLevel0.Add(item, new());
            aiLevel1.Add(item, new());
        }
        foreach (var item in StringHelper.Ai2List)
        {
            aiLevel2.Add(item, new());
            aiLevel3.Add(item, new());
        }
        foreach (var item in StringHelper.ElderPrefix)
        {
            foreach (var item2 in StringHelper.ElderSuffix)
            {
                aiLevel4.Add($"{item}{item2}", new());
            }
        }
        foreach (var item in StringHelper.FalseGodNameList)
        {
            aiLevel5.Add(item, new());
        }
    }

    public void UpdateValues(GameStatRequest stats)
    {
        switch (stats.aiLevel)
        {
            case 0:
                if (stats.isWin)
                {
                    aiLevel0[stats.aiName].wins++;
                }
                else
                {
                    aiLevel0[stats.aiName].loses++;
                }
                break;
            case 1:
                if (stats.isWin)
                {
                    aiLevel1[stats.aiName].wins++;
                }
                else
                {
                    aiLevel1[stats.aiName].loses++;
                }
                break;
            case 2:
                if (stats.isWin)
                {
                    aiLevel2[stats.aiName].wins++;
                }
                else
                {
                    aiLevel2[stats.aiName].loses++;
                }
                break;
            case 3:
                if (stats.isWin)
                {
                    aiLevel3[stats.aiName].wins++;
                }
                else
                {
                    aiLevel3[stats.aiName].loses++;
                }
                break;
            case 4:
                if (stats.isWin)
                {
                    aiLevel4[stats.aiName].wins++;
                }
                else
                {
                    aiLevel4[stats.aiName].loses++;
                }
                break;
            case 5:
                if (stats.isWin)
                {
                    aiLevel5[stats.aiName].wins++;
                }
                else
                {
                    aiLevel5[stats.aiName].loses++;
                }
                break;
            case 6:
                if (ArenaT50.ContainsKey(stats.aiName))
                {
                    if (stats.isWin)
                    {
                        ArenaT50[stats.aiName].wins++;
                    }
                    else
                    {
                        ArenaT50[stats.aiName].loses++;
                    }
                }
                else
                {
                    ArenaT50.Add(stats.aiName, new Stat() { wins = stats.isWin ? 1 : 0, loses = stats.isWin ? 0 : 1 });
                }
                break;
            case 7:
                if (PvpOne.ContainsKey(stats.aiName))
                {
                    if (stats.isWin)
                    {
                        PvpOne[stats.aiName].wins++;
                    }
                    else
                    {
                        PvpOne[stats.aiName].loses++;
                    }
                }
                else
                {
                    PvpOne.Add(stats.aiName, new Stat() { wins = stats.isWin ? 1 : 0, loses = stats.isWin ? 0 : 1 });
                }
                break;
            case 8:
                if (PvpTwo.ContainsKey(stats.aiName))
                {
                    if (stats.isWin)
                    {
                        PvpTwo[stats.aiName].wins++;
                    }
                    else
                    {
                        PvpTwo[stats.aiName].loses++;
                    }
                }
                else
                {
                    PvpTwo.Add(stats.aiName, new Stat() { wins = stats.isWin ? 1 : 0, loses = stats.isWin ? 0 : 1 });
                }
                break;
            default:
                break;
        }
    }
}

internal class StringHelper
{
    public static List<string> ElementStringList = new() { "Aether", "Darkness", "Air", "Light", "Gravity", "Death", "Entropy", "Water", "Life", "Earth", "Fire", "Time" };
    public static List<string> ElderPrefix = new() { "Aeth", "Air", "Shad", "Lum", "Mor", "Ter", "Dis", "Chr", "Pyr", "Mas", "Vit", "Aqua" };
    public static List<string> ElderSuffix = new() { "eric", "es", "ow", "iel", "tis", "ra", "cord", "onos", "ofuze", "sa", "al", "rius" };
    public static List<string> Ai2List = new() { "Ethereal", "Airborne", "Vampire", "Plague", "Mountain", "Entropy", "Flamer", "Relativity", "Growth", "LED", "Who", "Drowned" };
    public static List<string> FalseGodNameList = new() { "Divine Glory",
                                                                "Serket",
                                                                "Morte",
                                                                "Lionheart",
                                                                "Incarnate",
                                                                "Fire Queen",
                                                                "Seism",
                                                                "Miracle",
                                                                "Graviton",
                                                                "Paradox",
                                                                "Akebono",
                                                                "Neptune",
                                                                "Scorpio",
                                                                "Osiris",
                                                                "Octane",
                                                                "Rainbow",
                                                                "Obliterator",
                                                                "Gemini",
                                                                "Chaos Lord",
                                                                "Dark Matter",
                                                                "Decay",
                                                                "Destiny",
                                                                "Dream Catcher",
                                                                "Elidnis",
                                                                "Eternal Phoenix",
                                                                "Ferox",
                                                                "Hecate",
                                                                "Hermes",
                                                                "Jezebel"};
}
