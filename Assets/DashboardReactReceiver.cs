using System.Threading.Tasks;
using Networking;
using Settings;
using TMPro;
using UnityEngine;

public class DashboardReactReceiver : MonoBehaviour
{
    [SerializeField]
    private GameObject canvas, settingsPanelPrefab;

    [SerializeField] private TextMeshProUGUI saveStatus;

    [SerializeField] private DashResetAccountAnimator _resetAccountAnimator;
    [SerializeField] private DashAccountInfoAnimator _accountInfoAnimator;
    private bool _isSettingsPanelOpen, _isResetAccountPanelOpen, _isAccountInfoPanelOpen;
    private GameObject _settingsPanel;
    
    
    public void ToggleSettingsPanel()
    {
        if (!_isSettingsPanelOpen)
        {
            _settingsPanel = Instantiate(settingsPanelPrefab, canvas.transform);
            _settingsPanel.GetComponent<SettingsPanel>().SetCloseAction(ToggleSettingsPanel);
        }
        else
        {
            _settingsPanel.GetComponent<SettingsPanel>().CloseSettingsPanel();
            Destroy(_settingsPanel);
            _settingsPanel = null;
        }
        _isSettingsPanelOpen.Toggle();
    }

    public void ResetAccount()
    {
        if (_isResetAccountPanelOpen)
        {
            _resetAccountAnimator.DeAnimateFrame();
            return;
        }
        _resetAccountAnimator.AnimateAccountInfoFrame();
    }

    public void AccountInfo()
    {
        if (_isAccountInfoPanelOpen)
        {
            _accountInfoAnimator.DeAnimateFrame();
            return;
        }
        _accountInfoAnimator.AnimateAccountInfoFrame();
    }
    
    public void Logout()
    {
        ApiManager.Instance.LogoutUser();
        SceneTransitionManager.Instance.LoadScene("NewLoginScreen");
    }

    public async Task SaveGame()
    {
        if (ApiManager.IsTrainer) return;
        await ApiManager.Instance.SaveGameData();

        saveStatus.transform.parent.gameObject.SetActive(true);
        saveStatus.text = "Save Success";
        Invoke(nameof(HideSaveStatus), 3f);
        
    }
    
    private void HideSaveStatus()
    {
        saveStatus.transform.parent.gameObject.SetActive(false);
    }
}
