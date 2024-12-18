using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Networking.Response;
using UnityEngine;
using UnityEngine.Networking;

namespace Networking
{
    public class ApiManager : SingletonMono<ApiManager>
    {
        public static bool IsTrainer => PlayerPrefs.GetInt("IsTrainer") == 1;
        private string _jwtToken;
        private string _accountId;
        // private readonly string _baseUrl =  "https://www.elementstherevival.com/api/";
#if UNITY_EDITOR
        private readonly string _baseUrl =  "http://localhost:5158/api/";
#else
        private readonly string _baseUrl =  "https://www.elementstherevival.com/api/";
#endif
        
        
        private readonly string _apiKey = "ElementRevival-ApiKey";
        public AppInfo AppInfo;

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
            if (PlayerPrefs.GetInt("IsGuest") == 1)
            {
                PlayerData.SaveData();
                return;
            }
            var response = await SendPutRequest<SaveDataRequest, SaveDataResponse>(Endpointbuilder.UpdateSaveData,new SaveDataRequest(){ savedData = PlayerData.Shared});
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

        public async Task<AppInfo> GetAppInfo()
        {
            AppInfo = await SendGetRequest<AppInfo>(Endpointbuilder.AppInfo);
            return AppInfo;
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
    }

    public class AppInfo
    {
        public bool isMaintenance;
        public bool shouldUpdate;
        public string updateNote;
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
