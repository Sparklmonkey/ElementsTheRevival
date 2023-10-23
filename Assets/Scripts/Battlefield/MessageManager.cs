using UnityEngine;

public class MessageManager : MonoBehaviour
{
    [SerializeField]
    private GameObject messageAnimatedPrefab;
    public void DisplayMessage(string message)
    {
        GameObject messageAnimatedObject = Instantiate(messageAnimatedPrefab, transform);
        messageAnimatedObject.GetComponent<ErrorAnimatedBattlefield>().DisplayAnimatedError(message);
    }
}
