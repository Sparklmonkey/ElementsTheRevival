
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using UnityEngine;


//public class Game_PvpHubConnection : MonoBehaviour
//{
//    public static Game_PvpHubConnection shared;
//    [SerializeField]
//    private TMPro.TextMeshProUGUI errorMessage;

//    private PvpConnectionData pvpConnectionData = new PvpConnectionData();

//    public Element GetOpponentMark()
//    {
//        return pvpConnectionData.opponentUserInfo.ElementMark;
//    }

//    private static HubConnection connection;
//    private PvP_LobbyConnecteduser pvp_LobbyConnecteduser;
//    private string BaseUrl = "http://localhost:5000/pvpHub";//"https://elementstheresource.azure-api.net/pvpHub";
//    // Start is called before the first frame update
//    void Awake()
//    {
//        if(shared != null) { return; }
//        shared = new Game_PvpHubConnection();
//        DontDestroyOnLoad(gameObject);
//    }

//    public async void StartPvpHubConnection()
//    {
//        connection = new HubConnectionBuilder()
//            .WithUrl(BaseUrl, options =>
//            {
//                options.AccessTokenProvider = () => Task.FromResult(ApiManager.shared.GetToken());
//                //options.Headers.Add("Ocp-Apim-Subscription-Key", "74041a9edafd4118a226d2106a4c498d");
//            })
//            .Build();
//        connection.Closed += async (error) =>
//        {
//            await Task.Delay(UnityEngine.Random.Range(0, 5) * 1000);
//            await connection.StartAsync();
//        };
//        await Connect();
//        pvp_LobbyConnecteduser = GameObject.Find("Canvas").GetComponent<PvP_LobbyConnecteduser>();
//        pvp_LobbyConnecteduser.SetupPvpScreen(null);
//    }


//    public async void ConnectToPvpRoom()
//    {
//        Debug.Log("Find Opponent");
//        await connection.InvokeAsync("StartPvpConnection", 1);
//    }

//    public void ConfirmOpponent()
//    {
//        Debug.Log("Confirm Opponent");
//        connection.InvokeAsync("ConfirmOpponentConnection", pvpConnectionData.roomId, 1);
//    }

//    public async void SendShuffledDeck(List<CardObject> shuffledDeck)
//    {
//        await connection.InvokeAsync("SendShuffledDeck", pvpConnectionData.roomId, shuffledDeck, 1);
//    }
//    private async Task Connect()
//    {

//        connection.On<string>("ReceiveMessage", message =>
//        {
//            Debug.Log($"MessageReceived: {message}");
//        });

//        connection.On<Guid, PvpUserInfo>("UpdatePvpOpScreen", (roomId, opponenInfo) =>
//        {
//            pvpConnectionData.roomId = roomId;
//            if (opponenInfo == null) { return; }
//            pvpConnectionData.opponentUserInfo = opponenInfo;
//            pvp_LobbyConnecteduser.UpdateOpInfo(opponenInfo);
//        });

//        connection.On<string>("ReceivePvpAction", pvpActionAsJson =>
//        {
//            new PvPCommand_ReceiveAction(JsonUtility.FromJson<PvP_Action>(pvpActionAsJson)).AddToQueue();
//        });

//        connection.On<int, bool, List<CardObject>, List<CardObject>>("GetCoinFlipCount", (coinFlipCount, willStart, myDeck, opponentDeck) =>
//        {
//            DuelManager.opponentShuffledDeck = opponentDeck;
//            PlayerData.shared.currentDeck = myDeck;
//            BattleVars.shared.coinFlip = coinFlipCount;
//            BattleVars.shared.willStart = willStart;
//            BattleVars.shared.isPvp = true;
//            Debug.Log("Confirmation Complete");
//            pvp_LobbyConnecteduser.MoveToPvpBattlefield();
//        });

//        connection.On<List<QuantaObject>>("ReceiveQuantaObjects", oppoentnQuanta =>
//        {
//            BattleVars.shared.opponentPvpQuanta = oppoentnQuanta;

//            new PvPCommand_ReceiveAction(new PvP_Action(ActionType.EndTurn, null, null)).AddToQueue();
//        });

//        connection.On<string>("OpDisconnect", disconnect => {
//            //DisconnetionFromHub();
//        });

//        try
//        {
//            await connection.StartAsync();

//            Debug.Log("Connection started");
//        }
//        catch (Exception ex)
//        {
//            errorMessage.text = ex.Message;
//            Debug.Log(ex.Message);
//        }
//    }
//    // Update is called once per frame
//    void Update()
//    {
        
//    }

//    public string GetOpponentName()
//    {
//        return pvpConnectionData.opponentUserInfo.Username;
//    }

//    public void SendPvpAction(PvP_Action pvP_Action)
//    {
//        connection.InvokeAsync("SendPvpAction", pvpConnectionData.roomId, pvP_Action, 1);
//    }

//    public void EndPlayerTurn(List<QuantaObject> pvpQuantaList)
//    {
//        connection.InvokeAsync("EndPlayerTurn", pvpConnectionData.roomId, pvpQuantaList, 1);
//    }

//    //private void DisconnetionFromHub()
//    //{
//    //    GetComponent<DashboardSceneManager>().LoadNewScene("Dashboard");
//    //}

//}
