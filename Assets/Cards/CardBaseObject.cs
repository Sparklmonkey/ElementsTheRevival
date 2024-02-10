using System.Collections;
using System.Collections.Generic;
using Battlefield.Abilities;
using UnityEngine;
using Elements.Duel.Manager;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName = "CardObject", menuName = "ScriptableObjects/CardObject", order = 3)]
public class CardBaseObject : SerializedScriptableObject
{
    
    [PropertySpace(SpaceBefore = 10)]
    [Title("Basic Information", "",TitleAlignments.Centered)]
    public string CardName = "Card Name";
    
    [HorizontalGroup("Sprite", 100), HideLabel, PreviewField(100, ObjectFieldAlignment.Left)]
    public Sprite cardImage;
    [VerticalGroup("Sprite/Info"), LabelText("Unique Id")]
    public string Id = "Card ID";
    [VerticalGroup("Sprite/Info"), LabelText("Play Cost")]
    public int Cost = 0;
    [VerticalGroup("Sprite/Info"), LabelText("Card Element")]
    public Element CardElement;
    [VerticalGroup("Sprite/Info"), LabelText("Cost Element")]
    public Element CostElement;
    [VerticalGroup("Sprite/Info"), LabelText("Card Rarity")]
    public int Rarity;
    
    [PropertySpace(SpaceBefore = 10)]
    [Title("Card Description", null, TitleAlignments.Centered)]
    [HideLabel]
    [MultiLineProperty(3)]
    public string Desc = "";
    
    [PropertySpace(SpaceBefore = 10)]
    [EnumToggleButtons]
    [Title("Card Type", null, TitleAlignments.Centered), HideLabel]
    public CardType Type;
    
    [PropertySpace(SpaceBefore = 20)]
    [Title("Attack Info", null, TitleAlignments.Centered)]
    [ShowIf("@Type == CardType.Creature || Type == CardType.Weapon || Type == CardType.Shield")]
    public int Atk;
    [ShowIf("@Type == CardType.Creature || Type == CardType.Weapon || Type == CardType.Shield")]
    public int Def;
    
    [PropertySpace(SpaceBefore = 10)]
    [Title("Weapon Passive Skills", null, TitleAlignments.Centered)]
    [ShowIf("@Type == CardType.Weapon"), HideLabel, EnumToggleButtons]
    public WeaponSkill WeaponPassive;
    
    [PropertySpace(SpaceBefore = 10)]
    [Title("Shield Passive Skills", null, TitleAlignments.Centered)]
    [ShowIf("@Type == CardType.Shield"), HideLabel, EnumToggleButtons]
    public ShieldSkill ShieldPassive;
    
    [PropertySpace(SpaceBefore = 10)]
    [Title("Skill Info", null, TitleAlignments.Centered)]
    [HideIf("@Type == CardType.Pillar || Type == CardType.Shield || Type == CardType.Mark")]
    public ActivatedAbility Skill;
    [HideIf("@Type == CardType.Pillar || Type == CardType.Shield || Type == CardType.Mark")]
    public int SkillCost;
    [HideIf("@Type == CardType.Pillar || Type == CardType.Shield || Type == CardType.Mark")]
    public Element SkillElement;
    
    [PropertySpace(SpaceBefore = 10)]
    [Title("Passive Skills", null, TitleAlignments.Centered)]
    public PassiveSkills passiveSkills;
    
    [PropertySpace(SpaceBefore = 10)]
    [Title("Innate Skills", null, TitleAlignments.Centered)]
    public InnateSkills innateSkills;
    
    [PropertySpace(SpaceBefore = 10)]
    [Title("Effect Counters", null, TitleAlignments.Centered)]
    public CardCounters Counters;

    [HorizontalGroup("Ability")]
    public bool AbilityUsed;
    [HorizontalGroup("Ability")]
    public bool ReadyUsed;

    [PropertySpace(SpaceBefore = 10)]
    [Title("Turn Limit", null, TitleAlignments.Centered)]
    public bool HasTurnLimit;
    [ShowIf("@HasTurnLimit")]
    public int TurnsInPlay;
    
    
    public int DefNow => Def + DefModify - DefDamage;
    public int AtkNow => Atk + AtkModify;
    public int DefModify { get; set; }
    public int DefDamage { get; set; }
    public int AtkModify { get; set; }
    
    [HorizontalGroup("Bazaar"), ReadOnly, ShowInInspector]
    public int BuyPrice => Rarity * Rarity * 6;
    [HorizontalGroup("Bazaar"), ReadOnly, ShowInInspector]
    public int SellPrice => Rarity * Rarity * 4 + Cost;
    
    public bool IsAbilityUsable(QuantaCheck quantaCheck, int handCount)
    {
        if (Skill is null) return false;
        if (AbilityUsed) return false;
        if (Counters.Delay > 0) return false;
        if (Counters.Freeze > 0) return false;
        if (Type is CardType.Shield or CardType.Pillar or CardType.Mark) return false;
        if (!quantaCheck(SkillElement, SkillCost)) return false;
        if (Skill.Equals(typeof(Hasten)) && handCount >= 8) return false;

        return true;
    }
}

public struct CardCounters
{
    public int Poison;
    public int Charge;
    public int Freeze;
    public int Delay;
}