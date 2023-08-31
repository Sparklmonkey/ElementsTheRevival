using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OracleHelper
{

    private static readonly List<Vector3> elementZRotations = new()
    {
        new Vector3(0, 0, 345),     //Aether
        new Vector3(0, 0, 225),     //Air
        new Vector3(0, 0, 15),      //Darkness
        new Vector3(0, 0, 76),      //Light
        new Vector3(0, 0, 136),     //Death
        new Vector3(0, 0, 315),     //Earth
        new Vector3(0, 0, 195),     //Entropy
        new Vector3(0, 0, 165),     //Time
        new Vector3(0, 0, 45),      //Fire
        new Vector3(0, 0, 106),     //Gravity
        new Vector3(0, 0, 285),     //Life
        new Vector3(0, 0, 255)      //Water
    };
    private static readonly List<int> commonCardElectrum = new() { 20, 30, 40 };
    private static readonly List<int> nymphCardElectrum = new() { 75, 300 };
    private static readonly List<int> weaponCardElectrum = new() { 30, 120 };
    private static readonly List<string> falseGodNameList = new() { "Akebono",
                                                                "Chaos Lord",
                                                                "Dark Matter",
                                                                "Decay",
                                                                "Destiny",
                                                                "Divine Glory",
                                                                "Dream Catcher",
                                                                "Elidnis",
                                                                "Eternal Phoenix",
                                                                "Ferox",
                                                                "Fire Queen",
                                                                "Gemini",
                                                                "Graviton",
                                                                "Hecate",
                                                                "Hermes",
                                                                "Incarnate",
                                                                "Jezebel",
                                                                "Lionheart",
                                                                "Miracle",
                                                                "Morte",
                                                                "Neptune",
                                                                "Obliterator",
                                                                "Octane",
                                                                "Osiris",
                                                                "Paradox",
                                                                "Rainbow",
                                                                "Scorpio",
                                                                "Seism",
                                                                "Serket"};

    public static string GetNextFalseGod() => falseGodNameList[Random.Range(0, falseGodNameList.Count)];

    public static string GetPetForNextBattle(Element element)
    {
        if (Random.Range(0, 100) > 75)
        {
            return CardDatabase.Instance.GetRandomPet().cardName;
        }
        switch (element)
        {
            case Element.Aether:
                return "Spark";
            case Element.Air:
                return "Dragonfly";
            case Element.Darkness:
                return "Parasite";
            case Element.Light:
                return "Photon";
            case Element.Death:
                return "Virus";
            case Element.Earth:
                return "Gnome Rider";
            case Element.Entropy:
                return "Schrodinger's Cat";
            case Element.Time:
                return "Deja Vu";
            case Element.Fire:
                return "Ash Eater";
            case Element.Life:
                return "Rustler";
            case Element.Water:
                return "Chrysaora";
            default:
                return "";
        }
    }


    public static (Card, int, Vector3) GetOracleResults()
    {
        int finalRotation = Random.Range(0, 12);

        (Card, int, Vector3) oracleResult = (null, 0, elementZRotations[finalRotation]);
        int cardTypeToSpawn = Random.Range(0, 100);
        if (cardTypeToSpawn > 97)
        {
            oracleResult.Item1 = CardDatabase.Instance.GetRandomRegularNymph((Element)Random.Range(0, 12));
            oracleResult.Item2 = nymphCardElectrum[Random.Range(0, nymphCardElectrum.Count)];
        }
        else if (cardTypeToSpawn > 87)
        {
            oracleResult.Item1 = CardDatabase.Instance.GetRandomCardOfTypeWithElement(CardType.Weapon, (Element)finalRotation, false);
            oracleResult.Item2 = weaponCardElectrum[Random.Range(0, weaponCardElectrum.Count)];
        }
        else if (cardTypeToSpawn > 77)
        {
            oracleResult.Item1 = CardDatabase.Instance.GetRandomCardOfTypeWithElement(CardType.Shield, (Element)finalRotation, false);
            oracleResult.Item2 = commonCardElectrum[Random.Range(0, commonCardElectrum.Count)];
        }
        else if (cardTypeToSpawn > 57)
        {
            oracleResult.Item1 = CardDatabase.Instance.GetOracleCreature((Element)finalRotation);
            oracleResult.Item2 = commonCardElectrum[Random.Range(0, commonCardElectrum.Count)];
        }
        else if (cardTypeToSpawn > 37)
        {
            oracleResult.Item1 = CardDatabase.Instance.GetRandomCardOfTypeWithElement(CardType.Spell, (Element)finalRotation, false);
            oracleResult.Item2 = commonCardElectrum[Random.Range(0, commonCardElectrum.Count)];
        }
        else if (cardTypeToSpawn > 27)
        {
            oracleResult.Item1 = CardDatabase.Instance.GetRandomCardOfTypeWithElement(CardType.Artifact, (Element)finalRotation, false);
            oracleResult.Item2 = commonCardElectrum[Random.Range(0, commonCardElectrum.Count)];
        }
        else
        {
            oracleResult.Item1 = CardDatabase.Instance.GetRandomCardOfTypeWithElement(CardType.Pillar, (Element)finalRotation, false);
            oracleResult.Item2 = commonCardElectrum[Random.Range(0, commonCardElectrum.Count)];
        }

        return oracleResult;
    }
}
