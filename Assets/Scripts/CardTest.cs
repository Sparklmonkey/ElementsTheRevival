using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class CardTest : MonoBehaviour
{
    [SerializeField]
    private List<Card> cardList;
    public Dictionary<string, int> cardDict = new ();


    // Start is called before the first frame update
    void Start()
    {
        CardDatabase.Instance.SetupNewCardBase();
        List<string> innateString = new();
        foreach (var item in CardDatabase.Instance.fullCardList)
        {
            Debug.Log(item.innateSkills.Dagger);
        }


        foreach (var passive in innateString)
        {
            Debug.Log(passive);
        }
    }


    private async void SavePlayerTestAsync()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        var data = new Dictionary<string, object> { { "PlayerData", "HelloWorld" } };
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
        Debug.Log("Saved");
    } 
}