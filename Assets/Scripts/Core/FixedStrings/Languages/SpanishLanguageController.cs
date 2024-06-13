namespace Core.FixedStrings
{
    public class SpanishLanguageController : ILanguageStringController
    {
        public string SplashUnknownFailureButtonTitle => "Go To Login";
        public string SplashMaintenanceButtonTitle => "Close App";
        public string SplashForcedUpdateButtonTitle => "Update";
        public string SplashUnknownFailureModalTitle => "There was an unknown error. You can still try logging in and play, but it may not save your progress.";
        public string SplashMaintenanceModalTitle => "The app or server is currently under maintenance. Please try again later.";
        public string SplashForcedUpdateModalTitle => "There is a new version. Please update in order to continue.";
        
    }
}