using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oracle_SpinResult : MonoBehaviour
{
    private List<Vector3> elementZRotations = new List<Vector3> { new Vector3(0, 0, 345),   //Aether
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
    private List<int> commonCardElectrum = new List<int> { 20, 30, 40 };
    private List<int> nymphCardElectrum = new List<int> { 75, 300 };
    private List<int> weaponCardElectrum = new List<int> { 30, 120 };

    public (Card, int, Vector3) GetOracleResults()
    {
        int finalRotation = Random.Range(0, 12);

        (Card, int, Vector3) oracleResult = (null, 0, elementZRotations[finalRotation]);
        int cardTypeToSpawn = Random.Range(0, 100);
        if (cardTypeToSpawn > 97)
        {
            oracleResult.Item1 = CardDatabase.GetRandomRegularNymph((Element)Random.Range(0,12));
            oracleResult.Item2 = nymphCardElectrum[Random.Range(0, nymphCardElectrum.Count)];
        }
        else if (cardTypeToSpawn > 87)
        {
            oracleResult.Item1 = CardDatabase.GetRandomCardOfTypeWithElement(CardType.Weapon, (Element)finalRotation, false);
            oracleResult.Item2 = weaponCardElectrum[Random.Range(0, weaponCardElectrum.Count)];
        }
        else if (cardTypeToSpawn > 77)
        {
            oracleResult.Item1 = CardDatabase.GetRandomCardOfTypeWithElement(CardType.Shield, (Element)finalRotation, false);
            oracleResult.Item2 = commonCardElectrum[Random.Range(0, commonCardElectrum.Count)];
        }
        else if (cardTypeToSpawn > 57)
        {
            oracleResult.Item1 = CardDatabase.GetRandomCardOfTypeWithElement(CardType.Creature, (Element)finalRotation, false);
                oracleResult.Item1 = CardDatabase.GetRandomCardOfTypeWithElement(CardType.Creature, (Element)finalRotation, false);
            oracleResult.Item2 = commonCardElectrum[Random.Range(0, commonCardElectrum.Count)];
        }
        else if (cardTypeToSpawn > 37)
        {
            oracleResult.Item1 = CardDatabase.GetRandomCardOfTypeWithElement(CardType.Spell, (Element)finalRotation, false);
                oracleResult.Item1 = CardDatabase.GetRandomCardOfTypeWithElement(CardType.Spell, (Element)finalRotation, false);
            oracleResult.Item2 = commonCardElectrum[Random.Range(0, commonCardElectrum.Count)];
        }
        else if (cardTypeToSpawn > 27)
        {
            oracleResult.Item1 = CardDatabase.GetRandomCardOfTypeWithElement(CardType.Artifact, (Element)finalRotation, false);
            oracleResult.Item2 = commonCardElectrum[Random.Range(0, commonCardElectrum.Count)];
        }
        else
        {
            oracleResult.Item1 = CardDatabase.GetRandomCardOfTypeWithElement(CardType.Pillar, (Element)finalRotation, false);
            oracleResult.Item2 = commonCardElectrum[Random.Range(0, commonCardElectrum.Count)];
        }

        return oracleResult;
    }
}
