using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
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
        private ICloudCodeManager _cloudCodeManager;
        public event Action<string, string, string, ButtonActionNoParams> OnShowPopUp;
        public event Action OnLoadLogin;

        public SplashScreenViewModel(SplashScreenModel model, 
            ICloudSaveManager cloudSaveManager)
        {
            _model = model;
            _cloudSaveManager = cloudSaveManager;
            _model.OnAnimationCompleteChanged += HandleAnimationChange;
        }

        public void Initialize()
        {
            _model.InitializePlayerPrefs();
            CardDatabase.Instance.SortCardList();
        }

        public async Task SkipSplashAnimation()
        {
            if (!_model.HasSeenSplash || _model.IsLoadingNextScene) return;
            await LoadNextScene();
        }

        private async Task<bool> SetupRemoteConfig()
        {
            await UnityServices.InitializeAsync();
            _cloudCodeManager = new CloudCodeManager();
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
            var playerSavedData = await _cloudSaveManager.LoadPlayerData();
            if (playerSavedData is null)
            {
                OnLoadLogin?.Invoke();
                return;
            }
            PlayerData.Shared = playerSavedData;
            var cardList = PlayerData.Shared.CurrentDeck.ConvertCardCodeToList();
            SceneTransitionManager.Instance.LoadScene(
                cardList.Count < 30 ? "DeckSelector" : "Dashboard");
        }

        private void HandleAnimationChange(bool isAnimationComplete)
        {
            if (!isAnimationComplete) return;
            StartTitleAnimation();
        }

        private async void StartTitleAnimation()
        {
            await LoadNextScene();
        }
    }
}