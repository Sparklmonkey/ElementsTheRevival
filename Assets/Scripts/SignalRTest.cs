using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

public class SignalRTest : MonoBehaviour
{
    [FormerlySerializedAs("UserName")] public string userName;
    private int _x;
    private static HubConnection _connection;
    private string _connectionId;
    private bool _shouldUpdateList = false;
    private List<string> _connectedUsers;

    private void Start()
    {

        userName = "Sparklmonkey";
        SetupHubConnection();
    }

    public async void SetupHubConnection()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5158/pvpHub")
            .Build();
        _connection.Closed += async (error) =>
        {
            await Task.Delay(Random.Range(0, 5) * 1000);
            await _connection.StartAsync();
        };
        await Connect();
    }

    //Setup Connection and Methods
    private async Task Connect()
    {
        _connection.On("ReceiveConnId", (string message) =>
        {
            _connectionId = message;
            Debug.Log($"ConnID: {message}");
        });

        _connection.On<string>("ReceiveMessage", message =>
        {
            Debug.Log($"MessageReceived: {message}");
        });

        _connection.On<List<string>>("UpdateConnectedList", connectedUsers =>
        {
            this._connectedUsers = connectedUsers;
            _shouldUpdateList = true;
        });

        _connection.On<string>("RoomCreated", response =>
        {
            var conResponse = JsonConvert.DeserializeObject<ConnectionResponse>(response);
            Debug.Log(conResponse.ConnectionId);
            Debug.Log(conResponse.ServerMessage);
        });

        try
        {
            await _connection.StartAsync();

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
                AccountId = "4b61231c-174b-40bd-9d9f-13bcd6e22e47",
                IsOpenRoom = false,
                IsPvpOne = true,
                Mark = 1,
                PvpDeck = new List<string>() { "", "", "" }
            };
            await _connection.InvokeAsync<string>("StartPvpRoom", JsonConvert.SerializeObject(conRequest));
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
            await _connection.InvokeAsync("RefreshConnectedClients");
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }




    private void OnApplicationQuit()
    {
        _connection.InvokeAsync("DisconnectFromHub");
        _connection.DisposeAsync();
    }


    private async Task Send(string msg)
    {
        try
        {
            await _connection.InvokeAsync("SendMessageAsync", msg, _connectionId);
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

}

public class ConnectionRequest
{
    public string AccountId;
    public bool IsPvpOne;
    public bool IsOpenRoom;
    public List<string> PvpDeck;
    public int Mark;
}
public class ConnectionResponse
{
    public string ConnectionId;
    public string ServerMessage;
}