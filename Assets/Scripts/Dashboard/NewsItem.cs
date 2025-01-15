using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dashboard
{
    public class NewsItem : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private TextMeshProUGUI title;
        private GameNews _gameNews;
        private NewsUpdateMethod _updateMethod;
        public void SetupItem(GameNews gameNews, NewsUpdateMethod updateMethod)
        {
            _gameNews = gameNews;
            title.text = _gameNews.newsTitle;
            _updateMethod = updateMethod;
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            _updateMethod(_gameNews);
        }
    }
}