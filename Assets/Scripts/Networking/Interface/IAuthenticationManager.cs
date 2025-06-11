using System.Threading.Tasks;
using Core;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Networking.Networking
{
    public enum LoginResponse
    {
        Success,
        InvalidCredentials,
        Error
    }
    public interface IAuthenticationManager
    {
        public Task UpdateUserPassword(string oldPassword, string newPassword);
        public Task UpdateUsername(string newUsername);
        public Task<string> Login(string username, string password);
        public Task<string> RegisterUser(string username, string password);
        public void Logout();
    }

    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly IAuthenticationService _authenticationService;
        
        public AuthenticationManager(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task UpdateUserPassword(string oldPassword, string newPassword)
        {
            await _authenticationService.UpdatePasswordAsync(oldPassword, newPassword);
        }

        public async Task UpdateUsername(string newUsername)
        {
            await _authenticationService.UpdatePlayerNameAsync(newUsername);
        }

        public void Logout()
        {
            _authenticationService.SignOut(true);
        }

        public async Task<string> RegisterUser(string username, string password)
        {
            _authenticationService.SignOut();
            try
            {
                await _authenticationService.SignInWithUsernamePasswordAsync(username, password);
                return "Success";
            }
            catch (AuthenticationException ex)
            {
                var unityError = JsonUtility.FromJson<UnityResponse>(ex.GetBaseException().Message);
                return unityError.title;
            }
            catch (RequestFailedException ex)
            {
                var unityError = JsonUtility.FromJson<UnityResponse>(ex.GetBaseException().Message);
                return unityError.title;
            }
        }
        
        public async Task<string> Login(string username, string password)
        {
            _authenticationService.SignOut();
            try
            {
                await _authenticationService.SignInWithUsernamePasswordAsync(username, password);
                return "Success";
            }
            catch (AuthenticationException ex)
            {
                var unityError = JsonUtility.FromJson<UnityResponse>(ex.GetBaseException().Message);
                return unityError.title;
            }
            catch (RequestFailedException ex)
            {
                var unityError = JsonUtility.FromJson<UnityResponse>(ex.GetBaseException().Message);
                return unityError.title;
            }
        }
    }
}