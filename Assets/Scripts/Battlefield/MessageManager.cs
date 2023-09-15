using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Shared;

    [SerializeField]
    private GameObject messageAnimatedPrefab;
    private void Awake()
    {
        Shared = this;
    }

    public void DisplayMessage(string message)
    {
        GameObject messageAnimatedObject = Instantiate(messageAnimatedPrefab, transform);
        messageAnimatedObject.GetComponent<ErrorAnimatedBattlefield>().DisplayAnimatedError(message);
    }


}
