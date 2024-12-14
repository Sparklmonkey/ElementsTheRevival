using System.Collections;
using System.Collections.Generic;
using Core.FixedStrings;
using Networking;
using UnityEngine;
using UnityEngine.UI;

namespace SplashScreen
{
public class SplashScreen : MonoBehaviour
{
    public Transform finalImagePositionsParent;

    public Transform imageObjectsParent;

    public Image titleImage;

    public Material dissolveMat;

    public Sprite titleSprite;

    [SerializeField]
    private List<Transform> finalPositions;
    [SerializeField]
    private Transform popUpParent;

    [SerializeField] private GameObject popUpModal;
    [SerializeField]
    private List<GameObject> imageObjects;

    private bool _isLoadingNextScene = false;
    private bool _mustWait = true;
    private bool _dataLoaded = false;

    private IEnumerator MoveImageAround(GameObject imageToMove, int finalIndex)
    {
        if (finalIndex == 0)
        {
            while (imageToMove.transform.position != finalPositions[0].position)
            {
                imageToMove.transform.position = Vector3.MoveTowards(imageToMove.transform.position, finalPositions[0].position, 1500f * Time.deltaTime);
                yield return null;
            }
            yield break;
        }
        for (var i = 0; i < finalPositions.Count; i++)
        {
            var whileLoopBreak = 0;
            while (imageToMove.transform.position != finalPositions[i].position && whileLoopBreak < 16)
            {
                imageToMove.transform.position = Vector3.MoveTowards(imageToMove.transform.position, finalPositions[i].position, 1500f * Time.deltaTime);
                whileLoopBreak++;
                if (whileLoopBreak == 16)
                {
                    imageToMove.transform.position = finalPositions[i].position;
                }
                yield return null;
            }
            if (i == 0 && finalIndex > 0)
            {
                StartCoroutine(MoveImageAround(imageObjects[finalIndex - 1], finalIndex - 1));
            }
            if (i == finalIndex)
            {
                yield break;
            }
        }
    }

    public void GoToAppStore()
    {
        Application.OpenURL ("market://details?id=" + Application.productName);
    }
    private void Start()
    {
        PlayerPrefs.SetInt("IsGuest", 0);
        PlayerPrefs.SetInt("IsTrainer", 0);
        SoundManager.Instance.PlayBGM("LoginScreen");
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        
        if (PlayerPrefs.GetInt("HasKeysStored") != 1)
        {
            PlayerPrefs.SetInt("HasKeysStored", 1);

            PlayerPrefs.SetInt("QuickPlay", 1);
            PlayerPrefs.SetFloat("AnimSpeed", 1f);
            PlayerPrefs.SetFloat("BGMVolume", 100f);
            PlayerPrefs.SetFloat("SFXVolume", 100f);
        }

        PlayerPrefs.SetInt("IsAltArt", 0);
        if (!PlayerPrefs.HasKey("IsAltArt"))
        {
            PlayerPrefs.SetInt("IsAltArt", 0);
        }

        CardDatabase.Instance.SortCardList();
        StartCoroutine(MoveImageAround(imageObjects[11], 12));
        StartCoroutine(StartTitleAnimation());
    }

    public async void SkipSplashAnimation()
    {
        if (PlayerPrefs.GetFloat("HasSeenSplash") != 1f || _isLoadingNextScene) return;
        StopAllCoroutines();
        for (var i = 0; i < imageObjects.Count; i++)
        {
            imageObjects[i].transform.position = finalPositions[i].position;
        }
        titleImage.material.SetFloat("_Fade", 1f);
        LoadNextScene();
    }

    private async void LoadNextScene()
    {
        var appInfo = await ApiManager.Instance.GetAppInfo();
        if (appInfo == null)
        {
            var popUpObject = Instantiate(popUpModal, popUpParent);
            popUpObject.GetComponent<PopUpModal>().SetupModal(LanguageManager.Instance.LanguageStringController.SplashUnknownFailureModalTitle, 
                LanguageManager.Instance.LanguageStringController.SplashUnknownFailureButtonTitle,
                GoToLogin);
            return;
        }

        if (appInfo.isMaintenance)
        {
            var popUpObject = Instantiate(popUpModal, popUpParent);
            popUpObject.GetComponent<PopUpModal>().SetupModal(LanguageManager.Instance.LanguageStringController.SplashMaintenanceModalTitle, 
                LanguageManager.Instance.LanguageStringController.SplashMaintenanceButtonTitle,
                CloseApp);
            return;
        }
        
        if (appInfo.shouldUpdate)
        {
            var popUpObject = Instantiate(popUpModal, popUpParent);
            popUpObject.GetComponent<PopUpModal>().SetupModal(LanguageManager.Instance.LanguageStringController.SplashForcedUpdateModalTitle, 
                LanguageManager.Instance.LanguageStringController.SplashForcedUpdateButtonTitle,
                GoToAppStore);
            return;
        }
        await ApiManager.Instance.GetGameNews();
        if (PlayerPrefs.HasKey("AccessToken"))
        { 
            var token = PlayerPrefs.GetString("AccessToken"); 
            var response = await ApiManager.Instance.LoginController(new LoginRequest() 
            {
                accessToken = token
            }, Endpointbuilder.UserTokenLogin);
            ManageResponse(response);
            return;
        }
        GoToLogin();
    }

    public void GoToLogin()
    {
        SceneTransitionManager.Instance.LoadScene("LoginScreen");
    }
    
    public void CloseApp()
    {
        Application.Quit();
    }

    private void ManageResponse(LoginResponse response)
    {
        if (response.errorMessage == ErrorCases.AllGood)
        {
            PlayerData.LoadFromApi(response.savedData);
            PlayerData.Shared.email = response.emailAddress;
            PlayerPrefs.SetString("AccessToken", response.accessToken);
            PlayerData.Shared = response.savedData;
            PlayerData.Shared.username = response.username;
            
            SceneTransitionManager.Instance.LoadScene("Dashboard");
        }
        else
        {
            SceneTransitionManager.Instance.LoadScene("LoginScreen");
        }
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
        LoadNextScene();
    }
}
}