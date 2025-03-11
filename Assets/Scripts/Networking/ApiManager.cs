using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Networking.Response;
using Networking;
using Unity.Services.Authentication;
using Unity.Services.CloudCode.GeneratedBindings;
using Unity.Services.CloudCode;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
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
        private readonly string _baseUrl = "https://www.elementstherevival.com/api/";
        // private readonly string _baseUrl =  "http://localhost:5158/api/";
#else
        private readonly string _baseUrl = "https://www.elementstherevival.com/api/";
#endif

        private readonly string _apiKey = "ElementRevival-ApiKey";

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }


        public async Task CallModuleTest()
        {
            try
            {
                SessionManager.Instance.SetPlayerAchievementModule(
                    new PlayerAchievementsBindings(CloudCodeService.Instance));
            }
            catch (CloudCodeException exception)
            {
                Debug.LogException(exception);
            }
        }
        public async Task UpdateUserEmail(string email)
        {
            if (!email.IsValidEmail()) return;
            var data = new Dictionary<string, object> { { "Email", email } };
            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
        }
        private UnityWebRequest CreateApiRequest(string url, string method, object body)
        {
            var bodyString = body is string s ? s : JsonUtility.ToJson(body);
            var request = new UnityWebRequest
            {
                url = url,
                method = method,
                downloadHandler = new DownloadHandlerBuffer(),
                uploadHandler =
                    new UploadHandlerRaw(string.IsNullOrEmpty(bodyString) ? null : Encoding.UTF8.GetBytes(bodyString))
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

        //PUT Requests

        public async Task<CodeRedemptionResponse> CheckCodeRedemption(string redeemCode)
        {
            return await SendPutRequest<CodeRedemptionRequest, CodeRedemptionResponse>(Endpointbuilder.RedeemCode,
                new CodeRedemptionRequest() { redeemCode = redeemCode });
        }
        public async Task<CodeRedemptionResponse> GetCodeDetails(string redeemCode)
        {
            var arguments = new Dictionary<string, object> { { "CodeName", redeemCode } };
            var codeDetails = await CloudCodeService.Instance.CallEndpointAsync("validate-redeem-code", arguments);
            if (codeDetails is "Already Redeemed" or "Invalid Code")
            {
                return new CodeRedemptionResponse
                {
                    CodeName = codeDetails
                };
            }
            return JsonUtility.FromJson<CodeRedemptionResponse>(codeDetails);
        }
        public async Task RedeemCode(string redeemCode)
        {
            var arguments = new Dictionary<string, object> { { "CodeName", redeemCode } };
            await CloudCodeService.Instance.CallEndpointAsync("redeem-code", arguments);
        }

        public async Task<bool> CheckOraclePlay()
        {
            var result = await CloudCodeService.Instance.CallEndpointAsync("validate-oracle-usable");
            var canPlayObject = JsonUtility.FromJson<CanOracle>(result);
            return canPlayObject.CanOpenOracle;
        }

        public async Task<ScoreUpdateResponse> UpdateScore(int score)
        {
            var arguments = new Dictionary<string, object> { { "Score", score } };
            var result = await CloudCodeService.Instance.CallEndpointAsync("update-player-score", arguments);
            var canPlayObject = JsonUtility.FromJson<ScoreUpdateResponse>(result);
            return canPlayObject;
        }

        public async Task UpdateOraclePlayed()
        {
            await CloudCodeService.Instance.CallEndpointAsync("update-oracle-date");
        }

        public async Task<UpdateUserDataResponse> UpdateUserData(UpdateUserDataRequest updateUserDataRequest)
        {
            return await SendPutRequest<UpdateUserDataRequest, UpdateUserDataResponse>(Endpointbuilder.UpdateUserData,
                updateUserDataRequest);
        }

        public async Task UpdateUserPassword(string oldPassword, string newPassword)
        {
            await AuthenticationService.Instance.UpdatePasswordAsync(oldPassword, newPassword);
        }
        
        public async Task UpdateUsername(string newUsername)
        {
            await AuthenticationService.Instance.UpdatePlayerNameAsync(newUsername);
        }
        public async Task SaveGameStats(UpdateGameStatRequest updateGameStatRequest)
        {
            await SendPutRequest<UpdateGameStatRequest, UpdateGameStatResponse>(Endpointbuilder.UpdateGameStats,
                updateGameStatRequest);
        }

        public async Task<SaveDataRequest> ResetSaveData()
        {
            if (isUnityUser)
            {
                await SaveDataToUnity();
                return new SaveDataRequest
                {
                    savedData = PlayerData.Shared
                };
            }
            return await SendPutRequest<SaveDataRequest, SaveDataRequest>(Endpointbuilder.ResetSaveData,
                new SaveDataRequest() { savedData = PlayerData.Shared });
        }

        public async Task SaveGameData()
        {
            if (PlayerPrefs.GetInt("IsTrainer") == 1) return;

            if (PlayerPrefs.GetInt("IsGuest") == 1)
            {
                PlayerData.SaveData();
                return;
            }
            
            if (isUnityUser)
            {
                await SaveDataToUnity();
                return;
            }
            var response = await SendPutRequest<SaveDataRequest, SaveDataResponse>(Endpointbuilder.UpdateSaveData,
                new SaveDataRequest() { savedData = PlayerData.Shared });
            _jwtToken = response.newToken;
        }
        
        //POST Requests
        public async Task<LoginResponse> LoginController(LoginRequest loginRequest, string endPoint)
        {
            var response = await SendPostRequest<LoginRequest, LoginResponse>(endPoint, loginRequest);
            _jwtToken = response.token;
            _accountId = response.accountId;
            return response;
        }

        //GET Requests
        public async Task LogoutUser()
        {
            AuthenticationService.Instance.SignOut(true);
        }

        public async Task<GetAchievementsResponse> GetPlayersAchievements()
        {
            return await SendGetRequest<GetAchievementsResponse>(Endpointbuilder.GetAchievements);
        }

        public async Task UserLoginAsync(LoginType loginType, LoginUserHandler handler, string username = "",
            string password = "")
        {
            AuthenticationService.Instance.SignOut();
            try
            {
                switch (loginType)
                {
                    case LoginType.Unity:
                        isUnityUser = true;
                        break;
                    case LoginType.UserPass:
                        await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
                        await LoadSomeData();
                        isUnityUser = true;
                        break;
                    case LoginType.RegisterUserPass:
                        await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
                        PlayerData.Shared = new();
                        PlayerData.Shared.Username = username;
                        await AuthenticationService.Instance.UpdatePlayerNameAsync(username);
                        await SaveDataToUnity();
                        isUnityUser = true;
                        break;
                    case LoginType.RegisterUnity:
                        break;
                    case LoginType.LinkUserPass:
                        Debug.Log("Attempt to Link Account");
                        await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
                        await AuthenticationService.Instance.UpdatePlayerNameAsync(username);
                        await SaveDataToUnity();
                        isUnityUser = true;
                        break;
                }

                await CallModuleTest();
                handler("Success");
            }
            catch (AuthenticationException ex)
            {
                var unityError = JsonUtility.FromJson<UnityResponse>(ex.GetBaseException().Message);
                handler(unityError.title);
            }
            catch (RequestFailedException ex)
            {
                var unityError = JsonUtility.FromJson<UnityResponse>(ex.GetBaseException().Message);
                handler(unityError.title);
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
            return await SendPostRequest<ViewedNewsRequest, SimpleBoolResponse>(Endpointbuilder.NewsNotification,
                request);
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

        public async Task<bool> LoadSomeData()
        {
            Dictionary<string, Item> savedData =
                await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "SAVE_DATA" });
            if (savedData.Count == 0)
            {
                PlayerData.Shared = new();
                return false;
            }

            var data = savedData["SAVE_DATA"].Value;
            Debug.Log(data);
            PlayerData.Shared = data.GetAs<PlayerData>();
            var newScore = await UpdateScore(0);
            SessionManager.Instance.PlayerScore = newScore;
            return true;
        }

        public async Task SaveDataToUnity()
        {
            var data = new Dictionary<string, object> { { "SAVE_DATA", PlayerData.Shared } };
            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
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

    public class CanOracle
    {
        public bool CanOpenOracle;
    }
    public class ScoreUpdateResponse
    {
        public int overallScore;
        public int seasonalScore;
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

[Serializable]
public class UnityResponse
{
    public string detail;
    public List<string> details;
    public string title;
    public int status;
}