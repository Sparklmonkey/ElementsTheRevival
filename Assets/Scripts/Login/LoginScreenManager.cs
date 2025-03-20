using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.FixedStrings;
using Networking;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace Login
{
    public class LoginScreenManager : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        private TMP_InputField username, password;

        [SerializeField] private GameObject popUpModal, updatePasswordModal;
        [SerializeField] private TextMeshProUGUI lastUpdateNote;
        [SerializeField]
        private TextMeshProUGUI errorMessage, versionLabel;
        private GameObject _touchBlocker, _popUpObject;

        public List<TMP_InputField> fields;
        private int _fieldIndexer;


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (fields.Count <= _fieldIndexer)
                {
                    _fieldIndexer = 0;
                }
                fields[_fieldIndexer].Select();
                _fieldIndexer++;
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                AttemptToLoginUsernamePassword();
            }
        }

        private void Start()
        {
            var updateNote = RemoteConfigService.Instance.appConfig.GetString("VersionNote");
            lastUpdateNote.text = updateNote;
            fields = new List<TMP_InputField> { username, password };
            username.text = PlayerPrefs.HasKey("SavedUser") ? PlayerPrefs.GetString("SavedUser") : "";
            versionLabel.text = $"Version {Application.version}";
        }

        public void PlayAsTrainer()
        {
            PlayerPrefs.SetInt("IsTrainer", 1);
            AuthenticationService.Instance.SignOut(true);
            PlayerData.Shared = new PlayerData();
            var simpleList = CardDatabase.Instance.TrainerCardList;
            var fullList = new List<Card>(simpleList);
            fullList.AddRange(simpleList);
            fullList.AddRange(simpleList);
            fullList.AddRange(simpleList);
            fullList.AddRange(simpleList);
            fullList.AddRange(simpleList);
            fullList.Sort((x, y) => string.Compare(x.Id, y.Id));
            PlayerData.Shared.SetInventory(fullList.SerializeCard());

            PlayerData.Shared.SetDeck(CardDatabase.Instance.StarterDecks.First(x => x.MarkElement.Equals(Element.Darkness)).DeckList.SerializeCard());
            PlayerData.Shared.MarkElement = Element.Darkness;
            PlayerData.Shared.CurrentQuestIndex = 8;
            PlayerData.Shared.Electrum = 9999999;
            SceneTransitionManager.Instance.LoadScene("Dashboard");
        }

        public void PlayAsGuest()
        {
            PlayerPrefs.SetInt("IsGuest", 1);
            if (PlayerPrefs.HasKey("SaveData"))
            {
                PlayerData.LoadData();
                SceneTransitionManager.Instance.LoadScene("Dashboard");
            }
            else
            {
                PlayerData.Shared = new PlayerData();
                SceneTransitionManager.Instance.LoadScene("DeckSelector");
            }
        }

        public async void AttemptToLoginUsernamePassword()
        {
            _touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));
            _touchBlocker.transform.SetAsFirstSibling();
            Debug.Log("Attempting To Login");
            await ApiManager.Instance.UserLoginAsync(LoginType.UserPass, HandleUserLogin, username.text, password.text);
            
        }
    
    
        private void ManageResponse(LoginResponse response)
        {
            if (response.ErrorMessage == ErrorCases.AllGood)
            {
                PlayerData.LoadFromApi(response.savedData);
                PlayerData.Shared.Email = response.emailAddress;
                PlayerPrefs.SetString("AccessToken", response.accessToken);
                PlayerData.Shared.Username = username.text;
                
                SceneTransitionManager.Instance.LoadScene(PlayerData.Shared.GetDeck().Count == 0
                    ? "DeckSelector"
                    : "Dashboard");
            }
            else
            {
                errorMessage.text = response.ErrorMessage.ToLongDescription();
            }
        }

        private async void HandleUserLogin(string responseMessage)
        {
            if (responseMessage == "Success")
            {
                _touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
                Destroy(_touchBlocker);
                var cardList = PlayerData.Shared.CurrentDeck.ConvertCardCodeToList();
                SceneTransitionManager.Instance.LoadScene(
                    cardList.Count < 30 ? "DeckSelector" : "Dashboard");
            }
            else
            {
                var legacUser = await GetLegacyUser(username.text, password.text);
                if (legacUser.ErrorMessage == ErrorCases.AllGood)
                {
                    PlayerData.LoadFromApi(legacUser.savedData);
                    PlayerData.Shared.Email = legacUser.emailAddress;
                    PlayerPrefs.SetString("AccessToken", legacUser.accessToken);
                    PlayerData.Shared.Username = username.text;
                    switch (responseMessage)
                    {
                        case "WRONG_USERNAME_PASSWORD":
                            ShowLinkPopUp();
                            break; 
                        case "INVALID_PASSWORD":
                            ShowChangePasswordPopup();
                            break;
                        case "ENTITY_EXISTS":
                            ShowChangeUsernamePopup();
                            break;
                        default:
                            Debug.Log(responseMessage);
                            break;
                    }
                }
                else
                {
                    errorMessage.text = legacUser.ErrorMessage.ToLongDescription();
                }
                
            }
        }

        private void ShowChangePasswordPopup()
        {
            _touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
            Destroy(_touchBlocker);
            _popUpObject = Instantiate(updatePasswordModal, transform);
            _popUpObject.GetComponent<TextInputPopupModal>().SetupTextInputModal("LoginScreen", "LoginChangePasswordUnityTitle", 
                "LoginChangePasswordUnityButtonTitle", LinkUnityWithNewPassword);
        }

        private void ShowChangeUsernamePopup()
        {
            _touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
            Destroy(_touchBlocker);
            _popUpObject = Instantiate(updatePasswordModal, transform);
            _popUpObject.GetComponent<TextInputPopupModal>().SetupTextInputModal("LoginScreen", "LoginChangeUsernameUnityTitle", 
                "LoginChangeUsernameUnityButtonTitle", LinkUnityWithNewPassword);
        }
        
        private void ShowLinkPopUp()
        {
            _touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
            Destroy(_touchBlocker);
            _popUpObject = Instantiate(popUpModal, transform);
            _popUpObject.GetComponent<PopUpModal>().SetupModal("LoginScreen", "LoginMigrateToUnityTitle", 
                "LoginMigrateToUnityButtonOneTitle",
                LinkDataWithUnity);
        }
        
        
        private async void LinkUnityWithNewPassword(string newPassword)
        {
            Destroy(_popUpObject);
            _touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));
            _touchBlocker.transform.SetAsFirstSibling();
            password.text = newPassword;
            await ApiManager.Instance.UserLoginAsync(LoginType.LinkUserPass, HandleUserLogin, username.text,
                newPassword);
        }
        
        private async void LinkUnityWithNewUsername(string newUsername)
        {
            Debug.Log("Attempt to Link With Unity");
            _touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));
            _touchBlocker.transform.SetAsFirstSibling();
            username.text = newUsername;
            await ApiManager.Instance.UserLoginAsync(LoginType.LinkUserPass, HandleUserLogin, newUsername,
                password.text);
        }
        
        private async void LinkDataWithUnity()
        {
            Destroy(_popUpObject);
            _touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));
            _touchBlocker.transform.SetAsFirstSibling();
            await ApiManager.Instance.UserLoginAsync(LoginType.LinkUserPass, HandleUserLogin, username.text,
                password.text);
        }
        private async void HandleUserLoginWithLegacyFallback()
        {
            Destroy(_popUpObject);
            var response = await ApiManager.Instance.LoginController(new LoginRequest()
            {
                username = username.text,
                password = password.text
            }, Endpointbuilder.UserCredentialLogin);
        
            Destroy(_touchBlocker);
            ManageResponse(response);
        }

        private async Task<LoginResponse> GetLegacyUser(string username, string password)
        {
            var response = await ApiManager.Instance.LoginController(new LoginRequest()
            {
                username = username,
                password = password
            }, Endpointbuilder.UserCredentialLogin);
            return response;
        }
    }
}
