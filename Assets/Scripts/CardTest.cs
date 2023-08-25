using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System.Collections.Generic;
using UnityEngine;

public class CardTest : MonoBehaviour
{
    [SerializeField]
    private List<Card> cardList;
    public Dictionary<string, int> cardDict = new ();


    // Start is called before the first frame update
    async void Start()
    {
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        await ApiManager.shared.GetAppInfo();
    }

}