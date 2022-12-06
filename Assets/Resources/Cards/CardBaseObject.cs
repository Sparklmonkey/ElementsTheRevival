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
		iD = cardToCopy.iD;
		imageID = cardToCopy.imageID;
		cardName = cardToCopy.cardName;
		cardType = cardToCopy.cardType;
		atk = cardToCopy.atk;
		def = cardToCopy.def;
		skill = cardToCopy.skill;
		skillCost = cardToCopy.skillCost;
		skillElement = cardToCopy.skillElement;
		cost = cardToCopy.cost;
		costElement = cardToCopy.costElement;
		rarity = cardToCopy.rarity;
		passive = cardToCopy.passive;
		innate = cardToCopy.innate;
		desc = cardToCopy.desc;
		IsAflatoxin = cardToCopy.IsAflatoxin;
		AbilityUsed = cardToCopy.AbilityUsed;
		Poison = cardToCopy.Poison;
		Charge = cardToCopy.Charge;
		Freeze = cardToCopy.Freeze;
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