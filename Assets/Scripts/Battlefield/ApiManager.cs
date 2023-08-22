using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.CloudCode;
using Unity.Services.Core;
using System.Text;
using Unity.Services.CloudSave;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Services.PlayerAccounts;
using System.Threading.Tasks;

public delegate void CachedPlayerHandler(bool wasSuccess);
public delegate void RegisterUserHandler(string responseMessage);
public delegate void LoginUserHandler(string responseMessage);
public delegate void CodeRedemptionHandler(CodeRedemptionResponse response);
public delegate void LoginLegacyHandler(LoginResponse response);

public delegate void AccountSuccessHandler(AccountResponse accountResponse);
public delegate void AccountFailureHandler(AccountResponse accountResponse);
public delegate void LoginSuccessHandler(LoginResponse loginResponse);
public delegate void LoginFailureHandler(LoginResponse loginResponse);
public delegate void GameStatUpdateHandler(bool wasSuccess);
public delegate void GetArenaDeckHandler(ArenaResponse response);

public delegate void SimpleAction();

public class ApiManager : MonoBehaviour
{

    public GameObject touchBlocker;
    public static bool isTrainer;
    public bool isUnityUser = false;
    public static ApiManager shared;
    private string BaseUrl = "https://www.sparklmonkeygames.com/";
    private string apiKey = "ElementRevival-ApiKey";
    private string token = "";
    private string playerID = "";
    private string emailAddress = "";

    private string updateNote;

    private class UpdateNote
    {
        public string note;
    }

    public async void GetUpdateNote()
    {
        var response = await CloudCodeService.Instance.CallEndpointAsync<UpdateNote>("latest-update-note", null);
        Debug.Log(response.note);
        updateNote = response.note;
    }

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

    public void SetUsernameAndEmail(string emailAddress)
    {
        this.emailAddress = emailAddress;
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        await PlayerAccountService.Instance.StartSignInAsync();
        if (shared != null)
        {
            return;
        }
        shared = this;
        DontDestroyOnLoad(gameObject);
    }

    private async void SignInWithUnityTest()
    {
        await UnityServices.InitializeAsync();
        PlayerAccountService.Instance.SignedIn += SignInWithUnity;
        if (AuthenticationService.Instance.SessionTokenExists)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            //LoadSomeData();
        }
        else
        {
            await PlayerAccountService.Instance.StartSignInAsync();
        }

    }

    public async Task SaveDataToUnity()
    {
        var data = new Dictionary<string, object> { { "SaveData", PlayerData.shared } };
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    }

    async void SignInWithUnity()
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken);
            //LoadSomeData();
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    public string GetEmail() => emailAddress;

    private async void UnityCloudSave()
    {
        var data = new Dictionary<string, object> { { "SaveData", PlayerData.shared } };
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    }

    public IEnumerator SaveToApi(AccountSuccessHandler successHandler, AccountFailureHandler failureHandler)
    {
        UnityCloudSave();
        successHandler(new());
        yield break;

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
        var request = new UnityWebRequest
        {
            url = url,
            method = method,
            downloadHandler = new DownloadHandlerBuffer(),
            uploadHandler = new UploadHandlerRaw(string.IsNullOrEmpty(bodyString) ? null : Encoding.UTF8.GetBytes(bodyString))
        };
        request.SetRequestHeader("Accept", "application/test");
        request.SetRequestHeader("Access-Control-Allow-Origin", "*");
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("x-elementsrevival-apikey", apiKey);
        request.SetRequestHeader("Authorization", $"Bearer {token}");
        request.timeout = 60;
        return request;
    }



    public async Task LoginLegacy(LoginRequest loginRequest, LoginLegacyHandler loginSuccess)
    {
        using UnityWebRequest uwr = shared.CreateApiPostRequest("Login/login", loginRequest);
        await uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            LoginResponse loginResponse = new(){ errorMessage = ErrorCases.UnknownError };
            loginSuccess(loginResponse);
        }
        else
        {
            LoginResponse loginResponse = JsonUtility.FromJson<LoginResponse>(uwr.downloadHandler.text);
            if (loginResponse.errorMessage == ErrorCases.AllGood)
            {
                loginSuccess(loginResponse);
            }

            loginSuccess(loginResponse);
        }
    }


    public async Task CheckCodeRedemption(string redeemCode, CodeRedemptionHandler handler)
    {
        var arguments = new Dictionary<string, object> { { "redeemCode", redeemCode } };
        var response = await CloudCodeService.Instance.CallEndpointAsync<CodeRedemptionResponse>("redeem-code", arguments);
        handler(response);
    }

    public async void LoginCachedUser(CachedPlayerHandler handler)
    {
        if (AuthenticationService.Instance.SessionTokenExists)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            var hasData = await LoadSomeData();
            handler(hasData);
        }
        else
        {
            handler(false);
        }
    }

    public async Task SignUpWithUsernamePasswordAsync(string username, string password, RegisterUserHandler handler)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            handler("Success");
        }
        catch (AuthenticationException ex)
        {
            if (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
            {
                handler("Username is already in user. Please try a different one.");
            }
            else
            {
                handler("Something went wrong. Please try again later.");
            }
        }
        catch (RequestFailedException)
        {
            handler("Something went wrong. Please try again later.");
        }
    }

    public async Task SignUpWithUnityAsync(string username, RegisterUserHandler handler)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken);
            PlayerData.shared = new();
            PlayerData.shared.userName = username;
            handler("Success");
        }
        catch (AuthenticationException ex)
        {
            if (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
            {
                handler("Username is already in user. Please try a different one.");
            }
            else
            {
                handler("Something went wrong. Please try again later.");
            }
        }
        catch (RequestFailedException)
        {
            handler("Something went wrong. Please try again later.");
        }
    }

    public async Task SignInWithUsernamePasswordAsync(string username, string password, LoginUserHandler handler)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            await LoadSomeData();
            handler("Success");
        }
        catch (AuthenticationException ex)
        {
            if (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
            {
                handler("Username is already in user. Please try a different one.");
            }
            else
            {
                handler("Something went wrong. Please try again later.");
            }
        }
        catch (RequestFailedException)
        {
            handler("Something went wrong. Please try again later.");
        }
    }

    public async Task SignInWithUnityAsync(LoginUserHandler handler)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken);
            await LoadSomeData();
            handler("Success");
        }
        catch (AuthenticationException ex)
        {
            if (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
            {
                handler("Username is already in user. Please try a different one.");
            }
            else
            {
                handler("Something went wrong. Please try again later.");
            }
        }
        catch (RequestFailedException)
        {
            handler("Something went wrong. Please try again later.");
        }
    }

    public async Task<bool> LoginAsGuest()
    {
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        return await LoadSomeData();
    }

    public async Task<bool> LoadSomeData()
    {
        Dictionary<string, string> savedData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "SaveData" });
        if (savedData == null)
        {
            PlayerData.shared = new();
            return false;
        }
        else
        {
            PlayerData.shared = JsonUtility.FromJson<PlayerData>(savedData["SaveData"]);
            return true;
        }
    }
}
