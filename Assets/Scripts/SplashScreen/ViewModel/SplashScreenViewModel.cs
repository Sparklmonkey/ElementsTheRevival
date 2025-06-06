using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Networking.Networking;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace SplashScreen
{
    public class SplashScreenViewModel
    {
        private readonly SplashScreenModel _model;
        private readonly ICloudSaveManager _cloudSaveManager;
        
        public event Action OnTitleAnimationComplete;
        public event Action<string, string, string, ButtonActionNoParams> OnShowPopUp;
        public event Action OnLoadLogin;
        public event Action<List<Transform>, StartNextSpriteMover> OnSetupSpritePath;

        public SplashScreenViewModel(SplashScreenModel model, ICloudSaveManager cloudSaveManager)
        {
            _model = model;
            _cloudSaveManager = cloudSaveManager;
        }

        public async Task Initialize()
        {
            _model.InitializePlayerPrefs();
            CardDatabase.Instance.SortCardList();
            SetupInitialSprite();
            await StartTitleAnimation();
        }

        private void SetupInitialSprite()
        {
            _model.CurrentIndex = 0;
            if (OnSetupSpritePath != null)
            {
                // Note: You'll need to pass these parameters from the View
                //OnSetupSpritePath.Invoke(finalPositions, StartNextSprite);
            }
        }

        public void StartNextSprite(List<Transform> finalPositions)
        {
            _model.CurrentIndex += 1;
            if (_model.CurrentIndex >= finalPositions.Count) return;
            var path = finalPositions.GetRange(0, finalPositions.Count - _model.CurrentIndex);
            OnSetupSpritePath?.Invoke(path, () => StartNextSprite(finalPositions));
        }

        public async Task SkipSplashAnimation()
        {
            if (!_model.HasSeenSplash || _model.IsLoadingNextScene) return;
            await LoadNextScene();
        }

        private async Task<bool> SetupRemoteConfig()
        {
            await UnityServices.InitializeAsync();
            _model.IsCachedLogin = AuthenticationService.Instance.SessionTokenExists;
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            await RemoteConfigService.Instance.FetchConfigsAsync(new UserAttributes(), new AppAttributes());
            var featureFlags = RemoteConfigService.Instance.appConfig.GetJson("FeatureFlags");
            RemoteConfigHelper.Instance.SetFeatureFlags(featureFlags);
            return true;
        }

        private async Task LoadNextScene()
        {
            await SetupRemoteConfig();
            
            if (RemoteConfigHelper.Instance.IsMaintenance())
            {
                OnShowPopUp?.Invoke("SplashScreen", 
                    "SplashMaintenanceModalTitle", 
                    "SplashMaintenanceButtonTitle",
                    Application.Quit);
                return;
            }

            RemoteConfigHelper.Instance.SetupGameNews();

            if (RemoteConfigHelper.Instance.IsForceUpdate())
            {
                OnShowPopUp?.Invoke("SplashScreen",
                    "SplashForcedUpdateModalTitle",
                    "SplashForcedUpdateButtonTitle",
                    () => Application.OpenURL("market://details?id=" + Application.productName));
                return;
            }

            if (_model.IsCachedLogin)
            {
                await HandleCachedLogin();
                return;
            }

            OnLoadLogin?.Invoke();
        }

        private async Task HandleCachedLogin()
        {
            // await ApiManager.Instance.CallModuleTest();
            var playerSavedData = await _cloudSaveManager.LoadPlayerData();
            PlayerData.Shared = playerSavedData;
            var cardList = PlayerData.Shared.CurrentDeck.ConvertCardCodeToList();
            SceneTransitionManager.Instance.LoadScene(
                cardList.Count < 30 ? "DeckSelector" : "Dashboard");
        }

        private async Task StartTitleAnimation()
        {
            // This will be handled by the View
            await Task.Delay(6000); // 6 seconds
            OnTitleAnimationComplete?.Invoke();
            await LoadNextScene();
        }
    }
}