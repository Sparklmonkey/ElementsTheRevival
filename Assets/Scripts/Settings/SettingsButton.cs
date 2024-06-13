using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ButtonActionNoParams();
namespace Settings
{
public class SettingsButton : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanelPrefab;
    [SerializeField] private Transform parentTransform;
    private GameObject _settingsPanel;
    
    public void OpenSettingsPanel()
    {
        if (_settingsPanel is not null)
        {
            return;
        }
        _settingsPanel = Instantiate(settingsPanelPrefab, parentTransform);
        _settingsPanel.GetComponent<SettingsPanel>().SetCloseAction(CloseSettingsPanel);
    }

    public void CloseSettingsPanel()
    {
        _settingsPanel.GetComponent<SettingsPanel>().CloseSettingsPanel();
        Destroy(_settingsPanel);
        _settingsPanel = null;
    }
}
}