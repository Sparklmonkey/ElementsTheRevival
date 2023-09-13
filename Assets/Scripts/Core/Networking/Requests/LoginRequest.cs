using System;

[Serializable]
public class LoginRequest
{
    public string Username;
    public string Password;
    public string EmailAddress;
    public string OtpCode;
    public string Platform;
    public string AppVersion;
}


[Serializable]
public class CodeRedemptionRequest
{
    public string Token;
    public PlayerData PlayerSavedData;
    public string CodeValue;
}