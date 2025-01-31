using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.CloudCode;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using Unity.VisualScripting;
using UnityEngine;

public struct UserAttributes {
    // Optionally declare variables for any custom user attributes:
    public bool expansionFlag;
}

public struct AppAttributes {
    // Optionally declare variables for any custom app attributes:
    public string VersionNote;
    public bool IsMaintenance;
    public string MinVersion;
    public string GameNews;
    public FeatureFlags FeatureFlags;
}

public class FeatureFlags
{
    public bool CodeRedeem;
    public bool Achievements;
    public bool T50Arena;
    public bool Arena;
    public bool PvpOne;
    public bool PvpTwo;
    public bool PvpDuel;
}

public class CardTest : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        SetupRemoteConfig();
    }

    private async void SetupRemoteConfig()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        await RemoteConfigService.Instance.FetchConfigsAsync(new UserAttributes(), new AppAttributes());
        
        Debug.Log("-----------Min App Version-----------");
        Debug.Log(RemoteConfigService.Instance.appConfig.GetString("minVersion"));
        
        Debug.Log("-----------Version Note-----------");
        Debug.Log(RemoteConfigService.Instance.appConfig.GetString("versionNote"));
    }
    
}

[Serializable]
public class CardDBLegacy
{
    public List<CardDefinition> cardDb;
}

[Serializable]
public class RemoteConfigGameNews
{
    public List<GameNews> newsList;
    public string description;
    public string title;
}