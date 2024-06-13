namespace Core.FixedStrings
{
    public interface ILanguageStringController
    {
        //Button titles
        public string SplashUnknownFailureButtonTitle { get; }
        public string SplashMaintenanceButtonTitle { get; }
        public string SplashForcedUpdateButtonTitle { get; }
        //Modal Titles
        public string SplashUnknownFailureModalTitle { get; }
        public string SplashMaintenanceModalTitle { get; }
        public string SplashForcedUpdateModalTitle { get; }
    }
}