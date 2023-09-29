using System;

namespace Networking
{
    [Serializable]
    public class RegisterRequest
    {
        public string username;
        public string password;
        public string email;
        public PlayerData dataToLink;
    }
}