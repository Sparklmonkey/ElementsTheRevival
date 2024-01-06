using UnityEngine;

namespace Elements.Duel.Visual
{

    public class DeckDisplayer : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI deckCount;

        [SerializeField]
        private OwnerEnum owner;
        private EventBinding<DeckCountChangeEvent> _deckCountChangeBinding;
        public void OnEnable()
        {
            _deckCountChangeBinding = new EventBinding<DeckCountChangeEvent>(UpdateDeckCount);
            EventBus<DeckCountChangeEvent>.Register(_deckCountChangeBinding);
        }
        
    
        public void OnDisable() {
            EventBus<DeckCountChangeEvent>.Unregister(_deckCountChangeBinding);
        }

        public void UpdateDeckCount(DeckCountChangeEvent deckCountChangeEvent)
        {
            if (deckCountChangeEvent.Owner.Equals(owner)) return;
            deckCount.text = deckCountChangeEvent.DeckCount.ToString();
        }
    }
}