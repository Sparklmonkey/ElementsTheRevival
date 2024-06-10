using System.Collections.Generic;

namespace Core
{
    public class SessionManager
    {
        public static SessionManager Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new SessionManager();
                }

                return _instance;
            }
        }

        private static SessionManager _instance;

        public bool ShouldHideConfirm = false;
        public List<GameNews> GameNews = new();
    }
}