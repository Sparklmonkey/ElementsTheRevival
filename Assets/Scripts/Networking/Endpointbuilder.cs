namespace Networking
{
    public static class Endpointbuilder
    {
        public const string userCredentialLogin = "login/credential";
        public const string userTokenLogin = "login/token";
        public const string appInfo = "login/app-info";
        
        public const string arenaT50 = "arena/t50opponent";
        
        public const string updateGameStats = "game-stats/update";
        
        public const string registerNewUser = "register/new";
        public const string registerLinkData = "register/link";
        
        public const string redeemCode = "save-data/redeem";
        public const string updateSaveData = "save-data/update";
        public const string resetSaveData = "save-data/reset";
        
        public const string updateUserData = "user-data/update";
        
        public const string logout = "logout";
        public const string refreshToken = "refresh";
    }
}