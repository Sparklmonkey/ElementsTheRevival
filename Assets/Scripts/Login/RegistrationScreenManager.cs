using System.Collections.Generic;
using Networking;
using TMPro;
using UnityEngine;

namespace Login
{
    public class RegistrationScreenManager : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        private TMP_InputField username, password, email;
        [SerializeField]
        private TextMeshProUGUI serverResponse;

        [SerializeField] private GameObject linkDataPopUp;
        private GameObject _touchBlocker;

        public List<TMP_InputField> fields;
        private int _fieldIndexer;

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Tab)) return;
            if (fields.Count <= _fieldIndexer)
            {
                _fieldIndexer = 0;
            }
            fields[_fieldIndexer].Select();
            _fieldIndexer++;
        }

        private void Start()
        {
            fields = new List<TMP_InputField> { username, password };
        }

        public async void RegisterLinkData()
        {
            _touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/RegisterPanel"));
            _touchBlocker.transform.SetAsFirstSibling();
        
            PlayerData.LoadData();
            await ApiManager.Instance.UserLoginAsync(LoginType.LinkUserPass, HandleUserLogin, username.text, password.text);
        }

        public void AttemptToRegister()
        {
            if (!username.text.UsernameCheck())
            {
                serverResponse.text = "Username does not meet requirements";
                return;
            }
            if (!password.text.PasswordCheck())
            {
                serverResponse.text = "Password does not meet requirements";
                return;
            }

            if (PlayerPrefs.HasKey("SaveData"))
            {
                linkDataPopUp.SetActive(true);
                return;
            }
            RegisterNewUser();
        }

        public async void RegisterNewUser()
        {
            _touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/RegisterPanel"));
            _touchBlocker.transform.SetAsFirstSibling();

            await ApiManager.Instance.UserLoginAsync(LoginType.RegisterUnity, HandleUserLogin, username.text, password.text);
        }
        
        public async void HandleUserLogin(string responseMessage)
        {
            if (responseMessage == "Success")
            {
                if (email.text.IsValidEmail())
                {
                    await ApiManager.Instance.UpdateUserEmail(email.text);
                }
                _touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
                Destroy(_touchBlocker);
                PlayerData.Shared.Email = email.text;
                var cardList = PlayerData.Shared.CurrentDeck.ConvertCardCodeToList();
                PlayerData.Shared.Username = username.text;
                SceneTransitionManager.Instance.LoadScene(
                    cardList.Count < 30 ? "DeckSelector" : "Dashboard");
            }
            else
            {
                serverResponse.text = responseMessage;
            }
        }
    }
}