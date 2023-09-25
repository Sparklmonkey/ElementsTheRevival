using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudCode;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using Unity.Services.PlayerAccounts;
using UnityEngine;
using UnityEngine.Networking;

public delegate void CachedPlayerHandler(bool wasSuccess);
public delegate void LoginUserHandler(string responseMessage);
public delegate void CodeRedemptionHandler(CodeRedemptionResponse response);
public delegate void LoginLegacyHandler(LoginResponse response);

public delegate void GameStatUpdateHandler(bool wasSuccess);
public delegate void ArenaResponseHandler(ArenaResponse response);

public delegate void SimpleAction();

public class ApiManager : Singleton<ApiManager>
{
    public static bool IsTrainer;
    public bool isUnityUser = false;
    private string _jwtToken;
    public void LogoutUser()
    {
        AuthenticationService.Instance.SignOut(true);
    }

    private readonly string _baseUrl = "https://www.sparklmonkeygames.com/";
    private readonly string _apiKey = "ElementRevival-ApiKey";

    private AppInfo _appInfo;

    public class AppInfo
    {
        public bool IsMaintainence;
        public bool ShouldUpdate;
        public string UpdateNote;
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        DontDestroyOnLoad(gameObject);
    }

    public UnityWebRequest CreateApiPostRequest(string actionUrl, object body = null)
    {
        return CreateApiRequest(_baseUrl + actionUrl, UnityWebRequest.kHttpVerbPOST, body);
    }

    UnityWebRequest CreateApiRequest(string url, string method, object body)
    {
        string bodyString = null;
        if (body is string s)
        {
            bodyString = s; 
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
        request.SetRequestHeader("Authorization", $"Bearer ");
        request.timeout = 60;
        return request;
    }

    public async Task<LoginResponse> LoginController(LoginRequest loginRequest, string endPoint)
    {
        using var uwr = CreateApiPostRequest($"login/{endPoint}", loginRequest);
        await uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            return new() { errorMessage = ErrorCases.UnknownError };
        }
        else
        {
            var loginResponse = JsonUtility.FromJson<LoginResponse>(uwr.downloadHandler.text);
            if (loginResponse.errorMessage == ErrorCases.AllGood)
            {
                _jwtToken = loginResponse.token;
                PlayerPrefs.SetString("AccessToken", loginResponse.accessToken);
                PlayerData.LoadFromApi(loginResponse.savedData);
                return loginResponse;
            }

            return loginResponse;
        }
    }
    
    public async Task<LoginResponse> RegisterController(RegisterRequest registerRequest, string endPoint)
    {
        using var uwr = CreateApiPostRequest($"register/{endPoint}", registerRequest);
        await uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            LoginResponse loginResponse = new() { errorMessage = ErrorCases.UnknownError };
            return loginResponse;
        }
        else
        {
            var loginResponse = JsonUtility.FromJson<LoginResponse>(uwr.downloadHandler.text);
            if (loginResponse.errorMessage != ErrorCases.AllGood) return loginResponse;
            _jwtToken = loginResponse.token;
            PlayerPrefs.SetString("AccessToken", loginResponse.accessToken);
            PlayerData.LoadFromApi(loginResponse.savedData);
            return loginResponse;

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


    public async Task<bool> UpdateUserData(string inGameUserName, string oldPassword = "", string newPassword = "")
    {
        try
        {
            if (inGameUserName != PlayerData.Shared.userName)
            {
                PlayerData.Shared.userName = inGameUserName;
                await AuthenticationService.Instance.UpdatePlayerNameAsync(inGameUserName);
                await SaveGameData();
            }
            if (oldPassword != "")
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

    public async Task GetAppInfo()
    {
        var arguments = new Dictionary<string, object> { { "appVersion", Application.version } };
        _appInfo = await CloudCodeService.Instance.CallEndpointAsync<AppInfo>("get-app-info", arguments);
        Debug.Log(_appInfo);
    }

    public async Task SaveGameStats(GameStatRequest gameStatRequest)
    {
        using var uwr = CreateApiPostRequest($"user-data/update-game-stats", gameStatRequest);
        await uwr.SendWebRequest();
    }

    public async Task SaveGameData()
    {
        using var uwr = CreateApiPostRequest($"user-data/save-data", PlayerData.Shared);
        await uwr.SendWebRequest();
    }
}

[Serializable]
public class RegisterRequest
{
    public string username;
    public string password;
    public string email;
    public PlayerData dataToLink;
}


public enum LoginType
{
    Unity,
    UserPass,
    RegisterUserPass,
    RegisterUnity,
    LinkUserPass
}