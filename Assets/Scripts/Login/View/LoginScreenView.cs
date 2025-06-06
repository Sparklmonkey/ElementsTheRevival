using System.Collections.Generic;
using Networking;
using Networking.Networking;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.RemoteConfig;
using UnityEngine;

public class LoginScreenView : MonoBehaviour
{
    [SerializeField] private TMP_InputField username, password;
    [SerializeField] private TextMeshProUGUI lastUpdateNote;
    [SerializeField] private TextMeshProUGUI errorMessage, versionLabel;
    
    public List<TMP_InputField> fields;
    private int _fieldIndexer;
    
    private LoginViewModel _viewModel;
    private GameObject _touchBlocker;
    private GameObject _popUpObject;

    private void Start()
    {
        _viewModel = new LoginViewModel(new AuthenticationManager(AuthenticationService.Instance), new CloudSaveManager());
        fields = new List<TMP_InputField> { username, password };
        _fieldIndexer = 0;
        SetupViewModelEvents();
        InitializeUI();
    }

    private void SetupViewModelEvents()
    {
        _viewModel.OnErrorMessageChanged += UpdateErrorMessage;
        _viewModel.OnProcessingChanged += UpdateProcessingState;
        _viewModel.OnSceneTransition += SceneTransitionManager.Instance.LoadScene;
    }

    private void InitializeUI()
    {
        lastUpdateNote.text = RemoteConfigService.Instance.appConfig.GetString("VersionNote");
        username.text = PlayerPrefs.HasKey("SavedUser") ? PlayerPrefs.GetString("SavedUser") : "";
        versionLabel.text = $"Version {Application.version}";
    }

    private void Update()
    {
        HandleTabInput();
        HandleReturnInput();
    }

    private void HandleTabInput()
    {
        if (!Input.GetKeyDown(KeyCode.Tab)) return;
        
        if (fields.Count <= _fieldIndexer)
        {
            _fieldIndexer = 0;
        }
        fields[_fieldIndexer].Select();
        _fieldIndexer++;
    }

    private void HandleReturnInput()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AttemptToLoginUsernamePassword();
        }
    }

    public async void AttemptToLoginUsernamePassword()
    {
        await _viewModel.AttemptLogin(username.text, password.text);
    }

    private void UpdateErrorMessage(string message)
    {
        errorMessage.text = message;
    }

    private void UpdateProcessingState(bool isProcessing)
    {
        if (isProcessing)
        {
            ShowTouchBlocker();
        }
        else
        {
            HideTouchBlocker();
        }
    }

    private void ShowTouchBlocker()
    {
        _touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), 
            transform.Find("Background/MainPanel"));
        _touchBlocker.transform.SetAsFirstSibling();
    }

    private void HideTouchBlocker()
    {
        if (!_touchBlocker) return;
        _touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(_touchBlocker);
    }

    public void PlayAsTrainer() => _viewModel.PlayAsTrainer();
    public void PlayAsGuest() => _viewModel.PlayAsGuest();

    
}