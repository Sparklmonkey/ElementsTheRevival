using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Dashboard;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public delegate void NewsUpdateMethod(GameNews gameNews);
public class NewsPanelManager : MonoBehaviour
{
    [SerializeField] private GameObject newsPrefab;
    [SerializeField] private RectTransform topImageContainer, bottomImageContainer, detailPanel;
    [SerializeField] private RawImage topImage, bottomImage;
    [SerializeField] private TextMeshProUGUI subtitle, description;
    [SerializeField] private Transform content;
    
    private List<GameNews> _newsList;

    private void Awake()
    {
        _newsList = SessionManager.Instance.GameNews;
        foreach (var newsItem in _newsList)
        {
            var newsObject = Instantiate(newsPrefab, content);
            var newsScript = newsObject.GetComponent<NewsItem>();
            newsScript.SetupItem(newsItem, UpdateSelectedNewsItem);
        }
        UpdateSelectedNewsItem(_newsList[0]);
    }

    private void UpdateSelectedNewsItem(GameNews gameNews)
    {
        StopAllCoroutines();
        subtitle.text = gameNews.subtitle;
        description.text = gameNews.description;

        topImageContainer.sizeDelta = new Vector2(topImageContainer.rect.width, 0);
        bottomImageContainer.sizeDelta = new Vector2(bottomImageContainer.rect.width, 0);
        if (gameNews.topImageUrl.Contains("https"))
        {
            StartCoroutine(DownloadImage(gameNews.topImageUrl, topImage, topImageContainer));
        }
        
        if (gameNews.bottomImageUrl.Contains("https"))
        {
            StartCoroutine(DownloadImage(gameNews.bottomImageUrl, bottomImage, bottomImageContainer));
        }
    }
    
    private IEnumerator DownloadImage(string mediaUrl, RawImage image, RectTransform rectTransform)
    {   
        var request = UnityWebRequestTexture.GetTexture(mediaUrl);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            var text = ((DownloadHandlerTexture)request.downloadHandler).texture;
            image.texture = text;
            var scaledData = ScaleResolution(text.width, text.height, Mathf.RoundToInt(rectTransform.rect.width), 1000);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scaledData.x);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scaledData.y);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(detailPanel);
    } 
    
    private Vector2 ScaleResolution(int width, int height, int maxWidth, int maxHeight)
    {
        int new_width = width;
        int new_height = height;

        if (width > height){
            new_width = maxWidth;
            new_height = (new_width * height) / width;
        }
        else
        {
            new_height = maxHeight;
            new_width = (new_height * width) / height;
        }

        var dimension = new Vector2(new_width, new_height);
        return dimension;
    }
}

[Serializable]
public class GameNews
{
    public int id;
    public string title;
    public string subtitle;
    public string description;
    public string topImageUrl;
    public string bottomImageUrl;
}