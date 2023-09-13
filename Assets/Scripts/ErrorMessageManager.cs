using TMPro;
using UnityEngine;

public class ErrorMessageManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI errorText;
    public void SetupErrorMessage(string errorMessage)
    {
        errorText.text = errorMessage;
        gameObject.SetActive(true);
    }

}
