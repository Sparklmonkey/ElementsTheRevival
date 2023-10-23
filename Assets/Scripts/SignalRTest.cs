using Microsoft.AspNetCore.SignalR.Client;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class SignalRTest : MonoBehaviour
{
    public string UserName;
    private int x;
    private static HubConnection connection;
    private string connectionId;
    private bool shouldUpdateList = false;
    private List<string> connectedUsers;
    void Start()
    {

        UserName = "Sparklmonkey";

        Debug.Log("Hello World!");
        connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5158/pvphub")
                                 .Build();
        connection.Closed += async (error) =>
        {
            await Task.Delay(Random.Range(0, 5) * 1000);
            await connection.StartAsync();
        };
        ConnectionTest();
    }

    public void SetupHubConnection()
    {
        connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5158/pvpHub")
            .Build();
        connection.Closed += async (error) =>
        {
            await Task.Delay(Random.Range(0, 5) * 1000);
            await connection.StartAsync();
        };
    }

    //Setup Connection and Methods
    private async Task Connect()
    {
        connection.On<string>("ReceiveConnID", message =>
        {
            connectionId = message;
            Debug.Log($"ConnID: {message}");
        });

        connection.On<string>("ReceiveMessage", message =>
        {
            Debug.Log($"MessageReceived: {message}");
        });

        connection.On<List<string>>("UpdateConnectedList", connectedUsers =>
        {
            this.connectedUsers = connectedUsers;
            shouldUpdateList = true;
        });

        connection.On<string>("RoomCreated", response =>
        {
            var conResponse = JsonConvert.DeserializeObject<ConnectionResponse>(response);
            Debug.Log(conResponse.connectionId);
            Debug.Log(conResponse.serverMessage);
        });

        try
        {
            await connection.StartAsync();

            Debug.Log("Connection started");
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    private async void ConnectionTest()
    {
        await Connect();
        // RefreshConnectedUsers();
    }


    public async void CreateNewPvpRoom()
    {
        try
        {
            var conRequest = new ConnectionRequest()
            {
                accountId = "4b61231c-174b-40bd-9d9f-13bcd6e22e47",
                isOpenRoom = false,
                isPvpOne = true,
                mark = 1,
                pvpDeck = new List<string>() { "", "", "" }
            };
            await connection.InvokeAsync<string>("StartPvpRoom", JsonConvert.SerializeObject(conRequest));
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    
    public async void RefreshConnectedUsers()
    {
        try
        {
            await connection.InvokeAsync("RefreshConnectedClients");
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }




    private void OnApplicationQuit()
    {
        connection.InvokeAsync("DisconnectFromHub");
        connection.DisposeAsync();
    }


    private async Task Send(string msg)
    {
        try
        {
            await connection.InvokeAsync("SendMessageAsync", msg, connectionId);
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

}

public class ConnectionRequest
{
    public string accountId;
    public bool isPvpOne;
    public bool isOpenRoom;
    public List<string> pvpDeck;
    public int mark;
}
public class ConnectionResponse
{
    public string connectionId;
    public string serverMessage;
}