using System;
using System.Collections;
using System.Collections.Generic;
using Networking.Networking;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace SplashScreen
{
    public class SplashScreenView : MonoBehaviour, IPointerClickHandler
    {
        public Image titleImage;
        public Sprite titleSprite;
        [SerializeField] private List<Transform> finalPositions;
        [SerializeField] private Transform popUpParent;
        [SerializeField] private GameObject popUpModal;
        [SerializeField] private List<SpriteMover> spriteObjects;

        private SplashScreenViewModel _viewModel;
        private SplashScreenModel _model;

        private async void Start()
        {
            InitializeMVVM();
            SetupEventListeners();
            StartCoroutine(InitializeAsync());
            await _viewModel.Initialize();
        }

        private void InitializeMVVM()
        {
            _model = new SplashScreenModel();
            _viewModel = new SplashScreenViewModel(_model, new CloudSaveManager(new CloudCodeManager()), finalPositions);
        }

        private void SetupEventListeners()
        {
            _viewModel.OnShowPopUp += ShowPopUpModal;
            _viewModel.OnLoadLogin += GoToLogin;
            _viewModel.OnSetupSpritePath += HandleSpritePath;
        }

        private IEnumerator InitializeAsync()
        {
            SoundManager.Instance.PlayBGM("LoginScreen");
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;

            yield return StartCoroutine(StartTitleAnimation());
        }

        private void HandleSpritePath(List<Transform> animationPath, StartNextSpriteMover callback)
        {
            spriteObjects[_model.CurrentIndex].SetupSpritePath(animationPath, callback);
        }
        

        public async void SkipSplashAnimation()
        {
            StopAllCoroutines();
            titleImage.material.SetFloat("_Fade", 1f);
            await _viewModel.SkipSplashAnimation();
        }

        private void ShowPopUpModal(string localeTable, string messageTitleKey, string buttonTitleKey, ButtonActionNoParams actionButtonMethod)
        {
            var popUpObject = Instantiate(popUpModal, popUpParent);
            popUpObject.GetComponent<PopUpModal>().SetupModal(localeTable, messageTitleKey, buttonTitleKey, actionButtonMethod);
        }

        private void GoToLogin()
        {
            SceneTransitionManager.Instance.LoadScene("NewLoginScreen");
        }

        private IEnumerator StartTitleAnimation()
        {
            var shader = titleImage.material;
            shader.SetTexture("_MainTex", titleSprite.texture);
            shader.SetFloat("_Fade", 0f);
            shader.SetFloat("_Scale", 150f);
            var currentTime = 0f;
            while (currentTime < 6f)
            {
                var value = currentTime / 6f;
                currentTime += Time.deltaTime;
                shader.SetFloat("_Fade", value);
                yield return null;
            }
            shader.SetFloat("_Fade", 1f);
            _viewModel.StartTitleAnimation();
        }

        private void OnDestroy()
        {
            // Cleanup event listeners
            _viewModel.OnShowPopUp -= ShowPopUpModal;
            _viewModel.OnLoadLogin -= GoToLogin;
            _viewModel.OnSetupSpritePath -= HandleSpritePath;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SkipSplashAnimation();
        }
    }
}