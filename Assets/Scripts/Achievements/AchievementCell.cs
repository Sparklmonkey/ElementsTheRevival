using System;
using Achievements;
using ModelShark;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementCell : MonoBehaviour
{
    [SerializeField]
    private Image achieveFrameImg, progressBar;
    [SerializeField]
    private TooltipTrigger tooltipTrigger;
    [SerializeField]
    private TextMeshProUGUI achieveName, achieveCount;

    public void SetupCell(PlayerAchievement achievementData)
    {
        achieveCount.text = achievementData.Id.ToString();
        achieveName.text = achievementData.Name;
        var status = achievementData.CompletionPercent == 100 ? "Complete" : "In Progress";
        tooltipTrigger.SetText("TitleText", achievementData.Name);
        tooltipTrigger.SetText("BodyText", achievementData.Description);
        tooltipTrigger.SetText("ProgressText", $"{achievementData.CompletionPercent}% Complete");
        tooltipTrigger.SetText("StatusText", status);
        progressBar.sprite =
            ImageHelper.GetElementImage(((Element)achievementData.Element).FastElementString());
        progressBar.fillAmount = achievementData.CompletionPercent / 100.0f;
        achieveFrameImg.sprite = ImageHelper.GetAchievementFrame(achievementData.TierAchieved);
        // progressBar.sprite.fill
    }
    
    
}