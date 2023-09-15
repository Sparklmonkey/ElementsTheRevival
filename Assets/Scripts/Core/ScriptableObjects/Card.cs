using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CardSO", menuName = "ScriptableObjects/CardSO", order = 1)]
[Serializable]
public class CardSo : ScriptableObject
{
    [Header("Basic Info")]
    public Sprite cardImage;
    public string cardName;
    public string iD;
    [TextArea(4, 4)]
    public string description;
    public int buyPrice;
    public int sellPrice;
    public string imageID;
    public int cost;
    public Element element;
    public CardType type;
    public bool isRare;


    [Header("Creature Info")]
    public int power;
    public int basePower;
    public int hp;
    [FormerlySerializedAs("maxHP")] public int maxHp;
    [Header("On Field Info")]
    public bool firstTurn = true;
    public bool pendulumTurn = true;
    public bool abilityUsed = true;
    public int turnsInPlay = 999999;

}


