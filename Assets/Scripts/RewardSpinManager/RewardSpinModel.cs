using System.Collections.Generic;
using UnityEngine;

namespace RewardSpinManager
{
    public class RewardSpinModel
    {
        public List<Card> CardsWon { get; set; } = new();
        public List<Card> OpponentDeck { get; set; }
        public int SpinCount { get; set; }
        public int ElectrumValue { get; set; }
        public bool ElementalMasteryActive { get; set; }
        public int GameTurns { get; set; }
        public float GameTimeInSeconds { get; set; }
        public int PlayerScore { get; set; }
        public bool CanSpin { get; set; } = true;
        public List<Sprite> SpriteList { get; set; } = new();
        public SpinResult SpinResult { get; set; } = SpinResult.None;
    }
}