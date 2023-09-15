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
    public static GameStats Shared;

    public Dictionary<string, Stat> AILevel0;
    public Dictionary<string, Stat> AILevel1;
    public Dictionary<string, Stat> AILevel2;
    public Dictionary<string, Stat> AILevel3;
    public Dictionary<string, Stat> AILevel4;
    public Dictionary<string, Stat> AILevel5;
    public Dictionary<string, Stat> ArenaT50;
    public Dictionary<string, Stat> PvpOne;
    public Dictionary<string, Stat> PvpTwo;
    public GameStats()
    {
        AILevel0 = new();
        AILevel1 = new();
        AILevel2 = new();
        AILevel3 = new();
        AILevel4 = new();
        AILevel5 = new();
        PvpOne = new() { { "Sparklmonkey", new() } };
        PvpTwo = new() { { "Sparklmonkey", new() } };
        ArenaT50 = new() { { "Sparklmonkey", new() } };
        foreach (var item in StringHelper.ElementStringList)
        {
            AILevel0.Add(item, new());
            AILevel1.Add(item, new());
        }
        foreach (var item in StringHelper.Ai2List)
        {
            AILevel2.Add(item, new());
            AILevel3.Add(item, new());
        }
        foreach (var item in StringHelper.ElderPrefix)
        {
            foreach (var item2 in StringHelper.ElderSuffix)
            {
                AILevel4.Add($"{item}{item2}", new());
            }
        }
        foreach (var item in StringHelper.FalseGodNameList)
        {
            AILevel5.Add(item, new());
        }
    }

    public void UpdateValues(GameStatRequest stats)
    {
        switch (stats.aiLevel)
        {
            case 0:
                if (stats.isWin)
                {
                    AILevel0[stats.aiName].wins++;
                }
                else
                {
                    AILevel0[stats.aiName].loses++;
                }
                break;
            case 1:
                if (stats.isWin)
                {
                    AILevel1[stats.aiName].wins++;
                }
                else
                {
                    AILevel1[stats.aiName].loses++;
                }
                break;
            case 2:
                if (stats.isWin)
                {
                    AILevel2[stats.aiName].wins++;
                }
                else
                {
                    AILevel2[stats.aiName].loses++;
                }
                break;
            case 3:
                if (stats.isWin)
                {
                    AILevel3[stats.aiName].wins++;
                }
                else
                {
                    AILevel3[stats.aiName].loses++;
                }
                break;
            case 4:
                if (stats.isWin)
                {
                    AILevel4[stats.aiName].wins++;
                }
                else
                {
                    AILevel4[stats.aiName].loses++;
                }
                break;
            case 5:
                if (stats.isWin)
                {
                    AILevel5[stats.aiName].wins++;
                }
                else
                {
                    AILevel5[stats.aiName].loses++;
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
