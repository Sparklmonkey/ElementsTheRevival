using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "CardSO", menuName = "ScriptableObjects/CardSO", order = 1)]
[Serializable]
public class Card : ScriptableObject
{
    [Header("Basic Info")]
    public Sprite cardImage;

    [TextArea(4,4)]
    public string description;
    public int buyPrice;
    public int sellPrice;
    public string imageID;
    public int cost;
    public Element element;
    public CardType type;
    public bool isUpgradable;
    public bool isRare;
    public Card upgradedVersion;


    [Header("Creature Info")]
    public int power;
    public int basePower;
    public int hp;
    public int maxHP;

    [Header("Script Field")]
    public CardAbilities cardAbilities;
    public IActivateAbility activeAbility;
    public ISpellAbility spellAbility;
    public IEndTurnAbility endTurnAbility;
    public IOnDeathAbility onDeathAbility;
    public IOnPlayAbility onPlayAbility;
    public IWeaponAbility weaponAbility;
    public IShieldAbility shieldAbility;

    [Header("Counters")]
    public Counters cardCounters;

    [Header("Passives")]
    public CardPassives cardPassives;

    [Header("On Field Info")]
    public bool firstTurn = true;
    public bool pendulumTurn = true;
    public bool abilityUsed = true;
    public int turnsInPlay = 999999;

    private void Awake()
    {
        basePower = power;
        cardImage = ImageHelper.GetCardImage(imageID);
        if(SceneManager.GetActiveScene().name == "Battlefield")
        {
            activeAbility = cardAbilities.activeAbilityScript.GetScriptFromName<IActivateAbility>();
            spellAbility = cardAbilities.spellAbilityScript.GetScriptFromName<ISpellAbility>();
            weaponAbility = cardAbilities.weaponAbilityScript.GetScriptFromName<IWeaponAbility>();
            shieldAbility = cardAbilities.shieldAbilityScript.GetScriptFromName<IShieldAbility>();
            onDeathAbility = cardAbilities.onDeathAbilityScript.GetScriptFromName<IOnDeathAbility>();
            onPlayAbility = cardAbilities.onPlayAbilityScript.GetScriptFromName<IOnPlayAbility>();
            endTurnAbility = cardAbilities.endTurnAbilityScript.GetScriptFromName<IEndTurnAbility>();
        }
    }
}


