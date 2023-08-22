using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LoginScreen_SetupManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private TMP_InputField username, password, newUsername, newPassword;
    [SerializeField]
    private TextMeshProUGUI errorMessage, versionLabel;
    [SerializeField]
    private GameObject appUpdatePopUp, maintainancePopUp, selectNewUserPassword;
    private GameObject touchBlocker;

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

    public async void PlayAsGuest()
    {
        if(await ApiManager.shared.LoginAsGuest())
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
        touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));
        touchBlocker.transform.SetAsFirstSibling();
        await ApiManager.shared.SignInWithUsernamePasswordAsync(username.text, password.text, HandleUserLogin);
    }

    public async void AttemptToLoginUnity()
    {
        touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));
        touchBlocker.transform.SetAsFirstSibling();
        await ApiManager.shared.SignInWithUnityAsync(HandleUserLogin);
    }

    public void HandleUserLogin(string responseMessage)
    {
        touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(touchBlocker);
        if (responseMessage == "Success")
        {
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
            errorMessage.text = responseMessage;
        }
    }


    private async void HandleLegacyUserLogin(LoginResponse response)
    {
        touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(touchBlocker);

        if (username.text.UsernameCheck() && password.text.PasswordCheck())
        {
            await ApiManager.shared.SignUpWithUsernamePasswordAsync(username.text, password.text, HandleUserRegistration);
            return;
        }
        selectNewUserPassword.SetActive(true);
    }

    public async void UpdateUsernamePassword()
    {
        if (newUsername.text.UsernameCheck() && newPassword.text.PasswordCheck())
        {
            await ApiManager.shared.SignUpWithUsernamePasswordAsync(newUsername.text, newPassword.text, HandleUserRegistration);
            return;
        }
    }

    public async void HandleUserLoginWithLegacyFallback()
    {
        LoginRequest loginRequest = new()
        {
            Username = username.text,
            Password = password.text,
            EmailAddress = "",
            OtpCode = "",
            Platform = $"{Application.platform}",
            AppVersion = $"{Application.version}"
        };
        await ApiManager.shared.LoginLegacy(loginRequest, HandleLegacyUserLogin);
    }

    public void HandleUserRegistration(string responseMessage)
    {
        touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(touchBlocker);
        if (responseMessage == "Success")
        {
            PlayerData.shared = new();
            PlayerData.shared.userName = username.text;
            GetComponent<DashboardSceneManager>().LoadNewScene("DeckSelector");
        }
        else
        {
            errorMessage.text = responseMessage;
        }
    }
}
