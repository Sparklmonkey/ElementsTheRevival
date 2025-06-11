using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using Login.Model;
using Networking.Networking;

namespace Login.ViewModel
{
    public class RegisterViewModel
    {
        private readonly RegisterModel _model;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ICloudSaveManager _cloudSaveManager;

        public event Action<string> OnErrorMessageChanged;
        public event Action<bool> OnProcessingChanged;
        public event Action<string> OnSceneTransition;
        
        public RegisterViewModel(IAuthenticationManager authenticationManager, ICloudSaveManager cloudSaveManager)
        {
            _model = new RegisterModel();
            _authenticationManager = authenticationManager;
            _cloudSaveManager = cloudSaveManager;
        }

        public async Task AttemptRegister(string username, string password)
        {
            try
            {
                SetProcessing(true);
                _model.Username = username;
                _model.Password = password;

                var response = await _authenticationManager.RegisterUser(username, password);
                await _authenticationManager.UpdateUsername(username);
                var newPlayer = new PlayerData(username);
                await _cloudSaveManager.SavePlayerData(newPlayer);
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
                OnSceneTransition?.Invoke("DeckSelector");
            }
            else
            {
                SetErrorMessage(responseMessage);
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
}