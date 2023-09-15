using System;
using UnityEngine.Serialization;

[Serializable]
public class AccountRequest
{
    [FormerlySerializedAs("PlayerId")] public string playerId;
    [FormerlySerializedAs("NewEmailAddress")] public string newEmailAddress;
    [FormerlySerializedAs("Username")] public string username;
    [FormerlySerializedAs("OldPassword")] public string oldPassword;
    [FormerlySerializedAs("NewPassword")] public string newPassword;
    [FormerlySerializedAs("SavedData")] public PlayerData savedData;
    [FormerlySerializedAs("Token")] public string token;
}


