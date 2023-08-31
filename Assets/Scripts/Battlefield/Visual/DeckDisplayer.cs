using UnityEngine;

namespace Elements.Duel.Visual
{

    public class DeckDisplayer : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI deckCount;

        public void UpdateDeckCount(int count) => deckCount.text = count.ToString();
    }
}