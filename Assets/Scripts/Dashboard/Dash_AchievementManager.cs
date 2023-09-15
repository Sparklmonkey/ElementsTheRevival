using System.Collections.Generic;
using UnityEngine;

public class DashAchievementManager : MonoBehaviour
{
    [SerializeField]
    private List<AchievementCell> achievementCells;
    private List<AchievementData> _listOfPossibleAchievements;

    private int _pageIndex;

    private void ChangePage(bool isNext)
    {
        _pageIndex += isNext ? 1 : -1;
        if (_pageIndex * 4 > _listOfPossibleAchievements.Count) { _pageIndex = 0; }
        if (_pageIndex < 0) { _pageIndex = _listOfPossibleAchievements.Count / 4; }
    }

    public void SetupFirstPage()
    {

    }

}
