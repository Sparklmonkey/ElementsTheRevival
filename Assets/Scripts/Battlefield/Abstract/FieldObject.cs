using System.Collections.Generic;

public abstract class FieldManager
{
    public List<IDCardPair> PairList;
    public List<int> StackCountList = new();
    public List<IDCardPair> GetAllValidCardIds() => new(PairList.FindAll(x => x.HasCard()));
}
