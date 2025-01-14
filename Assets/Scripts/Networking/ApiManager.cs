using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Networking.Response;
using Networking;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.CloudCode;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Networking;

public delegate void LoginLegacyHandler(LoginResponse response);
public delegate void LoginUserHandler(string responseMessage);

namespace Networking
{
    public class ApiManager : SingletonMono<ApiManager>
    {
        public static bool IsTrainer => PlayerPrefs.GetInt("IsTrainer") == 1;
        private string _jwtToken;
        private string _accountId;
        public bool isUnityUser = false;

        // private readonly string _baseUrl =  "https://www.elementstherevival.com/api/";
#if UNITY_EDITOR
        private readonly string _baseUrl =  "https://www.elementstherevival.com/api/";
        // private readonly string _baseUrl =  "http://localhost:5158/api/";
#else
        private readonly string _baseUrl =  "https://www.elementstherevival.com/api/";
#endif
        
        private readonly string _apiKey = "ElementRevival-ApiKey";

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
        private UnityWebRequest CreateApiRequest(string url, string method, object body)
        {
            var bodyString = body is string s ? s : JsonUtility.ToJson(body);
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
            request.SetRequestHeader("AppVersion", Application.version);
            request.SetRequestHeader("Authorization", $"Bearer {_jwtToken}");
            request.SetRequestHeader("Account", $"{_accountId}");
            request.timeout = 60;
            return request;
        }

        public bool ShouldForceUpdate(string minVersion)
        {
            var version1 = new Version(Application.version);
            var version2 = new Version(minVersion);

            var result = version1.CompareTo(version2);
            return result < 0;
        }

        //PUT Requests

        public async Task<CodeRedemptionResponse> CheckCodeRedemption(string redeemCode)
        {
            return await SendPutRequest<CodeRedemptionRequest, CodeRedemptionResponse>(Endpointbuilder.RedeemCode, new CodeRedemptionRequest(){redeemCode = redeemCode});
        }

        public async Task<UpdateUserDataResponse> UpdateUserData(UpdateUserDataRequest updateUserDataRequest)
        {
            return await SendPutRequest<UpdateUserDataRequest, UpdateUserDataResponse>(Endpointbuilder.UpdateUserData, updateUserDataRequest);
        }

        public async Task SaveGameStats(UpdateGameStatRequest updateGameStatRequest)
        {
            await SendPutRequest<UpdateGameStatRequest, UpdateGameStatResponse>(Endpointbuilder.UpdateGameStats, updateGameStatRequest);
        }

        public async Task<SaveDataRequest> ResetSaveData()
        {
            return await SendPutRequest<SaveDataRequest, SaveDataRequest>(Endpointbuilder.ResetSaveData, new SaveDataRequest(){ savedData = PlayerData.Shared});
        }

        public async Task SaveGameData()
        {
            if (PlayerPrefs.GetInt("IsTrainer") == 1) return;
            
            if (PlayerPrefs.GetInt("IsGuest") == 1)
            {
                PlayerData.SaveData();
                return;
            }
            var response = await SendPutRequest<SaveDataRequest, SaveDataResponse>(Endpointbuilder.UpdateSaveData,new SaveDataRequest(){ savedData = PlayerData.Shared});
            _jwtToken = response.newToken;
        }


        public async Task LoginLegacy(LoginRequest loginRequest, LoginLegacyHandler loginSuccess, string endPoint)
        {
            var response = await SendPostRequest<LoginRequest, LoginResponse>(endPoint, loginRequest);
            _jwtToken = response.token;
            _accountId = response.accountId;
            PlayerData.LoadFromApi(response.savedData);
            loginSuccess(response);
        }

        //POST Requests
        public async Task<LoginResponse> LoginController(LoginRequest loginRequest, string endPoint)
        {
            var response = await SendPostRequest<LoginRequest, LoginResponse>(endPoint, loginRequest);
            _jwtToken = response.token;
            _accountId = response.accountId;
            return response;
        }

