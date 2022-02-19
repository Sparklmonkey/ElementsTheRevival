using UnityEngine;
using TMPro;

public class LoginScreen_SetupManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private TMP_InputField username, password;
    [SerializeField]
    private TextMeshProUGUI errorMessage;
    private GameObject touchBlocker;
    private delegate void SuccessHandler(LoginResponse loginResponse);
    private delegate void FailureHandler(LoginResponse loginResponse);


    void Start()
    {
        Game_SoundManager.PlayBGM("LoginScreen");
        username.text = PlayerPrefs.HasKey("SavedUser") ? PlayerPrefs.GetString("SavedUser") : "";
    }

    public void PlayAsGuest()
    {
        PlayerPrefs.SetInt("IsGuest", 1);
        if (PlayerPrefs.HasKey("SaveData"))
        {
            PlayerData.LoadData();
            if (PlayerData.shared.currentDeck.Count == 0)
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
            OtpCode = ""
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
        if (PlayerData.shared.currentDeck.Count == 0)
        {
            GetComponent<DashboardSceneManager>().LoadNewScene("DeckSelector");
        }
        else
        {
            GetComponent<DashboardSceneManager>().LoadNewScene("Dashboard");
        }
        Debug.Log(PlayerData.shared.markElement);

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
            default:
                break;
        }
        this.errorMessage.text = errorMessage;
        Destroy(touchBlocker.gameObject);
        Debug.Log(errorMessage);
    }
}
