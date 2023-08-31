using System.Collections.Generic;
using System;

[Serializable]
public class LoginResponse
{
    public string playerId;
    public string emailAddress;
    public PlayerData playerData;
    public ErrorCases errorMessage;
    public string token;
}

[Serializable]
public class CodeRedemptionResponse
{
    public string errorMessage;
    public int electrumReward;
    public List<string> cardRewards;
    public bool isCardSelection;
    public bool canRedeem;
}
