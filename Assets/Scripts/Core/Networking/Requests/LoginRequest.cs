using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

