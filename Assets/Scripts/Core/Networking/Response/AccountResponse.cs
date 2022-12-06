using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AccountResponse
{
    public string playerId;
    public string emailAddress;
    public string username;
    public PlayerData playerData;
    public ErrorCases errorMessage;
    public string token;
}