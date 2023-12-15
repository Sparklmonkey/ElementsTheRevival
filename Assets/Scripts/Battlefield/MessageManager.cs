using UnityEngine;

public class MessageManager : MonoBehaviour
{
    [SerializeField]
    private GameObject messageAnimatedPrefab;
    public void DisplayMessage(string message)
    {
        var messageAnimatedObject = Instantiate(messageAnimatedPrefab, transform);
        messageAnimatedObject.GetComponent<ErrorAnimatedBattlefield>().DisplayAnimatedError(message);
    }
}
