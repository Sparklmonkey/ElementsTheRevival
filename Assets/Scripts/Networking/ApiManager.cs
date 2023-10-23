using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Networking
{
    public class ApiManager : SingletonMono<ApiManager>
    {
        public static bool IsTrainer;
        private string _jwtToken;

        private readonly string _baseUrl = "https://www.elementstherevival.com/api/";//"http://localhost:5158/";//"https://www.sparklmonkeygames.com/";
        private readonly string _apiKey = "ElementRevival-ApiKey";

        public AppInfo AppInfo;

        private async void Start()
        {
            DontDestroyOnLoad(gameObject);
            _lastToken = DateTime.UtcNow;
        }
        private DateTime _lastToken;
        private async void Update()
        {
            if (!(DateTime.UtcNow.Subtract(_lastToken).TotalMinutes > 10)) return;
            await GetRefreshJwtToken();
            _lastToken = DateTime.UtcNow;
        }
    
        private UnityWebRequest CreateApiRequest(string url, string method, object body)
        {
            string bodyString = (body is string s) ? s : JsonUtility.ToJson(body);
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
            request.SetRequestHeader("Authorization", $"Bearer {_jwtToken}");
            request.timeout = 60;
            return request;
        }
    

        //PUT Requests

        public async Task<CodeRedemptionResponse> CheckCodeRedemption(string redeemCode)
        {
            return await SendPutRequest<CodeRedemptionRequest, CodeRedemptionResponse>(Endpointbuilder.redeemCode, new CodeRedemptionRequest(){redeemCode = redeemCode});
        }

        public async Task<UpdateUserDataResponse> UpdateUserData(UpdateUserDataRequest updateUserDataRequest)
        {
            return await SendPutRequest<UpdateUserDataRequest, UpdateUserDataResponse>(Endpointbuilder.updateUserData, updateUserDataRequest);
        }

        public async Task SaveGameStats(UpdateGameStatRequest updateGameStatRequest)
        {
            await SendPutRequest<UpdateGameStatRequest, UpdateGameStatResponse>(Endpointbuilder.updateGameStats, updateGameStatRequest);
        }

        public async Task<SaveDataRequest> ResetSaveData()
        {
            return await SendPutRequest<SaveDataRequest, SaveDataRequest>(Endpointbuilder.resetSaveData, new SaveDataRequest(){ savedData = PlayerData.Shared});
        }

        public async Task SaveGameData()
        {
            PlayerData.Shared.ClearIllegalCards();
            await SendPutRequest<SaveDataRequest, ErrorCases>(Endpointbuilder.updateSaveData,new SaveDataRequest(){ savedData = PlayerData.Shared});
        }
    
    
        //POST Requests
        public async Task<LoginResponse> LoginController(LoginRequest loginRequest, string endPoint)
        {
            var response = await SendPostRequest<LoginRequest, LoginResponse>(endPoint, loginRequest);
            _jwtToken = response.token;
            return response;
        }

        public async Task<LoginResponse> RegisterController(RegisterRequest registerRequest, string endPoint)
        {
            var response = await SendPostRequest<RegisterRequest, LoginResponse>(endPoint, registerRequest);
            _jwtToken = response.token;
            return response;
        }

    
        //GET Requests
        public async Task LogoutUser()
        {
            PlayerPrefs.DeleteKey("AccessToken");
            await SendGetRequest<bool>(Endpointbuilder.logout + $"/{_jwtToken}");
        }
        public async Task GetRefreshJwtToken()
        {
            _jwtToken = await SendGetRequest<string>($"{Endpointbuilder.refreshToken}/{_jwtToken}");
        }

        public async Task<AppInfo> GetAppInfo()
        {
            AppInfo = await SendGetRequest<AppInfo>(Endpointbuilder.appInfo);
            return AppInfo;
        }
    
        public async Task<ArenaResponse> GetT50Opponent()
        {
            return await SendGetRequest<ArenaResponse>(Endpointbuilder.arenaT50);
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
        public bool IsMaintenance;
        public bool ShouldUpdate;
        public string UpdateNote;
    }
}