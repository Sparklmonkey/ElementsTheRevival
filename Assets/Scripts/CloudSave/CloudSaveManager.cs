using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSaveManager : MonoBehaviour
{
    private static readonly CloudSaveManager instance = new();
    
    static CloudSaveManager()
    {
    }

    private CloudSaveManager()
    {
    }

    public static CloudSaveManager Instance
    {
        get
        {
            return instance;
        }
    }


    private async void SavePlayerTestAsync()
    {
        await UnityServices.InitializeAsync();
        var data = new Dictionary<string, object> { { "PlayerData", PlayerData.shared } };
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
        Debug.Log("https://icons8.com/icon/46579/google-play");
    }

}