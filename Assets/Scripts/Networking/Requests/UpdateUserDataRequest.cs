using System;
using UnityEngine.Serialization;

namespace Networking
{
    [Serializable]
    public class UpdateUserDataRequest
    {
        public string username;
        public string password;
        public string newUsername;
        public string newPassword;
    }
}

