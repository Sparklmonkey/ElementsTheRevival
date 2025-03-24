using System;
using Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

public class RemoteConfigHelper
{
    private static RemoteConfigHelper _instance;
    public static RemoteConfigHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new RemoteConfigHelper();
            }
            return _instance;
        }
    }
    private FeatureFlags _featureFlags;

    public void SetFeatureFlags(string featureFlags)
    {
        _featureFlags = JsonUtility.FromJson<FeatureFlags>(featureFlags);
    }
    public void SetupGameNews()
    {
        var news = RemoteConfigService.Instance.appConfig.GetJson("GameNews");
        var newsItem = JsonUtility.FromJson<RemoteConfigGameNews>(news);
        SessionManager.Instance.GameNews = newsItem.newsList;
    }

    public bool IsForceUpdate()
    {
        var minVersion = RemoteConfigService.Instance.appConfig.GetString("MinVersion");
        
        var version1 = new Version(Application.version);
        var version2 = new Version(minVersion);
        var result = version1.CompareTo(version2);
        return result < 0;
    }

    public bool IsMaintenance()
    {
        return RemoteConfigService.Instance.appConfig.GetBool("IsMaintenance");
    }

    public bool IsFeatureEnabled(FeatureType flag)
    {
        return flag switch
        {
            FeatureType.CodeRedeem => _featureFlags.CodeRedeem,
            FeatureType.Achievements => _featureFlags.Achievements,
            FeatureType.T50Arena => _featureFlags.T50Arena,
            FeatureType.Arena => _featureFlags.Arena,
            FeatureType.PvpOne => _featureFlags.PvpOne,
            FeatureType.PvpTwo => _featureFlags.PvpTwo,
            FeatureType.PvpDuel => _featureFlags.PvpDuel,
            _ => false
        };
    }
}

public enum FeatureType
{
    CodeRedeem,
    Achievements,
    T50Arena,
    Arena,
    PvpOne,
    PvpTwo,
    PvpDuel
}