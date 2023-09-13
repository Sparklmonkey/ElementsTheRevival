using System.Collections.Generic;
using UnityEngine;

public class Dash_AchievementManager : MonoBehaviour
{
    [SerializeField]
    private List<AchievementCell> achievementCells;
    private List<AchievementData> listOfPossibleAchievements;

    private int pageIndex;

    private void ChangePage(bool isNext)
    {
        pageIndex += isNext ? 1 : -1;
        if (pageIndex * 4 > listOfPossibleAchievements.Count) { pageIndex = 0; }
        if (pageIndex < 0) { pageIndex = listOfPossibleAchievements.Count / 4; }
    }

    public void SetupFirstPage()
    {

    }

}
