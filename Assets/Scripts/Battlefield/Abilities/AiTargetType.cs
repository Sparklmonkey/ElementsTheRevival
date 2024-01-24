public class AiTargetType
{
    public int Estimate;
    public bool OnlyFriend;
    public bool OnlyFoe;
    public bool Freeze;
    public TargetType Targeting;
    public int DefineValue;
    public int DefTolerance;
    public AiTargetType(bool onlyFriend, bool onlyFoe, bool freeze, TargetType targeting, int estimate, int defineValue, int defTolerance)
    {
        Estimate = estimate;
        OnlyFriend = onlyFriend;
        OnlyFoe = onlyFoe;
        Freeze = freeze;
        Targeting = targeting;
        DefineValue = defineValue;
        DefTolerance = defTolerance;
    }
}