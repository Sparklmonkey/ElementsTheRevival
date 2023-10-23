using System.Collections.Generic;
using Networking;
using TMPro;
using UnityEngine;

public class LoginScreenSetupManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private TMP_InputField username, password;

    [SerializeField] private TextMeshProUGUI lastUpdateNote;
    [SerializeField]
    private TextMeshProUGUI errorMessage, versionLabel;
    private GameObject _touchBlocker;

    public List<TMP_InputField> fields;
    int _fieldIndexer;


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

    void Start()
    {
        lastUpdateNote.text = ApiManager.Instance.AppInfo.UpdateNote;
        fields = new List<TMP_InputField> { username, password };
        ApiManager.IsTrainer = false;
        SoundManager.Instance.PlayBGM("LoginScreen");
        username.text = PlayerPrefs.HasKey("SavedUser") ? PlayerPrefs.GetString("SavedUser") : "";
        versionLabel.text = $"Version {Application.version}";
    }

    public void PlayAsTrainer()
    {
        ApiManager.IsTrainer = true;
        PlayerData.Shared = new PlayerData();
        List<string> simpleList = CardDatabase.Instance.TrainerCardList;
        List<string> fullList = new List<string>(simpleList);
        fullList.AddRange(simpleList);
        fullList.AddRange(simpleList);
        fullList.AddRange(simpleList);
        fullList.AddRange(simpleList);
        fullList.AddRange(simpleList);
        fullList.Sort((x, y) => string.Compare(x, y));
        PlayerData.Shared.inventoryCards = new List<string>(fullList);

        PlayerData.Shared.currentDeck = StarterDecks.Instance.GetStarterDeck(Element.Darkness);
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
        var response = await ApiManager.Instance.LoginController(new LoginRequest()
        {
            username = username.text,
            password = password.text
        }, Endpointbuilder.userCredentialLogin);
        
        Destroy(_touchBlocker);
        ManageResponse(response);
    }
    
    
    private void ManageResponse(LoginResponse response)
    {
        if (response.errorMessage == ErrorCases.AllGood)
        {
            PlayerData.LoadFromApi(response.savedData);
            PlayerData.Shared.email = response.emailAddress;
            PlayerPrefs.SetString("AccessToken", response.accessToken);
            PlayerData.Shared = response.savedData;
            PlayerData.Shared.userName = username.text;
            
            SceneTransitionManager.Instance.LoadScene("Dashboard");
        }
        else
        {
            errorMessage.text = response.errorMessage.ToLongDescription();
        }
    }
}
