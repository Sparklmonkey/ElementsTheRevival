using System.Collections.Generic;

public class BattleVars
{

    public void ResetBattleVars()
    {
        TurnCount = 0;
        HasToDiscard = false;
        IsSingularity = 0;
        PlayerHp = 0;
        IsSelectingTarget = false;
        SpaceTapped = false;
        IsAnimationPlaying = false;
        IsPlayerTurn = true;
        ElementalMastery = false;
        AbilityIDOrigin = null;
        AbilityCardOrigin = null;
        IsPvp = false;
        CoinFlip = 0;
        WillStart = false;
        IsTest = false;
        IsArena = false;
    }

    public static BattleVars Shared = new();
    public float AIPlaySpeed = 0.5f;
    public int AILevel = 0;
    public Element PrimaryElement = Element.Aether;
    public Element SecondaryElement = Element.Aether;
    public EnemyAi EnemyAiData;
    public bool SpaceTapped = false;
    public bool IsAnimationPlaying = false;
    public bool IsPlayerTurn = true;
    public bool ElementalMastery = false;
    public bool IsSelectingTarget = false;
    public int PlayerHp;
    public bool IsArena;
    public bool IsTest;
    public int TurnCount;
    public long GameStartInTicks;

    public int IsSingularity = 0;
    public bool HasToDiscard = false;

    //Spell, Abilities Targeting
    public Card AbilityCardOrigin;
    public ID AbilityIDOrigin;

    public bool IsPvp;
    public int CoinFlip;
    public bool WillStart;


    public void ChangePlayerTurn()
    {
        if (IsPlayerTurn)
        {
            TurnCount++;
        }
        else
        {
            SpaceTapped = false;
        }
        IsPlayerTurn = !IsPlayerTurn;
    }
}
