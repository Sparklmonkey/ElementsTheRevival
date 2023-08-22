using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public static MessageManager shared;

    [SerializeField]
    private GameObject messageAnimatedPrefab;
    private void Awake()
    {
        shared = this;
    }

    public void DisplayMessage(string message)
    {
        GameObject messageAnimatedObject = Instantiate(messageAnimatedPrefab, transform);
        messageAnimatedObject.GetComponent<Error_Animated_Battlefield>().DisplayAnimatedError(message);
    }


}
