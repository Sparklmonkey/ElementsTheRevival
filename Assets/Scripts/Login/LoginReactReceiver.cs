using System.Collections.Generic;
using System.Linq;
using Settings;
using Unity.Services.Authentication;
using UnityEngine;

public class LoginReactReceiver : MonoBehaviour
{
    [SerializeField]
    private GameObject canvas, settingsPanelPrefab;
    private bool _isSettingsPanelOpen;
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

    public void LoginToTrainerAccount()
    {
        PlayerPrefs.SetInt("IsTrainer", 1);
        AuthenticationService.Instance.SignOut(true);
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
        PlayerData.Shared.MarkElement = Element.Darkness;
        PlayerData.Shared.CurrentQuestIndex = 8;
        PlayerData.Shared.Electrum = 9999999;
        SceneTransitionManager.Instance.LoadScene("Dashboard");
    }
}
