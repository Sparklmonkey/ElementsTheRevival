using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class AchievementCell : MonoBehaviour
{
    [SerializeField]
    private Image achieveImg, rewardImg;
    [SerializeField]
    private TextMeshProUGUI achieveName, achieveRequire, rewardAmount;

public void SetupCell(AchievementData achievementData)
{
    achieveName.text = achievementData.achieveName;
    achieveRequire.text = achievementData.achieveDesc;
    rewardAmount.text = achievementData.rewardAmount.ToString();
}
}

[Serializable]
public class AchievementData
{
    public string achieveName;
    public string achieveDesc;
    public int rewardAmount;
    public bool isCard;
}