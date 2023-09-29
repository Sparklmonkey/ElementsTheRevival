using System;

namespace Networking
{
    [Serializable]
    public class LoginResponse
    {
        public string username;
        public string accessToken;
        public string emailAddress;
        public PlayerData savedData;
        public ErrorCases errorMessage;
        public string token;
    }
}
