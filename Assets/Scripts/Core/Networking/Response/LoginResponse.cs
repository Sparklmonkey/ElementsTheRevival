using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public int electrumReward;
    public List<string> cardRewards;
    public bool wasValidCode;
    public bool isCardSelection;
    public bool isSingleUseCode;
    public PlayerData playerSavedData;
}
