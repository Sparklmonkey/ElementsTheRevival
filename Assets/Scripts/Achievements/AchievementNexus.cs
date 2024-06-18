using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class AchievementNexus : MonoBehaviour
{
    private int _pageIndex, _totalPages;
    public List<AchievementCell> achievementCells;
    public GameObject nextPage, previousPage;
    // Start is called before the first frame update
    private void Start()
    {
        _totalPages = Mathf.CeilToInt(SessionManager.Instance.Achievements.Count / 12.0f);
        SetupCurrentPage(1);
    }

    public void ChangePage(bool isNext)
    {
        var nextIndex = isNext ? _pageIndex + 1 : _pageIndex - 1;
        if (nextIndex < 1)
        {
            nextIndex = 1;
        }
        else if (nextIndex > _totalPages)
        {
            nextIndex = _totalPages;
        }
        SetupCurrentPage(nextIndex);
    }

    private void SetupCurrentPage(int pageIndex)
    {
        _pageIndex = pageIndex;
        previousPage.SetActive(pageIndex > 1);
        nextPage.SetActive(pageIndex < _totalPages);

        var maxIndex = _pageIndex * 12;
        var minIndex = maxIndex - 12;
        var currentAchievements = SessionManager.Instance.Achievements;
        var cellIndex = 0;
        for (int i = minIndex; i < maxIndex; i++)
        {
            if (currentAchievements.Count <= i)
            {
                achievementCells[cellIndex].gameObject.SetActive(false);
                cellIndex++;
                continue;
            };
            achievementCells[cellIndex].gameObject.SetActive(true);
            achievementCells[cellIndex].SetupCell(currentAchievements[i]);
            cellIndex++;
        }
    }
}
