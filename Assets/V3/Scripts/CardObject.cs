using System.Collections.Generic;
using Battlefield.Abilities;
using UnityEngine;

namespace V3.Scripts
{
    public interface ICard
    {
        // Name of the card
        public string CardName { get; set; }
        // Image of the card
        public Sprite CardImage { get; set; }
        // Unique Id of the card
        public string Id { get; set; }
        // Cost to play the card
        public int Cost { get; set; }
        // Quanta identity of the card
        public Element CardElement { get; set; }
        // Quanta used to play the card
        public Element CostElement { get; set; }
        // Rarity of the card
        public int Rarity { get; set; }
        // Text that fills the text box.
        public string Desc { get; set; }
    }

    public interface IPermanent
    {
        
        public InnateSkills InnateSkills { get; set; }
        public List<PassiveModifiers> PassiveModifiers { get; set; }
        public List<Counters> Counters { get; set; }
        
        public void AddInnateModifier(InnateModifiers modifier);
        public void AddPassiveModifier(PassiveModifiers modifier);
        public void AddCounter(Counters counter);
        public void RemovePassiveModifier(PassiveModifiers modifier);
        public void RemoveCounter(Counters counter);
    }

    public enum Counters
    {
        Poison,
        Purify,
        Freeze,
        Delay,
        Aflatoxin,
        Dive,
        DivineShield,
    }

    public enum PassiveModifiers
    {
        Momentum,
        Burrow,
        Venom,
        Neurotoxin,
        Vampire,
        Psion,
        DeadlyVenom,
        Antimatter,
        Adrenaline,
        GravityPull,
        Mutant,
        Readiness
    }

    public enum InnateModifiers
    {
        Ranged,
        Airborne,
        Undead,
        Venom,
        Mummy,
        Poisonous,
        Voodoo,
        Obsession,
        Immaterial
    }

    public interface ICreature
    {
        public int BaseAtk { get; }
        public int BaseDef { get; }
        
        public int Damage { get; set; }
        
        public int AtkModify { get; set; }
        public int DefModify { get; set; }
        
        public int DefNow => BaseAtk + DefModify - Damage;
        public int AtkNow => BaseDef + AtkModify;

        public void SetDefDamage(int amount)
        {
            Damage += amount;
            if (Damage < 0)
            {
                Damage = 0;
            }
        }

    }
    
    // public class CardObject
    // {
    //
    // [PropertySpace(SpaceBefore = 10)] [Title("Turn Phase Abilities", null, TitleAlignments.Centered)]
    // public DeathTriggerAbility DeathTriggerAbility;
    // public OnPlayRemoveAbility PlayRemoveAbility;
    // public OnEndTurnAbility TurnEndAbility;
    // public OnEndTurnAbility PreAttackAbility;
    //
    // [PropertySpace(SpaceBefore = 10)]
    // [EnumToggleButtons]
    // [Title("Card Type", null, TitleAlignments.Centered), HideLabel]
    // public CardType Type;
    //
    // [PropertySpace(SpaceBefore = 10)]
    // [Title("Weapon Passive Skills", null, TitleAlignments.Centered)]
    // [ShowIf("@Type == CardType.Weapon"), HideLabel, EnumToggleButtons]
    // public WeaponSkill WeaponPassive;
    //
    // [PropertySpace(SpaceBefore = 10)]
    // [Title("Shield Passive Skills", null, TitleAlignments.Centered)]
    // [ShowIf("@Type == CardType.Shield"), HideLabel, EnumToggleButtons]
    // public ShieldSkill ShieldPassive;
    //
    // [PropertySpace(SpaceBefore = 10)]
    // [Title("Skill Info", null, TitleAlignments.Centered)]
    // [HideIf("@Type == CardType.Pillar || Type == CardType.Shield || Type == CardType.Mark")]
    // public ActivatedAbility Skill;
    // [HideIf("@Type == CardType.Pillar || Type == CardType.Shield || Type == CardType.Mark")]
    // public int SkillCost;
    // [HideIf("@Type == CardType.Pillar || Type == CardType.Shield || Type == CardType.Mark")]
    // public Element SkillElement;
    //
    //
    // [HorizontalGroup("Ability")]
    // public bool AbilityUsed = true;
    // [HorizontalGroup("Ability")]
    // public bool ReadyUsed;
    //
    // [PropertySpace(SpaceBefore = 10)]
    // [Title("Turn Limit", null, TitleAlignments.Centered)]
    // public bool HasTurnLimit;
    // [ShowIf("@HasTurnLimit")]
    // public int TurnsInPlay;
    //
    // [HideInInspector]
    // public bool IsPendulumTurn;
    //
    //
    // [HorizontalGroup("Bazaar"), ReadOnly, ShowInInspector]
    // public int BuyPrice
    // {
    //     get
    //     {
    //         if (!Id.IsUpgraded()) return Rarity * Rarity * 6;
    //         var unUppedCost = CardDatabase.Instance.GetUnuppedAlt(Id).BuyPrice;
    //         return unUppedCost + 1500;
    //     }
    // }
    //
    // [HorizontalGroup("Bazaar"), ReadOnly, ShowInInspector]
    // public int SellPrice => Rarity * Rarity * 4 + Cost;
    //
    // public bool IsAbilityUsable(QuantaCheck quantaCheck, int handCount)
    // {
    //     if (Skill is null) return false;
    //     if (Counters.Delay > 0) return false;
    //     if (Counters.Freeze > 0) return false;
    //     if (Type is CardType.Shield or CardType.Pillar or CardType.Mark) return false;
    //     if (passiveSkills.Readiness)
    //     {
    //         if (AbilityUsed && ReadyUsed) return false;
    //     }
    //     else if (AbilityUsed) return false;
    //     if (!quantaCheck(SkillElement, SkillCost)) return false;
    //     if (Skill is Hasten or Duality && handCount >= 8) return false;
    //
    //     return true;
    // }
    // }
}