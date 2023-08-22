using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class CardTest : MonoBehaviour
{
    [SerializeField]
    private List<Card> cardList;
    public Dictionary<string, int> cardDict = new ();


    // Start is called before the first frame update
    void Start()
    {

        foreach (var card in CardDatabase.Instance.trainerCardList)
        {
            if(CardDatabase.Instance.fullCardList.Find(x => x.iD == card) == null)
            {
                Debug.Log(card);
            }
        }


#if UNITY_ANDROID
        InitializePlayGamesLogin();
        LoginGoogle();
#endif
        //CardDatabase.Instance.SetupNewCardBase();
        //List<string> innateString = new();
        //foreach (var item in CardDatabase.Instance.fullCardList)
        //{
        //    Debug.Log(item.innateSkills.Dagger);
        //}


        //foreach (var passive in innateString)
        //{
        //    Debug.Log(passive);
        //}
    }



    private async void SavePlayerTestAsync()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        var data = new Dictionary<string, object> { { "PlayerData", "HelloWorld" } };
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
        Debug.Log("Saved");
    }

#if UNITY_ANDROID

    void LoginGoogle()
    {
        Social.localUser.Authenticate(OnGoogleLogin);
    }

    void InitializePlayGamesLogin()
    {

        var config = new PlayGamesClientConfiguration.Builder()
            // Requests an ID token be generated.  
            // This OAuth token can be used to
            // identify the player to other services such as Firebase.
            .RequestIdToken()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }


    void OnGoogleLogin(bool success)
    {
        if (success)
        {
            // Call Unity Authentication SDK to sign in or link with Google.
            Debug.Log("Login with Google done. IdToken: " + ((PlayGamesLocalUser)Social.localUser).GetIdToken());
        }
        else
        {
            Debug.Log("Unsuccessful login");
        }
    }
#endif
}