namespace Core.FixedStrings
{
    public class SpanishLanguageController : ILanguageStringController
    {
        public string SplashUnknownFailureButtonTitle => "Ir Al Login";
        public string SplashMaintenanceButtonTitle => "Cerrar App";
        public string SplashForcedUpdateButtonTitle => "Actualizar";
        public string SplashUnknownFailureModalTitle => "Hubo un error inesperadp. Puedes intentar ingresar y jugar, pero puede ser que no se guarda los cambios.";
        public string SplashMaintenanceModalTitle => "La applicacion esta en mantenimiento. Por favor vuelva a intentar mas tarde.";
        public string SplashForcedUpdateModalTitle => "Hay una nueva version. Por favor actualiza para poder seguir.";
        
    }
}