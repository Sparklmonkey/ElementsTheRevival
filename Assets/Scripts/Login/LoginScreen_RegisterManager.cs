using System.Collections.Generic;
using Networking;
using TMPro;
using UnityEngine;

public class LoginScreenRegisterManager : MonoBehaviour
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
        var response = await ApiManager.Instance.RegisterController(new()
        {
            username = username.text,
            password = password.text,
            email = email.text,
            dataToLink = PlayerData.Shared
        }, Endpointbuilder.RegisterLinkData);

        _touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(_touchBlocker);
        ManageResponse(response);
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
        var response = await ApiManager.Instance.RegisterController(new()
        {
            dataToLink = new(),
            username = username.text,
            password = password.text,
            email = email.text
        }, Endpointbuilder.RegisterNewUser);

        _touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(_touchBlocker);
        ManageResponse(response);
    }

    private void ManageResponse(LoginResponse response)
    {
        if (response.errorMessage == ErrorCases.AllGood)
        {
            PlayerData.LoadFromApi(response.savedData);
            PlayerData.Shared.email = response.emailAddress;
            PlayerData.Shared.username = username.text;
            PlayerPrefs.SetString("AccessToken", response.accessToken);
            PlayerData.Shared = response.savedData;
            PlayerData.Shared.username = username.text;
            
            SceneTransitionManager.Instance.LoadScene(PlayerData.Shared.currentDeck.Count == 0 ? "DeckSelector" : "Dashboard");
        }
        else
        {
            serverResponse.text = response.errorMessage.ToLongDescription();
        }
    }
}