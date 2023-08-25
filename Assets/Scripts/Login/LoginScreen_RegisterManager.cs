using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginScreen_RegisterManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private TMP_InputField username, password, email;
    [SerializeField]
    private TextMeshProUGUI serverResponse;
    private GameObject touchBlocker;

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

    public async void AttemptToRegister()
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

        if(await ApiManager.shared.CheckUsername(username.text))
        {
            touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/RegisterPanel"));
            touchBlocker.transform.SetAsFirstSibling();
            await ApiManager.shared.UserLoginAsync(LoginType.RegisterUserPass, HandleUserRegistration, username.text, password.text);
        }
        else
        {
            serverResponse.text = "Username is already in use. Please try a different one.";
        }

    }

    public async void AttemptToRegisterWithUnity()
    {
        if (!username.text.UsernameCheck())
        {
            serverResponse.text = "Username does not meet requirements";
            return;
        }

        if (await ApiManager.shared.CheckUsername(username.text))
        {
            touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/RegisterPanel"));
            touchBlocker.transform.SetAsFirstSibling();
            await ApiManager.shared.UserLoginAsync(LoginType.RegisterUnity, HandleUserRegistration, username.text);
        }
        else
        {
            serverResponse.text = "Username is already in use. Please try a different one.";
        }
    }

    public async void HandleUserRegistration(string responseMessage)
    {
        touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(touchBlocker);
        if (responseMessage == "Success")
        {
            PlayerData.shared = new();
            PlayerData.shared.userName = username.text;
            await ApiManager.shared.SaveDataToUnity();
            GetComponent<DashboardSceneManager>().LoadNewScene("DeckSelector");
        }
        else
        {
            serverResponse.text = responseMessage;
        }
    }
}