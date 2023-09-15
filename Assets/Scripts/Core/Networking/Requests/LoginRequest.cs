using System;
using UnityEngine.Serialization;

[Serializable]
public class LoginRequest
{
    [FormerlySerializedAs("Username")] public string username;
    [FormerlySerializedAs("Password")] public string password;
    [FormerlySerializedAs("EmailAddress")] public string emailAddress;
    [FormerlySerializedAs("OtpCode")] public string otpCode;
    [FormerlySerializedAs("Platform")] public string platform;
    [FormerlySerializedAs("AppVersion")] public string appVersion;
}


[Serializable]
public class CodeRedemptionRequest
{
    [FormerlySerializedAs("Token")] public string token;
    [FormerlySerializedAs("PlayerSavedData")] public PlayerData playerSavedData;
    [FormerlySerializedAs("CodeValue")] public string codeValue;
}