using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleVars
{
    public static BattleVars shared = new BattleVars();
    public EnemyAi enemyAiData;
    public bool spaceTapped = false;
    public bool isAnimationPlaying = false;
    public bool isPlayerTurn = true;
    public bool elementalMastery = false;
    public bool isSelectingTarget = false;

    public int turnCount;
    public long gameStartInTicks;

    public bool isSingularity = false;
    public bool hasToDiscard = false;

    //Spell, Abilities Targeting
    public ISpellAbility spellOnStandBy;
    public IActivateAbility abilityOnStandBy;
    public ID originId;

    public int eclipseCounters = 0;
    public bool isPvp;
    public int coinFlip;
    public bool willStart;
    public List<QuantaObject> opponentPvpQuanta;
}
