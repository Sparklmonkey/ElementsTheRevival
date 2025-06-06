using System.Collections.Generic;

namespace Deck_Manager.Events
{
    public struct UpdateDeckDisplayEvent : IEvent
    {
        public List<string> DeckString;

        public UpdateDeckDisplayEvent(List<string> deckString)
        {
            DeckString = deckString;
        }
    }
    public struct UpdateCurrentDeckEvent : IEvent
    {
        public List<string> DeckString;
        public int Mark;

        public UpdateCurrentDeckEvent(List<string> deckString, int mark)
        {
            DeckString = deckString;
            Mark = mark;
        }
    }
}