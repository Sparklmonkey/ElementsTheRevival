using System;

namespace Networking
{
    [Serializable]
    public class LoginRequest
    {
        public string username;
        public string password;
        public string accessToken;
    }
}