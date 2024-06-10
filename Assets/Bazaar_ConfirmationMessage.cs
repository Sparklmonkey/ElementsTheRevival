using System.Collections;
using System.Collections.Generic;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bazaar_ConfirmationMessage : MonoBehaviour
{
    public Toggle hideConfirmation;
    [SerializeField]
    private TextMeshProUGUI errorText;
    public void SetupErrorMessage(string errorMessage)
    {
        hideConfirmation.isOn = SessionManager.Instance.ShouldHideConfirm;
        errorText.text = errorMessage;
        gameObject.SetActive(true);
    }
}
