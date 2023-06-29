using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Card
{
    public string iD;
    public string imageID;
    public string cardName;
    public CardType cardType;
    public int atk;
    public int def;
    public string skill;
    public int skillCost;
    public Element skillElement;
    public int cost;
    public Element costElement;
    public int rarity;
    public List<string> passive;
    public List<string> innate;
    public string desc;

    public bool IsDelayed { get { return innate.Contains("delay"); } }
    public bool IsGravity { get { return passive.Contains("delay"); } }
    public bool IsPsion { get { return innate.Contains("psion"); } }
    public bool IsAdrenaline { get { return passive.Contains("adrenaline"); } }
    public bool IsMomentum { get { return passive.Contains("momentum"); } }
    public bool IsImmaterial { get { return innate.Contains("immaterial"); } }
    public bool IsBurrowed { get { return innate.Contains("burrowed"); } }
    public bool IsAflatoxin { get; set; }
    public bool AbilityUsed { get; set; }
    public bool ReadyUsed { get; set; }
    public int Poison { get; set; }
    public int Charge { get; set; }
    public int Freeze { get; set; }
    public int TurnsInPlay { get; set; }
    public int DefNow { get { return def + DefModify - DefDamage; } }
    public int AtkNow { get { return atk + AtkModify; } }
    public int DefModify { get; set; }
    public int DefDamage { get; set; }
    public int AtkModify { get; set; }
    public int BuyPrice { get { return (rarity * rarity * 6) + cost; } }
    public int SellPrice { get { return (rarity * rarity * 4) + cost; } }
    public Card(Card cardToCopy)
    {
        Card newBase = (Card)cardToCopy.MemberwiseClone();
        iD = newBase.iD;
        imageID = newBase.imageID;
        cardName = newBase.cardName;
        cardType = newBase.cardType;
        atk = newBase.atk;
        def = newBase.def;
        skill = newBase.skill;
        skillCost = newBase.skillCost;
        skillElement = newBase.skillElement;
        cost = newBase.cost;
        costElement = newBase.costElement;
        rarity = newBase.rarity;
        passive = newBase.passive;
        innate = newBase.innate;
        desc = newBase.desc;
        IsAflatoxin = newBase.IsAflatoxin;
        AbilityUsed = newBase.AbilityUsed;
        Poison = newBase.Poison;
        Charge = newBase.Charge;
        Freeze = newBase.Freeze;
    }
}

[Serializable]
public class CardDB
{
	public List<Card> cardDb;
	public CardDB()
    {
		cardDb = new List<Card>();
    }
}