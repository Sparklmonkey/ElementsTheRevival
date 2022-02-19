using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


public delegate void AccountSuccessHandler(AccountResponse accountResponse);
public delegate void AccountFailureHandler(AccountResponse accountResponse);
public delegate void LoginSuccessHandler(LoginResponse loginResponse);
public delegate void LoginFailureHandler(LoginResponse loginResponse);

public delegate void SimpleAction();

public class ApiManager : MonoBehaviour
{

    public GameObject touchBlocker;
    public static ApiManager shared;
    private string BaseUrl = "https://elementswebapi.herokuapp.com/";//"https://elementstheresource.azure-api.net/"; //"http://localhost:5000/";
    private string apiKey = "ElementRevival-ApiKey";
    private string token = "";
    private string playerID = "";
    private string emailAddress = "";
    private string userName = "";
    public IEnumerator Login(LoginRequest loginRequest, LoginSuccessHandler loginSuccess, LoginFailureHandler loginFailure)
    {
        using UnityWebRequest uwr = shared.CreateApiPostRequest("Login/login", loginRequest);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            //errorMessageView.SetActive(true);
            //string errorMessage = "Something went wrong. Try again in a bit.";
            //errorMessageText.text = errorMessage;
            Debug.Log("Error While Sending: " + uwr.error);
            LoginResponse loginResponse = new LoginResponse { errorMessage = ErrorCases.UnknownError };
            loginFailure(loginResponse);
        }
        else
        {
            Debug.Log(uwr.downloadHandler.text);
            LoginResponse loginResponse = JsonUtility.FromJson<LoginResponse>(uwr.downloadHandler.text);
            if (loginResponse.errorMessage == ErrorCases.AllGood)
            {
                loginSuccess(loginResponse);
                yield break;
            }

            loginFailure(loginResponse);
            yield break;
        }
    }
    public IEnumerator RegisterUser(string endpoint, LoginRequest requestToSend, LoginSuccessHandler successHandler, LoginFailureHandler failureHandler)
    {
        UnityWebRequest uwr = shared.CreateApiPostRequest(endpoint, requestToSend);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            LoginResponse loginResponse = new LoginResponse { errorMessage = ErrorCases.UnknownError };
            failureHandler(loginResponse);
        }
        else
        {
            Debug.Log(uwr.downloadHandler.text);
            LoginResponse loginResponse = JsonUtility.FromJson<LoginResponse>(uwr.downloadHandler.text);
            if (loginResponse.errorMessage == ErrorCases.AllGood)
            {
                successHandler(loginResponse);
                yield break;
            }

            failureHandler(loginResponse);
            yield break;
        }
    }

    public void SetUsernameAndEmail(string username, string emailAddress)
    {
        userName = username;
        this.emailAddress = emailAddress;
    }

    private void Start()
    {

        if (shared != null)
        {
            return;
        }
        shared = new ApiManager();
        DontDestroyOnLoad(gameObject);
    }

    public (string, string) GetEmailAndUsername() => (emailAddress, userName);

    public IEnumerator SaveToApi(AccountSuccessHandler successHandler, AccountFailureHandler failureHandler)
    {
        AccountRequest requestToSend = new AccountRequest
        {
            PlayerId = shared.GetPlayerId(),
            SavedData = PlayerData.shared,
            Token = token
        };

        using UnityWebRequest uwr = shared.CreateApiPostRequest("UserData/save-game", requestToSend);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            AccountResponse accountResponse = new AccountResponse { errorMessage = ErrorCases.UnknownError };
            failureHandler(accountResponse);
        }
        else
        {
            Debug.Log(uwr.downloadHandler.text);
            AccountResponse accountResponse = JsonUtility.FromJson<AccountResponse>(uwr.downloadHandler.text);
            if (accountResponse.errorMessage == ErrorCases.AllGood)
            {
                token = accountResponse.token;
                successHandler(accountResponse);
                yield break;
            }

            failureHandler(accountResponse);
            yield break;
        }
    }

    public IEnumerator UpdateUserAccount(AccountRequest accountRequest, AccountSuccessHandler successHandler, AccountFailureHandler failureHandler)
    {
        accountRequest.Token = token;
        accountRequest.PlayerId = playerID;

        using UnityWebRequest uwr = shared.CreateApiPostRequest("UserData/update-account", accountRequest);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log(uwr.downloadHandler.text);
            AccountResponse accountResponse = JsonUtility.FromJson<AccountResponse>(uwr.downloadHandler.text);
            if (accountResponse.errorMessage == ErrorCases.AllGood)
            {
                successHandler(accountResponse);
                yield break;
            }

            failureHandler(accountResponse);
            yield break;
        }
    }

    public IEnumerator UpdateOracleDay(SimpleAction simpleAction)
    {
        using UnityWebRequest uwr = UnityWebRequest.Get("https://www.microsoft.com");
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            PlayerData.shared.playedOracleToday = true;
            simpleAction();
            yield break;
        }
           
        var response = uwr.GetResponseHeaders();
        string todaysDates = response["date"];
        DateTime currentDate = DateTime.ParseExact(todaysDates,
                                   "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                                   CultureInfo.InvariantCulture.DateTimeFormat,
                                   DateTimeStyles.AssumeUniversal);
        PlayerData.shared.dayLastOraclePlay = currentDate;
        if (currentDate.Day > PlayerData.shared.dayLastOraclePlay.Day)
        {
            PlayerData.shared.playedOracleToday = false;
            PlayerData.shared.dayLastOraclePlay = currentDate;
        }
        else
        {
            PlayerData.shared.playedOracleToday = true;
        }
        simpleAction();
    }

    public void SetToken(string token)
    {
        this.token = token;
    }

    public void SetPlayerId(string id) { shared.playerID = id; }
    public string GetPlayerId() => shared.playerID;

    // Start is called before the first frame update
    public UnityWebRequest CreateApiGetRequest(string actionUrl, object body = null)
    {
        return CreateApiRequest(BaseUrl + actionUrl, UnityWebRequest.kHttpVerbGET, body);
    }

    public UnityWebRequest CreateApiPostRequest(string actionUrl, object body = null)
    {
        return CreateApiRequest(BaseUrl + actionUrl, UnityWebRequest.kHttpVerbPOST, body);
    }

    UnityWebRequest CreateApiRequest(string url, string method, object body)
    {
        PlayerPrefs.SetString("IsOnline", "True");
        string bodyString = null;
        if (body is string)
        {
            bodyString = (string)body;
        }
        else if (body != null)
        {
            bodyString = JsonUtility.ToJson(body);
        }
        Debug.Log(bodyString);
        var request = new UnityWebRequest();
        request.url = url;
        request.method = method;
        request.downloadHandler = new DownloadHandlerBuffer();
        request.uploadHandler = new UploadHandlerRaw(string.IsNullOrEmpty(bodyString) ? null : Encoding.UTF8.GetBytes(bodyString));
        request.SetRequestHeader("Accept", "application/test");
        request.SetRequestHeader("Access-Control-Allow-Origin", "*");
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("x-elementsrevival-apikey", apiKey);
        request.SetRequestHeader("Authorization", $"Bearer {token}");
        request.timeout = 60;
        return request;
    }
}