using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleVars
{

    public void ResetBattleVars()
    {
        turnCount = 0;
        hasToDiscard = false;
        isSingularity = 0;
        playerHP = 0;
        isSelectingTarget = false;
        spaceTapped = false;
        isAnimationPlaying = false;
        isPlayerTurn = true;
        elementalMastery = false;
        cardOnStandBy = null;
        originId = null;
        isPvp = false;
        coinFlip = 0;
        willStart = false;
        isTest = false;
        isArena = false;
    }

    public static BattleVars shared = new BattleVars();
    public float aiPlaySpeed = 0.5f;
    public int aiLevel = 0;
    public Element primaryElement = Element.Aether;
    public Element secondaryElement = Element.Aether;
    public EnemyAi enemyAiData;
    public bool spaceTapped = false;
    public bool isAnimationPlaying = false;
    public bool isPlayerTurn = true;
    public bool elementalMastery = false;
    public bool isSelectingTarget = false;
    public int playerHP;
    public bool isArena;
    public bool isTest;
    public int turnCount;
    public long gameStartInTicks;

    public int isSingularity = 0;
    public bool hasToDiscard = false;

    //Spell, Abilities Targeting
    public Card cardOnStandBy;

    public bool IsFixedTarget()
    {
        return CardDatabase.skillsNoTarget.Contains(cardOnStandBy.skill);
    }

    public ID originId;

    public bool isPvp;
    public int coinFlip;
    public bool willStart;
    public List<QuantaObject> opponentPvpQuanta;
}
