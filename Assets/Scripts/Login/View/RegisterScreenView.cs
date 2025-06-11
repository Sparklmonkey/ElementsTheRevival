using System.Collections.Generic;
using Login.ViewModel;
using Networking.Networking;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;

namespace Login.View
{
    public class RegisterScreenView : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        private TMP_InputField username, password;
        [SerializeField]
        private TextMeshProUGUI serverResponse;

        [SerializeField] private GameObject linkDataPopUp;
        private GameObject _touchBlocker;
        public List<TMP_InputField> fields;
        private int _fieldIndexer;
        
    private RegisterViewModel _viewModel;
    private GameObject _popUpObject;

    private void Start()
    {
        _viewModel = new RegisterViewModel(new AuthenticationManager(AuthenticationService.Instance), new CloudSaveManager(new CloudCodeManager()));
        fields = new List<TMP_InputField> { username, password };
        _fieldIndexer = 0;
        SetupViewModelEvents();
    }

    private void SetupViewModelEvents()
    {
        _viewModel.OnErrorMessageChanged += UpdateErrorMessage;
        _viewModel.OnProcessingChanged += UpdateProcessingState;
        _viewModel.OnSceneTransition += SceneTransitionManager.Instance.LoadScene;
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
        await _viewModel.AttemptRegister(username.text, password.text);
    }

    private void UpdateErrorMessage(string message)
    {
        serverResponse.text = message;
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
        
    }
}