        public async Task<LoginResponse> RegisterController(RegisterRequest registerRequest, string endPoint)
        {
            var response = await SendPostRequest<RegisterRequest, LoginResponse>(endPoint, registerRequest);
            _jwtToken = response.token;
            _accountId = response.accountId;
            return response;
        }

    
        //GET Requests
        public async Task LogoutUser()
        {
            var accessToken = PlayerPrefs.GetString("AccessToken");
            PlayerPrefs.DeleteKey("AccessToken");
            await SendPostRequest<LogoutRequest, LogoutResponse>(Endpointbuilder.Logout, new LogoutRequest(accessToken));
        }

        public async Task<GetAchievementsResponse> GetPlayersAchievements()
        {
            return await SendGetRequest<GetAchievementsResponse>(Endpointbuilder.GetAchievements);
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
                    PlayerData.Shared = new();
                    PlayerData.Shared.username = username;
                    await AuthenticationService.Instance.UpdatePlayerNameAsync(username);
                    await SaveDataToUnity();
                    break;
                case LoginType.RegisterUnity:
                    await AuthenticationService.Instance.SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken);
                    PlayerData.Shared = new();
                    PlayerData.Shared.username = username;
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
        public async Task GetGameNews()
        {
            var gameNews = await SendGetRequest<GameNewsResponse>(Endpointbuilder.GameNews);
            SessionManager.Instance.GameNews = gameNews.newsList;
        }

        public async Task<ArenaResponse> GetT50Opponent()
        {
            return await SendGetRequest<ArenaResponse>(Endpointbuilder.ArenaT50);
        }
        
        public async Task<SimpleBoolResponse> HasSeenLatestNews()
        {
            return await SendPostRequest<ViewedNewsRequest, SimpleBoolResponse>(Endpointbuilder.NewsNotification, null);
        }
        
        public async Task<SimpleBoolResponse> UpdateSeenNews(ViewedNewsRequest request)
        {
            return await SendPostRequest<ViewedNewsRequest, SimpleBoolResponse>(Endpointbuilder.NewsNotification, request);
        }
        
        private async Task<TResponse> SendPostRequest<TRequest, TResponse>(string actionUrl, TRequest requestBody)
        {
            using var uwr = CreateApiRequest(_baseUrl + actionUrl, UnityWebRequest.kHttpVerbPOST, requestBody);
            await uwr.SendWebRequest();
            return JsonUtility.FromJson<TResponse>(uwr.downloadHandler.text);
        }
    
        private async Task<TResponse> SendGetRequest<TResponse>(string actionUrl)
        {
            using var uwr = CreateApiRequest(_baseUrl + actionUrl, UnityWebRequest.kHttpVerbGET, null);
            await uwr.SendWebRequest();
            return JsonUtility.FromJson<TResponse>(uwr.downloadHandler.text);
        }
    
        private async Task<TResponse> SendPutRequest<TRequest, TResponse>(string actionUrl, TRequest requestBody)
        {
            using var uwr = CreateApiRequest(_baseUrl + actionUrl, UnityWebRequest.kHttpVerbPUT, requestBody);
            await uwr.SendWebRequest();
            return JsonUtility.FromJson<TResponse>(uwr.downloadHandler.text);
        }
    
    public async Task SavePlayerScore()
    {
        var arguments = new Dictionary<string, object> { { "playerScore", PlayerData.Shared.playerScore } };
        await CloudCodeService.Instance.CallEndpointAsync("update-player-score", arguments);
    }
    
    public async Task<bool> LoadSomeData()
    {
        Dictionary<string, string> savedData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "SaveData" });
        if (savedData == null)
        {
            PlayerData.Shared = new();
            return false;
        }
        else
        {
            PlayerData.Shared = JsonUtility.FromJson<PlayerData>(savedData["SaveData"]);
            return true;
        }
    }

    public async Task SaveDataToUnity()
    {
        await SavePlayerScore();
        var data = new Dictionary<string, object> { { "SaveData", PlayerData.Shared } };
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    }
    
    }

    public class SaveDataResponse
    {
        public string newToken;
        public bool wasSuccess;
    }

    public class LogoutRequest
    {
        public LogoutRequest(string accessToken)
        {
            AccessToken = accessToken;
        }
        public string AccessToken;
    }

    public class LogoutResponse
    {
        public bool WasSuccess;
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