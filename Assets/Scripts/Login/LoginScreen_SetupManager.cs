using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LoginScreen_SetupManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private TMP_InputField username, password;
    [SerializeField]
    private TextMeshProUGUI errorMessage, versionLabel;
    [SerializeField]
    private GameObject appUpdatePopUp, maintainancePopUp;
    private GameObject touchBlocker;
    private delegate void SuccessHandler(LoginResponse loginResponse);
    private delegate void FailureHandler(LoginResponse loginResponse);

    public List<TMP_InputField> fields;
    int _fieldIndexer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (fields.Count <= _fieldIndexer)
            {
                _fieldIndexer = 0;
            }
            fields[_fieldIndexer].Select();
            _fieldIndexer++;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AttemptToLogin();
        }
    }


    void Start()
    {
        fields = new List<TMP_InputField> { username, password };
        ApiManager.isTrainer = false;
        Game_SoundManager.shared.PlayBGM("LoginScreen");
        username.text = PlayerPrefs.HasKey("SavedUser") ? PlayerPrefs.GetString("SavedUser") : "";
        versionLabel.text = $"Version {Application.version}";
    }

    public void PlayAsTrainer()
    {
        ApiManager.isTrainer = true;
        PlayerData.shared = new PlayerData();
        List<string> simpleList = CardDatabase.Instance.trainerCardList;
        List<string> fullList = new List<string>(simpleList);
        fullList.AddRange(simpleList);
        fullList.AddRange(simpleList);
        fullList.AddRange(simpleList);
        fullList.AddRange(simpleList);
        fullList.AddRange(simpleList);
        fullList.Sort((x, y) => string.Compare(x, y));
        PlayerData.shared.cardInventory = new List<string>(fullList);

        PlayerData.shared.currentDeck = Resources.Load<StarterDeck>($"StarterDecks/Darkness").deck.SerializeCard();
        PlayerData.shared.markElement = Element.Darkness;
        PlayerData.shared.currentQuestIndex = 8;
        PlayerData.shared.electrum = 9999999;
        GetComponent<DashboardSceneManager>().LoadNewScene("Dashboard");
    }

    public void PlayAsGuest()
    {
        PlayerPrefs.SetInt("IsGuest", 1);
        if (PlayerPrefs.HasKey("SaveData"))
        {
            PlayerData.LoadData();
            if (PlayerData.shared.currentDeck.Count < 30)
            {
                GetComponent<DashboardSceneManager>().LoadNewScene("DeckSelector");
            }
            else
            {
                GetComponent<DashboardSceneManager>().LoadNewScene("Dashboard");
            }
        }
        else
        {
            PlayerData.shared = new PlayerData();
            GetComponent<DashboardSceneManager>().LoadNewScene("DeckSelector");
        }
    }

    public void AttemptToLogin()
    {
        touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));
        touchBlocker.transform.SetAsFirstSibling();
        LoginRequest loginRequest = new LoginRequest()
        {
            Username = username.text,
            Password = password.text,
            EmailAddress = "",
            OtpCode = "",
            Platform = $"{Application.platform}",
            AppVersion = $"{Application.version}"
        };
        StartCoroutine(ApiManager.shared.Login(loginRequest, LoginSuccess, LoginRequestFailure));
    }

    // Success / Failure Handlers
    private void LoginSuccess(LoginResponse loginResponse)
    {
        PlayerPrefs.SetInt("IsGuest", 0);
        PlayerData.LoadFromApi(loginResponse.playerData);
        ApiManager.shared.SetPlayerId(loginResponse.playerId);
        ApiManager.shared.SetToken(loginResponse.token);
        ApiManager.shared.SetUsernameAndEmail(username.text, loginResponse.emailAddress);
        PlayerPrefs.SetString("SavedUser", username.text);
        touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(touchBlocker);
        if (PlayerData.shared.currentDeck.Count < 30)
        {
            GetComponent<DashboardSceneManager>().LoadNewScene("DeckSelector");
        }
        else
        {
            GetComponent<DashboardSceneManager>().LoadNewScene("Dashboard");
        }

    }
    public void GoToUpdateApp()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            Application.OpenURL("market://details?id=com.SparklmonkeyGames.ElementsRevival");
        }
    }
    private void LoginRequestFailure(LoginResponse loginResponse)
    {
        string errorMessage = "Something went wrong. Try again in a bit.";
        switch (loginResponse.errorMessage)
        {
            case ErrorCases.UserNameInUse:
                errorMessage = "Username is already in user. Please try a different one.";
                break;
            case ErrorCases.UserDoesNotExist:
                errorMessage = "There is no account linked to that username.";
                break;
            case ErrorCases.IncorrectPassword:
                errorMessage = "Something went wrong. The username or password is incorrect.";
                break;
            case ErrorCases.AllGood:
                errorMessage = "All Good";
                break;
            case ErrorCases.UserMismatch:
                errorMessage = "User Mismatch";
                break;
            case ErrorCases.UnknownError:
                errorMessage = "Unknown Error";
                break;
            case ErrorCases.IncorrectEmail:
                errorMessage = "The email provided is not valid. Please try again.";
                break;
            case ErrorCases.OtpIncorrect:
                errorMessage = "Failed to verify the code. We have sent a new one to try again.";
                break;
            case ErrorCases.OtpExpired:
                errorMessage = "Failed to verify the code. We have sent a new one to try again.";
                break;
            case ErrorCases.AccountNotVerified:
                errorMessage = "Your account has not been verified. We have sent a confirmation code to the email used on registration to verify your account.";
                break;
            case ErrorCases.AppUpdateRequired:
                errorMessage = "Please update game.";
                appUpdatePopUp.SetActive(true);
                break;
            case ErrorCases.ServerMaintainance:
                errorMessage = "The server is under maintainance at the moment. Please try again later.";
                maintainancePopUp.SetActive(true);
                break;
            default:
                break;
        }
        this.errorMessage.text = errorMessage;
        if(loginResponse.errorMessage != ErrorCases.AppUpdateRequired)
        {
            touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
            Destroy(touchBlocker);
        }
    }
}
