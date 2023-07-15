using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManagerViewPlacement : MonoBehaviour
{
    private float xStart = 0.008f;
    private float yStart = 0.981f;
    private float xAxisDiff = 0.023f;
    private float yAxisDiff = 0.017f;
    private float widthDiff = 0.139f;
    private float heightDiff = 0.071f;

    public List<RectTransform> cardHeads;

    private void Awake()
    {
        int yCount = 0;
        int xCount = 0;

        foreach (var cardH in cardHeads)
        {
            float xMin = xStart + (xAxisDiff * xCount) + (widthDiff * xCount);
            float yMin = yStart - (yAxisDiff * yCount) - (heightDiff * yCount);
            cardH.anchorMin = new(xMin, yMin - heightDiff);
            cardH.anchorMax = new(xMin + widthDiff, yMin);
            cardH.offsetMin = new(0, 0);
            cardH.offsetMax = new(0, 0);


            yCount++;
            if (yCount > 9) 
            { 
                yCount = 0;
                xCount++;
            }
        }
    }

    public void SetupCardHeadAnchors(List<Card> cardList, CardDisplay cardDisplay)
    {
        for (int i = 0; i < cardHeads.Count; i++)
        {
            cardHeads[i].gameObject.SetActive(true);
            if (i > cardList.Count) { cardHeads[i].gameObject.SetActive(false); }
            cardHeads[i].GetComponent<StarterDeckCardHead>().SetupCardHead(cardList[i], cardDisplay);
        }
    }
}
