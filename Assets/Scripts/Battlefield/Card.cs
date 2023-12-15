using System;
using System.Collections.Generic;

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
    public string desc;

    public PassiveSkills passiveSkills;
    public InnateSkills innateSkills;
    public bool IsAflatoxin { get; set; }
    public bool AbilityUsed { get; set; }
    public bool ReadyUsed { get; set; }
    public int Poison { get; set; }
    public int Charge { get; set; }
    public int Freeze { get; set; }
    public int TurnsInPlay { get; set; }
    public int DefNow => def + DefModify - DefDamage;
    public int AtkNow => atk + AtkModify;
    public int DefModify { get; set; }
    public int DefDamage { get; set; }
    public int AtkModify { get; set; }
    public int BuyPrice => iD.IsUpgraded() ? iD.GetRegularBuyPrice() + 1500 : rarity * rarity * 6 + cost;
    public int SellPrice => rarity * rarity * 4 + cost;

    public Card(Card cardToCopy)
    {
        var newBase = (Card)cardToCopy.MemberwiseClone();
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
        desc = newBase.desc;
        IsAflatoxin = newBase.IsAflatoxin;
        AbilityUsed = newBase.AbilityUsed;
        passiveSkills = newBase.passiveSkills;
        innateSkills = newBase.innateSkills;
        Poison = newBase.Poison;
        Charge = newBase.Charge;
        Freeze = newBase.Freeze;
    }

    public Card()
    {
        iD = "";
        imageID = "";
        cardName = "";
        cardType = CardType.Pillar;
        atk = 0;
        def = 0;
        skill = "";
        skillCost = 0;
        skillElement = Element.Aether;
        cost = 0;
        costElement = Element.Aether;
        rarity = 0;
        passiveSkills = default;
        innateSkills = default;
        desc = "";
        IsAflatoxin = false;
        AbilityUsed = true;
        Poison = 0;
        Charge = 0;
        Freeze = 0;
    }

    public Card Clone()
    {
        var clone = MemberwiseClone();
        if (clone is Card card)
        {
            return card;
        }
        return this;
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
