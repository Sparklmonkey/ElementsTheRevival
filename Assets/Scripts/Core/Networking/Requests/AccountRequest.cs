using System;

[Serializable]
public class AccountRequest
{
    public string PlayerId;
    public string NewEmailAddress;
    public string Username;
    public string OldPassword;
    public string NewPassword;
    public PlayerData SavedData;
    public string Token;
}


