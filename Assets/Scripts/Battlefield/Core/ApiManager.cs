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
public delegate void LoginUserHandler(string responseMessage);
public delegate void CodeRedemptionHandler(CodeRedemptionResponse response);
public delegate void LoginLegacyHandler(LoginResponse response);

public delegate void GameStatUpdateHandler(bool wasSuccess);
public delegate void ArenaResponseHandler(ArenaResponse response);

public delegate void SimpleAction();

public class ApiManager : Singleton<ApiManager>
{

    public GameObject touchBlocker;

    public static bool isTrainer;
    public bool isUnityUser = false;

    public void LogoutUser()
    {
        AuthenticationService.Instance.SignOut(true);
    }

    private string BaseUrl = "https://www.sparklmonkeygames.com/";
    private string apiKey = "ElementRevival-ApiKey";
    private string token = "";
    private string playerID = "";

    public AppInfo appInfo;

    public class AppInfo
    {
        public bool isMaintainence;
        public bool shouldUpdate;
        public string updateNote;
    }

    public IEnumerator UpdateGameStats(GameStatRequest request, GameStatUpdateHandler handler)
    {
        using UnityWebRequest uwr = CreateApiPostRequest("UserData/update-stats", request);
        yield return uwr.SendWebRequest();

        handler(uwr.result == UnityWebRequest.Result.Success);
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        DontDestroyOnLoad(gameObject);
    }

    public void SetPlayerId(string id) { playerID = id; }
    public string GetPlayerId() => playerID;

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
        using UnityWebRequest uwr = CreateApiPostRequest("Login/login", loginRequest);
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
                PlayerData.LoadFromApi(loginResponse.playerData);
                loginSuccess(loginResponse);
            }

            loginSuccess(loginResponse);
        }
    }

    public async Task GetT50Opponent(ArenaResponseHandler handler)
    {
        var response = await CloudCodeService.Instance.CallEndpointAsync<ArenaResponse>("get-t50-opponent", null);
        handler(response);
    }

    public async Task CheckCodeRedemption(string redeemCode, CodeRedemptionHandler handler)
    {
        var arguments = new Dictionary<string, object> { { "redeemCode", redeemCode } };
        var response = await CloudCodeService.Instance.CallEndpointAsync<CodeRedemptionResponse>("redeem-code", arguments);
        handler(response);
    }

    public async void LoginCachedUser(CachedPlayerHandler handler)
    {
        if (AuthenticationService.Instance.IsSignedIn)
        {
            var hasData = await LoadSomeData();
            handler(hasData);
        }
        else if (AuthenticationService.Instance.SessionTokenExists)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            var hasData = await LoadSomeData();
            handler(hasData);
        }
        else
        {
            AuthenticationService.Instance.SignOut();
            handler(false);
        }
    }

    public async Task UserLoginAsync(LoginType loginType, LoginUserHandler handler, string username = "", string password = "")
    {
        try
        {
            switch (loginType)
            {
                case LoginType.Unity:
                    await AuthenticationService.Instance.SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken);
                    await LoadSomeData();
                    isUnityUser = true;
                    break;
                case LoginType.UserPass:
                    await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
                    await LoadSomeData();
                    break;
                case LoginType.RegisterUserPass:
                    await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
                    PlayerData.shared = new();
                    PlayerData.shared.userName = username;
                    await AuthenticationService.Instance.UpdatePlayerNameAsync(username);
                    await SaveDataToUnity();
                    break;
                case LoginType.RegisterUnity:
                    await AuthenticationService.Instance.SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken);
                    PlayerData.shared = new();
                    PlayerData.shared.userName = username;
                    await AuthenticationService.Instance.UpdatePlayerNameAsync(username);
                    await SaveDataToUnity();
                    isUnityUser = true;
                    break;
                case LoginType.LinkUserPass:
                    await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
                    await AuthenticationService.Instance.UpdatePlayerNameAsync(username);
                    await SaveDataToUnity();
                    break;
                default:
                    break;
            }
            handler("Success");
        }
        catch (AuthenticationException ex)
        {
            if (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
            {
                handler("Username is already in use. Please try a different one.");
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

    public async Task<bool> UpdateUserData(string inGameUserName, string oldPassword = "", string newPassword = "")
    {
        try
        {
            if(inGameUserName != PlayerData.shared.userName)
            {
                PlayerData.shared.userName = inGameUserName;
                await AuthenticationService.Instance.UpdatePlayerNameAsync(inGameUserName);
                await SaveDataToUnity();
            }
            if(oldPassword != "")
            {
                await AuthenticationService.Instance.UpdatePasswordAsync(oldPassword, newPassword);
            }
            return true;
        }

        catch (AuthenticationException)
        {
            return false;
        }
        catch (RequestFailedException)
        {
            return false;
        }
    }

    public async Task<bool> CheckUsername(string username)
    {
        var arguments = new Dictionary<string, object> { { "username", username } };
        var response = await CloudCodeService.Instance.CallEndpointAsync<bool>("check-username", arguments);
        return response;
    }

    public async Task GetAppInfo()
    {
        var arguments = new Dictionary<string, object> { { "appVersion", Application.version } };
        appInfo = await CloudCodeService.Instance.CallEndpointAsync<AppInfo>("get-app-info", arguments);
        Debug.Log(appInfo);
    }

    public async Task SavePlayerScore()
    {
        var arguments = new Dictionary<string, object> { { "playerScore", PlayerData.shared.playerScore } };
        await CloudCodeService.Instance.CallEndpointAsync("update-player-score", arguments);
    }

    public async Task SaveDataToUnity()
    {
        await SavePlayerScore();
        var data = new Dictionary<string, object> { { "SaveData", PlayerData.shared } };
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    }

    public async Task<bool> LoginAsGuest()
    {
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        return await LoadSomeData();
    }

    public async Task<bool> LoadSomeData()
    {
        await GetAppInfo();
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


public enum LoginType
{
    Unity,
    UserPass,
    RegisterUserPass,
    RegisterUnity,
    LinkUserPass
}