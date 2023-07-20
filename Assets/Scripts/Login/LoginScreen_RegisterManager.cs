using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginScreen_RegisterManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private TMP_InputField username, password, email;
    [SerializeField]
    private TextMeshProUGUI serverResponse;
    [SerializeField]
    private GameObject linkLocalDataPopup;
    private GameObject touchBlocker;

    private bool linkLocalData;


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
    }


    void Start()
    {
        fields = new List<TMP_InputField> { username, password, email };

    }
    public void ShouldLinkData(bool shouldLink) => linkLocalData = shouldLink;
    public void AttemptToRegister()
    {
        touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/RegisterPanel"));
        touchBlocker.transform.SetAsFirstSibling();
        LoginRequest loginRequest = new LoginRequest()
        {
            Username = username.text,
            Password = password.text,
            EmailAddress = email.text,
            OtpCode = "",
            Platform = $"{Application.platform}",
            AppVersion = $"{Application.version}"
    };
        StartCoroutine(ApiManager.shared.RegisterUser("Login/register", loginRequest, LoginSuccess, LoginRequestFailure));
    }

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("SaveData"))
        {
            linkLocalDataPopup.SetActive(true);
        }
        else
        {
            linkLocalDataPopup.SetActive(false);
        }
    }
    private void LoginSuccess(LoginResponse loginResponse)
    {
        if (linkLocalData)
        {
            PlayerData.LoadData();
            PlayerPrefs.DeleteKey("IsGuest");
            PlayerData.shared.id = loginResponse.playerData.id;
        }
        else
        {
            PlayerData.LoadFromApi(loginResponse.playerData);
            PlayerPrefs.DeleteKey("IsGuest");
        }
        ApiManager.shared.SetPlayerId(loginResponse.playerId);
        ApiManager.shared.SetToken(loginResponse.token);
        ApiManager.shared.SetUsernameAndEmail(username.text, loginResponse.emailAddress);
        PlayerPrefs.SetString("SavedUser", username.text);


        touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(touchBlocker);
        if (linkLocalData)
        {
            GetComponent<DashboardSceneManager>().LoadNewScene("Dashboard");
            return;
        }
        GetComponent<DashboardSceneManager>().LoadNewScene("DeckSelector");

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
                break;
            case ErrorCases.UserMismatch:
                break;
            case ErrorCases.UnknownError:
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
            default:
                break;
        }
        serverResponse.text = errorMessage;
        touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(touchBlocker);
    }
}