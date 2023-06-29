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
public delegate void CodeRedeemSuccessHandler(CodeRedemptionResponse codeRedemptionResponse);
public delegate void CodeRedeemFailureHandler(CodeRedemptionResponse codeRedemptionResponse);
public delegate void GameStatUpdateHandler(bool wasSuccess);
public delegate void GetArenaDeckHandler(ArenaResponse response);

public delegate void SimpleAction();

public class ApiManager : MonoBehaviour
{

    public GameObject touchBlocker;
    public static bool isTrainer;
    public static ApiManager shared;
    private string BaseUrl = "https://www.sparklmonkeygames.com/";
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
            LoginResponse loginResponse = new LoginResponse { errorMessage = ErrorCases.UnknownError };
            loginFailure(loginResponse);
        }
        else
        {
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

    public IEnumerator RefreshToken(bool isRetry)
    {
        if (!isRetry) { yield return new WaitForSecondsRealtime(840); }

        using UnityWebRequest uwr = shared.CreateApiGetRequest("UserData/refresh-token");
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.Success)
        {
            SetToken(uwr.downloadHandler.text);
        }
        else
        {
            StartCoroutine(RefreshToken(true));
        }
    }
    public IEnumerator UpdateGameStats(GameStatRequest request, GameStatUpdateHandler handler)
    {
        using UnityWebRequest uwr = shared.CreateApiPostRequest("UserData/update-stats", request);
        yield return uwr.SendWebRequest();

        handler(uwr.result == UnityWebRequest.Result.Success);
    }

    public IEnumerator GetOpponentDeck(GetArenaDeckHandler handler)
    {
        using UnityWebRequest uwr = shared.CreateApiGetRequest("UserData/get-opponent");
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            handler(new ArenaResponse());
        }
        else
        {
            ArenaResponse arenaResponse = JsonUtility.FromJson<ArenaResponse>(uwr.downloadHandler.text);
            handler(arenaResponse);
            yield break;
        }
    }

    public IEnumerator CheckAppVersion(LoginRequest loginRequest, LoginSuccessHandler loginSuccess, LoginFailureHandler loginFailure)
    {
        using UnityWebRequest uwr = shared.CreateApiPostRequest("Login/version", loginRequest);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            LoginResponse loginResponse = new LoginResponse { errorMessage = ErrorCases.UnknownError };
            loginFailure(loginResponse);
        }
        else
        {
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
        shared = this;
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
            AccountResponse accountResponse = new AccountResponse { errorMessage = ErrorCases.UnknownError };
            failureHandler(accountResponse);
        }
        else
        {
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

    public void SetToken(string token)
    {
        this.token = token;

        StartCoroutine(RefreshToken(false));
    }

    public void SetPlayerId(string id) { shared.playerID = id; }
    public string GetPlayerId() => shared.playerID;




    public IEnumerator CheckRedeemCode(CodeRedemptionRequest requestToSend, CodeRedeemSuccessHandler successHandler, CodeRedeemFailureHandler failureHandler)
    {
        requestToSend.Token = token;
        UnityWebRequest uwr = shared.CreateApiPostRequest("UserData/redeem-code", requestToSend);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            CodeRedemptionResponse codeRedemptionResponse = new CodeRedemptionResponse { wasValidCode = false };
            failureHandler(codeRedemptionResponse);
        }
        else
        {
            CodeRedemptionResponse codeRedemptionResponse = JsonUtility.FromJson<CodeRedemptionResponse>(uwr.downloadHandler.text);
            if (codeRedemptionResponse.wasValidCode)
            {
                successHandler(codeRedemptionResponse);
                yield break;
            }

            failureHandler(codeRedemptionResponse);
            yield break;
        }
    }


    public UnityWebRequest CreateApiGetRequest(string actionUrl)
    {
        return CreateApiRequest(BaseUrl + actionUrl, UnityWebRequest.kHttpVerbGET, null);
    }

    public UnityWebRequest CreateApiPostRequest(string actionUrl, object body = null)
    {
        return CreateApiRequest(BaseUrl + actionUrl, UnityWebRequest.kHttpVerbPOST, body);
    }

    UnityWebRequest CreateApiRequest(string url, string method, object body)
    {
        string bodyString = null;
        if (body is string)
        {
            bodyString = (string)body;
        }
        else if (body != null)
        {
            bodyString = JsonUtility.ToJson(body);
        }
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