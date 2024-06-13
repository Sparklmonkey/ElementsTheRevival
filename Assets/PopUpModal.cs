using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpModal : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI popUpMessage, buttonText;
    [SerializeField] private Button actionButton;

    public void SetupModal(string popUpMessageString, string buttonTitle, ButtonActionNoParams actionButtonMethod)
    {
        popUpMessage.text = popUpMessageString;
        actionButton.onClick.RemoveAllListeners();
        buttonText.text = buttonTitle;
        actionButton.onClick.AddListener(delegate { actionButtonMethod(); });
    }
}
