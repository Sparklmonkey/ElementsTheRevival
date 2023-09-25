using System.Collections.Generic;
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
    }

    void Start()
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
        }, "link-data");

        _touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(_touchBlocker);
        if (response.errorMessage == ErrorCases.AllGood)
        {
            PlayerData.Shared = response.savedData;
            PlayerData.Shared.userName = username.text;
            GetComponent<DashboardSceneManager>().LoadNewScene("DeckSelector");
        }
        else
        {
            serverResponse.text = response.errorMessage.ToLongDescription();
        }
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
            username = username.text,
            password = password.text,
            email = email.text
        }, "register");

        _touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(_touchBlocker);
        if (response.errorMessage == ErrorCases.AllGood)
        {
            PlayerData.Shared = response.savedData;
            PlayerData.Shared.userName = username.text;
            GetComponent<DashboardSceneManager>().LoadNewScene("DeckSelector");
        }
        else
        {
            serverResponse.text = response.errorMessage.ToLongDescription();
        }
    }
}