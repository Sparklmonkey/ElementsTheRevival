using System;

[Serializable]
public class CardPassives
{
    //Cannot be target of spells/abilities
    public bool isImmaterial;
    //Cannot be target of spells/abilities and Damage dealt is halved
    public bool isBurrowed;
    //When card dies, spawn skeleton
    public bool willSkeleton;
    //Bypass Wings Shield
    public bool isAirborne;
    //Bypass Wings Shield
    public bool hasReach;
    //Bypass all shields
    public bool hasMomentum;
    //Damage heals instead
    public bool isAntimatter;
    //When target of "Reverse Time" spawn random creature/elite creature
    public bool isUndead;
    //When target of "Reverse Time" spawn Pharaoh/Elite Pharaoh
    public bool isMummy;
    //When card dies, spawn Malignant Cell
    public bool hasAflatoxin;
    //Damage dealt causes poison on target
    public bool isVenemous;
    //If devoured, Devourer is poisoned
    public bool isPoisonous;
    //All attacks hit this card first
    public bool hasGravity;
    //Spells are reflected back to opponent
    public bool isReflect;
    //Flag to tell if Card is a mutant
    public bool isMutant;
    //Double attack damage for one turn
    public bool isDiving;
    //Damage dealt, heals user
    public bool isVampire;
    //Damage dealt, damages opponent. Statuses also affect opponent
    public bool isVoodoo;
    //Damage is considered to come from a spell
    public bool isPsion;
    //Spawn an Ash when this creature dies
    public bool isPhoenix;
    //Damage dealt causes 2 poison on target
    public bool isDeadlyVenemous;
    //If creature has Adrenaline
    public bool hasAdrenaline;
    //Add Extra poison for each card played by opponent
    public bool hasNeurotoxin;
    //add 5 damage for each Fire quanta in pool on attack only
    public bool isFiery;

    public CardPassives()
    {
        isImmaterial = false;
        isBurrowed = false;
        willSkeleton = false;
        isAirborne = false;
        hasReach = false;
        hasMomentum = false;
        isAntimatter = false;
        isUndead = false;
        isMummy = false;
        hasAflatoxin = false;
        isVenemous = false;
        isPoisonous = false;
        hasGravity = false;
        isReflect = false;
        isMutant = false;
        isDiving = false;
        isVampire = false;
        isVoodoo = false;
        isPsion = false;
        isPhoenix = false;
        isDeadlyVenemous = false;
        hasAdrenaline = false;
    hasNeurotoxin = false;
    isFiery = false;
}
}


public enum PassiveEnum
{
    IsMaterial,
    IsBurrowed,
    WillSkeleton,
    IsAirborne,
    HasReach,
    HasMomentum,
    IsAntimatter,
    IsUndead,
    IsMummy,
    HasAflatoxin,
    IsVenemous,
    IsPoisonous,
    HasGravity,
    IsReflect,
    IsMutant,
    IsDiving,
    IsVampire,
    IsVoodoo,
    IsPsion,
    IsPheonix,
    IsDeadlyVenemous,
    hasAdrenaline,
    isReady
}