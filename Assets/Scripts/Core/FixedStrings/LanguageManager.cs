using System;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace Core.FixedStrings
{
    public class LanguageManager : SingletonMono<LanguageManager>
    {
        private Language _currentLanguage;
        public ILanguageStringController LanguageStringController;
        public void ChangeLanguage(Language newLanguage)
        {
            _currentLanguage = newLanguage;
            switch (newLanguage)
            {
                case Language.English:
                    LanguageStringController = new EnglishLanguageController();
                    break;
                case Language.Spanish:
                    LanguageStringController = new SpanishLanguageController(); 
                    break;
            }
        }

        private void Start()
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.English:
                    ChangeLanguage(Language.English);
                    break;
                case SystemLanguage.Spanish:
                    ChangeLanguage(Language.Spanish);
                    break;
                default:
                    ChangeLanguage(Language.English);
                    break;
            }
        }
    }
}