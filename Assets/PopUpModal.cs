using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

public class PopUpModal : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI popUpMessage, buttonOneText, buttonTwoText;
    [SerializeField] private LocalizedString localizedTitle, localizedButtonOne, localizedButtonTwo;
    [SerializeField] private Button actionOneButton, actionTwoButton;

    public void SetupModal(string localeTable, string messageTitleKey, string buttonTitleKey, ButtonActionNoParams actionButtonMethod)
    {
        actionOneButton.onClick.RemoveAllListeners();
        actionOneButton.onClick.AddListener(delegate { actionButtonMethod(); });
        actionTwoButton.gameObject.SetActive(false);
        localizedTitle.SetReference(localeTable,messageTitleKey);
        localizedButtonOne.SetReference(localeTable,buttonTitleKey);
        localizedTitle.StringChanged += UpdateTitle;
        localizedButtonOne.StringChanged += UpdateButtonOne;
    }

    public void SetupModalWithTwoButtons(string localeTable, string messageTitleKey, string buttonTitleOneKey, string buttonTitleTwoKey, ButtonActionNoParams actionButtonOneMethod, ButtonActionNoParams actionButtonTwoMethod)
    {
        actionOneButton.onClick.RemoveAllListeners();
        actionOneButton.onClick.AddListener(delegate { actionButtonOneMethod(); });
        actionTwoButton.onClick.RemoveAllListeners();
        actionTwoButton.onClick.AddListener(delegate { actionButtonTwoMethod(); });
        localizedTitle.SetReference(localeTable,messageTitleKey);
        localizedButtonOne.SetReference(localeTable,buttonTitleOneKey);
        localizedButtonTwo.SetReference(localeTable,buttonTitleTwoKey);
        localizedTitle.StringChanged += UpdateTitle;
        localizedButtonOne.StringChanged += UpdateButtonOne;
        localizedButtonTwo.StringChanged += UpdateButtonTwo;
    }
    private void UpdateTitle(string value)
    {
        popUpMessage.text = value;
    }

    private void UpdateButtonOne(string value)
    {
        buttonOneText.text = value;
    }

    private void UpdateButtonTwo(string value)
    {
        buttonTwoText.text = value;
    }

    private void OnDisable()
    {
        localizedTitle.StringChanged -= UpdateTitle;
        localizedButtonOne.StringChanged -= UpdateButtonOne;
        localizedButtonTwo.StringChanged -= UpdateButtonTwo;
    }
}
