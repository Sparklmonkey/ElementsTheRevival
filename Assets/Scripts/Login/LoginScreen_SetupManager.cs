using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginScreenSetupManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private TMP_InputField username, password, newUsername, newPassword;
    [SerializeField]
    private TextMeshProUGUI errorMessage, versionLabel;
    [SerializeField]
    private GameObject appUpdatePopUp, maintainancePopUp, selectNewUserPassword;
    private GameObject _touchBlocker;

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
            AttemptToLoginUsernamePassword();
        }
    }

    void Start()
    {
        fields = new List<TMP_InputField> { username, password };
        ApiManager.IsTrainer = false;
        SoundManager.Instance.PlayBGM("LoginScreen");
        username.text = PlayerPrefs.HasKey("SavedUser") ? PlayerPrefs.GetString("SavedUser") : "";
        versionLabel.text = $"Version {Application.version}";
    }

    public void PlayAsTrainer()
    {
        ApiManager.IsTrainer = true;
        PlayerData.Shared = new PlayerData();
        List<string> simpleList = CardDatabase.Instance.TrainerCardList;
        List<string> fullList = new List<string>(simpleList);
        fullList.AddRange(simpleList);
        fullList.AddRange(simpleList);
        fullList.AddRange(simpleList);
        fullList.AddRange(simpleList);
        fullList.AddRange(simpleList);
        fullList.Sort((x, y) => string.Compare(x, y));
        PlayerData.Shared.cardInventory = new List<string>(fullList);

        PlayerData.Shared.currentDeck = StarterDecks.Instance.GetStarterDeck(Element.Darkness);
        PlayerData.Shared.markElement = Element.Darkness;
        PlayerData.Shared.currentQuestIndex = 8;
        PlayerData.Shared.electrum = 9999999;
        GetComponent<DashboardSceneManager>().LoadNewScene("Dashboard");
    }

    public async void PlayAsGuest()
    {
        if (await ApiManager.Instance.LoginAsGuest())
        {
            GetComponent<DashboardSceneManager>().LoadNewScene("Dashboard");
        }
        else
        {
            GetComponent<DashboardSceneManager>().LoadNewScene("DeckSelector");
        }
    }

    public async void AttemptToLoginUsernamePassword()
    {
        _touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));
        _touchBlocker.transform.SetAsFirstSibling();
        await ApiManager.Instance.UserLoginAsync(LoginType.UserPass, HandleUserLogin, username.text, password.text);
    }

    public async void AttemptToLoginUnity()
    {
        _touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));
        _touchBlocker.transform.SetAsFirstSibling();
        await ApiManager.Instance.UserLoginAsync(LoginType.Unity, HandleUserLogin);
    }

    public void HandleUserLogin(string responseMessage)
    {
        if (responseMessage == "Success")
        {
            _touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
            Destroy(_touchBlocker);
            if (PlayerData.Shared.currentDeck.Count < 30)
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
            HandleUserLoginWithLegacyFallback();
        }
    }


    private async void HandleLegacyUserLogin(LoginResponse response)
    {

        if (username.text.UsernameCheck() && password.text.PasswordCheck() && await ApiManager.Instance.CheckUsername(username.text))
        {
            await ApiManager.Instance.UserLoginAsync(LoginType.RegisterUserPass, HandleUserRegistration, username.text, password.text);
            await ApiManager.Instance.SaveDataToUnity();
            return;
        }
        _touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(_touchBlocker);
        selectNewUserPassword.SetActive(true);
    }

    public async void UpdateUsernamePassword()
    {
        _touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));
        _touchBlocker.transform.SetAsFirstSibling();
        if (newUsername.text.UsernameCheck() && newPassword.text.PasswordCheck() && await ApiManager.Instance.CheckUsername(username.text))
        {
            await ApiManager.Instance.UserLoginAsync(LoginType.UserPass, HandleUserRegistration, username.text, password.text);
            return;
        }
    }

    public async void HandleUserLoginWithLegacyFallback()
    {
        LoginRequest loginRequest = new()
        {
            username = username.text,
            password = password.text,
            emailAddress = "",
            otpCode = "",
            platform = $"{Application.platform}",
            appVersion = $"{Application.version}"
        };
        await ApiManager.Instance.LoginLegacy(loginRequest, HandleLegacyUserLogin);
    }

    public void HandleUserRegistration(string responseMessage)
    {
        _touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(_touchBlocker);
        if (responseMessage == "Success")
        {
            GetComponent<DashboardSceneManager>().LoadNewScene("DeckSelector");
        }
        else
        {
            errorMessage.text = responseMessage;
        }
    }
}
