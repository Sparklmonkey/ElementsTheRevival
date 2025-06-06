using System;
using System.Threading.Tasks;
using Networking.Networking;
using Unity.Services.Authentication;
using Unity.Services.RemoteConfig;
using UnityEngine;

public class LoginViewModel
{
    private readonly LoginModel _model;
    private readonly IAuthenticationManager _authenticationManager;
    private readonly ICloudSaveManager _cloudSaveManager;

    public event Action<string> OnErrorMessageChanged;
    public event Action<bool> OnProcessingChanged;
    public event Action<string> OnSceneTransition;

    public LoginViewModel(IAuthenticationManager authenticationManager, ICloudSaveManager cloudSaveManager)
    {
        _model = new LoginModel();
        _authenticationManager = authenticationManager;
        _cloudSaveManager = cloudSaveManager;
        LoadInitialData();
    }

    private void LoadInitialData()
    {
        _model.VersionNote = RemoteConfigService.Instance.appConfig.GetString("VersionNote");
        _model.Version = $"Version {Application.version}";
        _model.Username = PlayerPrefs.HasKey("SavedUser") ? PlayerPrefs.GetString("SavedUser") : "";
    }

    public async Task AttemptLogin(string username, string password)
    {
        try
        {
            SetProcessing(true);
            _model.Username = username;
            _model.Password = password;

            var response = await _authenticationManager.Login(username, password);
            HandleUserLogin(response);
        }
        catch (Exception ex)
        {
            SetErrorMessage(ex.Message);
        }
        finally
        {
            SetProcessing(false);
        }
    }

    private async void HandleUserLogin(string responseMessage)
    {
        if (responseMessage == "Success")
        {
            PlayerData.Shared = await _cloudSaveManager.LoadPlayerData();
            var cardList = PlayerData.Shared.CurrentDeck.ConvertCardCodeToList();
            OnSceneTransition?.Invoke(cardList.Count < 30 ? "DeckSelector" : "Dashboard");
        }
        else
        {
            SetErrorMessage(responseMessage);
        }
    }

    public void PlayAsTrainer()
    {
        PlayerPrefs.SetInt("IsTrainer", 1);
        AuthenticationService.Instance.SignOut(true);
        PlayerData.Shared = new PlayerData();
        // ... rest of the trainer setup logic
        OnSceneTransition?.Invoke("Dashboard");
    }

    public void PlayAsGuest()
    {
        PlayerPrefs.SetInt("IsGuest", 1);
        if (PlayerPrefs.HasKey("SaveData"))
        {
            PlayerData.LoadData();
            OnSceneTransition?.Invoke("Dashboard");
        }
        else
        {
            PlayerData.Shared = new PlayerData();
            OnSceneTransition?.Invoke("DeckSelector");
        }
    }

    private void SetErrorMessage(string message)
    {
        _model.ErrorMessage = message;
        OnErrorMessageChanged?.Invoke(message);
    }

    private void SetProcessing(bool isProcessing)
    {
        _model.IsProcessing = isProcessing;
        OnProcessingChanged?.Invoke(isProcessing);
    }
}