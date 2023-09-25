using System;
using System.Collections.Generic;

[Serializable]
public class LoginResponse
{
    public string accessToken;
    public string emailAddress;
    public PlayerData savedData;
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
