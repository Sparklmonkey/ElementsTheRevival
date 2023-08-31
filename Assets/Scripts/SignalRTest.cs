//using Microsoft.AspNetCore.SignalR.Client;
//using System.Collections;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using UnityEngine;

//public class SignalRTest : MonoBehaviour
//{
//    public string UserName;
//    private int x;
//    private static HubConnection connection;
//    [SerializeField]
//    private Error_Animated error_Anim;
//    [SerializeField]
//    private Transform lobbyPanelContent;
//    [SerializeField]
//    private GameObject userNamePrefab;

//    private bool shouldUpdateList = false;
//    private List<string> connectedUsers;
//    //void Start()
//    //{

//    //    UserName = "Sparklmonkey";
        
//    //    Debug.Log("Hello World!");
//    //    connection = new HubConnectionBuilder()
//    //        .WithUrl("http://localhost:5000/pvpHub", options =>
//    //        {
//    //            options.AccessTokenProvider = () => Task.FromResult("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJFdEciLCJuYW1lIjoiU3BhcmtsbW9ua2V5IiwicGxheWVySUQiOiI2MTc4OTllMzFlMmM0ZGExMzQ5YmQxNTIiLCJleHAiOjE2MzYzOTUwODQsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.ub62nS5oUDqQhUcGvV_l6bYnnPJZtY2KZsZ_Pc9peZ0");
//    //        })
//    //        .Build();
//    //    connection.Closed += async (error) =>
//    //    {
//    //        await Task.Delay(Random.Range(0, 5) * 1000);
//    //        await connection.StartAsync();
//    //    };
//    //    ConnectionTest();
//    //}

//    //Connect to Hub

//    public void SetupHubConnection()
//    {
//        connection = new HubConnectionBuilder()
//            .WithUrl("http://localhost:5000/pvpHub") //Hub http url 
//            .Build();
//        connection.Closed += async (error) =>
//        {
//            await Task.Delay(Random.Range(0, 5) * 1000);
//            await connection.StartAsync();
//        };
//    }

//    //Seupt Connection and Methods
//    private async Task Connect()
//    {
//        //Add methods to connection that are called from SignalR Hub
//        //<> -> parameters received from Hub
//        //First string Value is the name of the method that is called by the Hub
//        connection.On<string>("ReceiveConnID", message =>
//        {
//            connectionId = message;
//            Debug.Log($"ConnID: {message}");
//        });

//        connection.On<string>("ReceiveMessage", message =>
//        {
//            Debug.Log($"MessageReceived: {message}");
//        });

//        connection.On<List<string>>("UpdateConnectedList", connectedUsers =>
//        {
//            this.connectedUsers = connectedUsers;
//            shouldUpdateList = true;
//        });

//        try
//        {
//            await connection.StartAsync();

//            Debug.Log("Connection started");
//            error_Anim.DisplayAnimatedError("Connection Successful");
//        }
//        catch (System.Exception ex)
//        {
//            Debug.Log(ex.Message);
//        }
//    }

//    private async void ConnectionTest()
//    {
//        await Connect();
//        RefreshConnectedUsers();
//    }

//    public async void RefreshConnectedUsers()
//    {
//        try
//        {
//            await connection.InvokeAsync("RefreshConnectedClients");
//        }
//        catch (System.Exception ex)
//        {
//            Debug.Log(ex.Message);
//        }
//    }

//    private string connectionId = "";


//    private void SetupConnectedUserPanel(List<string> connectedUsers)
//    {
//        foreach (string username in connectedUsers)
//        {
//            GameObject userNameObject = Instantiate(userNamePrefab, lobbyPanelContent);
//            //userNameObject.GetComponent<PvP_LobbyConnecteduser>().SetupUsername(username);
//        }
//    }

//    //private void OnApplicationQuit()
//    //{
//    //    connection.InvokeAsync("DisconnectFromHub");
//    //    connection.DisposeAsync();
//    //}


//    private async Task Send(string msg)
//    {
//        try
//        {
//            await connection.InvokeAsync("SendMessageAsync", msg, connectionId);
//        }
//        catch (System.Exception ex)
//        {
//            Debug.Log(ex.Message);
//        }
//    }

//    private void Update()
//    {
//        if(!shouldUpdateList) { return; }
//        foreach (Transform item in lobbyPanelContent)
//        {
//            Destroy(item.gameObject);
//        }
//        SetupConnectedUserPanel(connectedUsers);
//        shouldUpdateList = false;
//    }
//}
