using System.Collections.Generic;
using System.Linq;
using Networking;
using TMPro;
using UnityEngine;

namespace Login
{
    public class LoginScreenManager : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        private TMP_InputField username, password;

        [SerializeField] private TextMeshProUGUI lastUpdateNote;
        [SerializeField]
        private TextMeshProUGUI errorMessage, versionLabel;
        private GameObject _touchBlocker;

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
            lastUpdateNote.text = ApiManager.Instance.AppInfo.updateNote;
            fields = new List<TMP_InputField> { username, password };
            username.text = PlayerPrefs.HasKey("SavedUser") ? PlayerPrefs.GetString("SavedUser") : "";
            versionLabel.text = $"Version {Application.version}";
        }

        public void PlayAsTrainer()
        {
            PlayerPrefs.SetInt("IsTrainer", 1);
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
            PlayerData.Shared.markElement = Element.Darkness;
            PlayerData.Shared.currentQuestIndex = 8;
            PlayerData.Shared.electrum = 9999999;
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

            await ApiManager.Instance.UserLoginAsync(LoginType.UserPass, HandleUserLogin, username.text, password.text);
            
        }
    
    
        private void ManageResponse(LoginResponse response)
        {
            if (response.errorMessage == ErrorCases.AllGood)
            {
                PlayerData.LoadFromApi(response.savedData);
                PlayerData.Shared.email = response.emailAddress;
                PlayerPrefs.SetString("AccessToken", response.accessToken);
                PlayerData.Shared = response.savedData;
                PlayerData.Shared.username = username.text;
                
                SceneTransitionManager.Instance.LoadScene(PlayerData.Shared.GetDeck().Count == 0
                    ? "DeckSelector"
                    : "Dashboard");
            }
            else
            {
                errorMessage.text = response.errorMessage.ToLongDescription();
            }
        }
        
        public void HandleUserLogin(string responseMessage)
        {
            if (responseMessage == "Success")
            {
                _touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
                Destroy(_touchBlocker);
                if (PlayerData.Shared.currentDeck.DecompressDeckCode().Count < 30)
                {
                    GetComponent<DashboardSceneManager>().LoadNewScene("DeckSelector");
                }
                else
                {
                    GetComponent<DashboardSceneManager>().LoadNewScene("Dashboard");
                }
            }
            else
            {
                HandleUserLoginWithLegacyFallback();
            }
        }
        
        private async void HandleUserLoginWithLegacyFallback()
        {
            var response = await ApiManager.Instance.LoginController(new LoginRequest()
            {
                username = username.text,
                password = password.text
            }, Endpointbuilder.UserCredentialLogin);
        
            Destroy(_touchBlocker);
            ManageResponse(response);
        }
    }
}
