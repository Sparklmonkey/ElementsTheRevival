using System.Collections.Generic;

public abstract class FieldManager
{
    public List<IDCardPair> pairList;
    public List<int> stackCountList = new();
    public List<IDCardPair> GetAllValidCardIds() => new(pairList.FindAll(x => x.HasCard()));
}
