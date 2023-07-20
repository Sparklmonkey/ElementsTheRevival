using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class AchievementCell : MonoBehaviour
{
    [SerializeField]
    private Image achieveFrameImg;
    [SerializeField]
    private TextMeshProUGUI achieveName, achieveCount, achieveDesc;

    public void SetupCell(AchievementData achievementData)
    {
        achieveName.text = achievementData.achieveName;
        achieveDesc.text = achievementData.achieveDesc;
        achieveCount.text = achievementData.achieveCount.ToString();
        var test = " _31ok_l1su_61rj_18pn";
    }
}

[Serializable]
public class AchievementData
{
    public string achieveName;
    public string achieveDesc;
    public int achieveCount;

    public AchievementData(string achievement)
    {

    }
}