namespace Networking
{
    public static class Endpointbuilder
    {
        public const string UserCredentialLogin = "login/credential";
        public const string UserTokenLogin = "login/token";
        public const string AppInfo = "login/app-info";
        
        public const string ArenaT50 = "arena/t50opponent";
        
        public const string UpdateGameStats = "game-stats/update";
        
        public const string RegisterNewUser = "register/new";
        public const string RegisterLinkData = "register/link";
        
        public const string RedeemCode = "save-data/redeem";
        public const string UpdateSaveData = "save-data/update";
        public const string ResetSaveData = "save-data/reset";
        
        public const string UpdateUserData = "user-data/update";
        
        public const string GameNews = "game-news";
        public const string NewsNotification = "viewed-news/has-seen";
        public const string UpdateNews = "viewed-news/update";
        
        public const string Logout = "logout";
        public const string RefreshToken = "refresh";
    }
}