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
        private List<Transform> _finalPositions;
        public event Action<string, string, string, ButtonActionNoParams> OnShowPopUp;
        public event Action OnLoadLogin;
        public event Action<List<Transform>, StartNextSpriteMover> OnSetupSpritePath;

        public SplashScreenViewModel(SplashScreenModel model, 
            ICloudSaveManager cloudSaveManager, 
            List<Transform> finalPositions)
        {
            _model = model;
            _cloudSaveManager = cloudSaveManager;
            _finalPositions = finalPositions;
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
            OnSetupSpritePath?.Invoke(_finalPositions, StartNextSprite);
        }

        private void StartNextSprite()
        {
            _model.CurrentIndex += 1;
            if (_model.CurrentIndex >= _finalPositions.Count) return;
            var path = _finalPositions.GetRange(0, _finalPositions.Count - _model.CurrentIndex);
            OnSetupSpritePath?.Invoke(path, StartNextSprite);
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
            // await ApiManager.Instance.CallModuleTest();
            var playerSavedData = await _cloudSaveManager.LoadPlayerData();
            PlayerData.Shared = playerSavedData;
            var cardList = PlayerData.Shared.CurrentDeck.ConvertCardCodeToList();
            SceneTransitionManager.Instance.LoadScene(
                cardList.Count < 30 ? "DeckSelector" : "Dashboard");
        }

        public async Task StartTitleAnimation()
        {
            await LoadNextScene();
        }
    }
}