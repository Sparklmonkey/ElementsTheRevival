using System.Collections.Generic;

public class PvpRequest
{
    public string PlayerId;
    public string OpponentId;
    public List<PvP_Action> ActionList;
    public string Token;
}
