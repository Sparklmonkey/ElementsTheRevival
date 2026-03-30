using System;
using System.Collections;
using System.Collections.Generic;
using Networking.Networking;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using V3.Scripts;

namespace SplashScreen
{
    public delegate void StartNextSpriteMover();
    public class SplashScreenView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Transform popUpParent;
        [SerializeField] private GameObject popUpModal, elementAnimationPrefab;

        private SplashScreenViewModel _viewModel;
        private SplashScreenModel _model;
        private ElementAnimationController _elementAnimationController;

        private async void Start()
        {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
    WebGLInput.captureAllKeyboardInput = false;
#endif
            InitializeMVVM();
        }

        private async void InitializeMVVM()
        {
            Debug.Log(" --------------Game START --------------");
            try
            {
                await UnityServices.InitializeAsync();
                Debug.Log(" --------------Initialize Complete --------------");
                _model = new SplashScreenModel();
                _viewModel = new SplashScreenViewModel(_model, new CloudSaveManager(new CloudCodeManager()));
                SetupEventListeners();
                InitializeAsync();
                _viewModel.Initialize();
            }
            catch (Exception e)
            {
                Debug.LogError("Error initializing Unity Services: " + e.Message);
                // Handle specific exceptions here
            }
        }

        private void SetupEventListeners()
        {
            _viewModel.OnShowPopUp += ShowPopUpModal;
            _viewModel.OnLoadLogin += GoToLogin;
        }

        private void InitializeAsync()
        {
            SoundManager.Instance.PlayBGM("LoginScreen");
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;
            _elementAnimationController = Instantiate(elementAnimationPrefab, new Vector3(0,1,0), Quaternion.identity).GetComponent<ElementAnimationController>();
            _elementAnimationController.SetupAnimationController(_model);
        }

        public async void SkipSplashAnimation()
        {
            StopAllCoroutines();
            _elementAnimationController.EndAnimation();
            await _viewModel.SkipSplashAnimation();
        }

        private void ShowPopUpModal(string localeTable, string messageTitleKey, string buttonTitleKey, ButtonActionNoParams actionButtonMethod)
        {
            var popUpObject = Instantiate(popUpModal, popUpParent);
            popUpObject.GetComponent<PopUpModal>().SetupModal(localeTable, messageTitleKey, buttonTitleKey, actionButtonMethod);
        }

        private void GoToLogin()
        {
            Debug.Log("Just before Scene Change");
            SceneTransitionManager.Instance.LoadScene("NewLoginScreen");
        }

        private void OnDestroy()
        {
            // Cleanup event listeners
            _viewModel.OnShowPopUp -= ShowPopUpModal;
            _viewModel.OnLoadLogin -= GoToLogin;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SkipSplashAnimation();
        }
    }
}