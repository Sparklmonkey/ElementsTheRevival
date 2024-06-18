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
        achieveName.text = achievementData.name;
        var status = achievementData.isAchieved ? "Complete" : "In Progress";
        tooltipTrigger.SetText("TitleText", achievementData.name);
        tooltipTrigger.SetText("BodyText", achievementData.description);
        tooltipTrigger.SetText("ProgressText", $"{achievementData.completionPercent}% Complete");
        tooltipTrigger.SetText("StatusText", status);
        progressBar.sprite =
            ImageHelper.GetElementImage(((Element)achievementData.element).FastElementString());
        progressBar.fillAmount = achievementData.completionPercent / 100.0f;
        achieveFrameImg.sprite = ImageHelper.GetAchievementFrame(achievementData.rarity);
        // progressBar.sprite.fill
    }
    
    
}