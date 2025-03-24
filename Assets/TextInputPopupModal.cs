using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class TextInputPopupModal : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI titleText, buttonText;
    [SerializeField] private LocalizedString localizedTitle, localizedButton;
    [SerializeField] private Button actionButton;
    
    public void SetupTextInputModal(string localeTable, string messageTitleKey, string buttonTitleKey, ButtonActionStringParam actionButtonMethod)
    {
        actionButton.onClick.RemoveAllListeners();
        actionButton.onClick.AddListener(delegate { actionButtonMethod(inputField.text); });
        localizedTitle.SetReference(localeTable,messageTitleKey);
        localizedButton.SetReference(localeTable,buttonTitleKey);
        localizedTitle.StringChanged += UpdateTitle;
        localizedButton.StringChanged += UpdateButtonOne;
    }
    
    private void UpdateTitle(string value)
    {
        titleText.text = value;
    }

    private void UpdateButtonOne(string value)
    {
        buttonText.text = value;
    }

}
