using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Core;
using Networking.Networking;
using UnityEngine;

namespace RewardSpinManager
{
    public enum SpinResult
    {
        None,
        ElectrumReward,
        CardReward
    }
    public class RewardSpinViewModel : INotifyPropertyChanged
    {
        private readonly RewardSpinModel _model;
        private readonly ICloudSaveManager _cloudSaveManager;
        private readonly ICloudCodeManager _cloudCodeManager;
    
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<List<Card>> OnSpinResultsReady;
        public event Action OnCardsWonUpdated;
        public event Action OnMoveToDashboard;
        

        public ICommand SpinCommand { get; }
        public ICommand SpinAllCommand { get; }

        private string _buttonText;
        public string ButtonText
        {
            get => _buttonText;
            set
            {
                _buttonText = value;
                OnPropertyChanged(nameof(ButtonText));
            }
        }

        public string SpinCountText => _model.SpinCount.ToString();
        public string ElectrumValueText => _model.ElectrumValue.ToString();
        public bool CanSpinAll => _model.SpinCount > 0;
        public bool ElementalMasteryVisible => _model.ElementalMasteryActive;

        public RewardSpinViewModel(RewardSpinModel model)
        {
            _model = model;
            _cloudCodeManager = new CloudCodeManager();
            _cloudSaveManager = new CloudSaveManager(_cloudCodeManager);
            InitializeData();
            SpinCommand = new RelayCommand(ExecuteSpin, CanExecuteSpin);
            SpinAllCommand = new RelayCommand(ExecuteSpinAll, () => CanSpinAll);
        }

        private async void InitializeData()
        {
            var bonus = GetBonusGain();
            var coinsWon = bonus;
            var newScore = await _cloudCodeManager.UpdateScore(BattleVars.Shared.EnemyAiData.scoreWin + bonus);
            SessionManager.Instance.PlayerScore = newScore;
            if (BattleVars.Shared.ElementalMastery)
            {
                coinsWon *= 2;
                EventBus<PlaySoundEffectEvent>.Raise(new PlaySoundEffectEvent("ElementalMastery"));
                _model.ElementalMasteryActive = true;
            }

            var oppDeck = new List<string>(BattleVars.Shared.EnemyAiData.deck.Split(" ")).DeserializeCard();
            if (BattleVars.Shared.IsArena)
            {
                oppDeck.Add(CardDatabase.Instance.GetShardOfElement(BattleVars.Shared.EnemyAiData.mark));
            }

            _model.ElectrumValue = coinsWon;
            _model.SpinCount = BattleVars.Shared.EnemyAiData.spins;
            
            SetupImageList(oppDeck);
            ButtonText = _model.SpinCount == 0 ? "Continue" : "Start Spin";
            
            var gameTimeInSeconds = (DateTime.Now - BattleVars.Shared.GameStartInTicks).TotalSeconds;
            _model.GameTurns = BattleVars.Shared.TurnCount;
            _model.GameTimeInSeconds = (float)gameTimeInSeconds;
            _model.PlayerScore = newScore.seasonalScore;
        }
        
        private void SetupImageList(List<Card> deck)
        {

            foreach (var card in deck)
            {
                _model.SpriteList.Add(card.cardImage);
            }

            _model.SpriteList.Shuffle();
        }
        
        private int GetBonusGain()
        {
            var enemyAi = BattleVars.Shared.EnemyAiData;
            if (enemyAi.hpDivide == 0)
            {
                return 0;
            }
            
            return enemyAi.coinAvg + Mathf.FloorToInt(BattleVars.Shared.PlayerHp / (float)enemyAi.hpDivide);
        }
        private bool CanExecuteSpin() => _model.CanSpin;

        private void ExecuteSpin()
        {
            if (ButtonText == "Continue" && _model.CanSpin)
            {
                OnMoveToDashboard?.Invoke();
                return;
            }

            if (_model.CanSpin)
            {
                _model.SpinCount--;
                _model.CanSpin = false;
            
                var spinResults = GetSpinResults();
                OnSpinResultsReady?.Invoke(spinResults);
            
                ButtonText = _model.SpinCount == 0 ? "Continue" : "Spin Again";
                OnPropertyChanged(nameof(SpinCountText));
                OnPropertyChanged(nameof(CanSpinAll));
                OnPropertyChanged(nameof(ElectrumValueText));
                OnPropertyChanged(nameof(ElementalMasteryVisible));
            }
        }

        private async void ExecuteSpinAll()
        {
            _model.CanSpin = false;
            var totalSpins = _model.SpinCount;
        
            for (var i = 0; i < totalSpins; i++)
            {
                var spinResults = GetSpinResults();
                OnSpinResultsReady?.Invoke(spinResults);
                _model.SpinCount--;
                OnPropertyChanged(nameof(SpinCountText));
                await Task.Delay(500); // Delay between spins
            }

            ButtonText = "Continue";
            _model.CanSpin = true;
            OnPropertyChanged(nameof(ButtonText));
        }

        private List<Card> GetSpinResults()
        {
            // Same logic as original GetSpinResults
            var rewardType = UnityEngine.Random.Range(0, 100);
            var rnd = new System.Random();
        
            var cardOne = _model.OpponentDeck.OrderBy(_ => rnd.Next()).First();
            var cardTwo = _model.OpponentDeck.Where(c => c.Id != cardOne.Id)
                .OrderBy(_ => rnd.Next()).First();
            var cardThree = _model.OpponentDeck.Where(c => c.Id != cardOne.Id && c.Id != cardTwo.Id)
                .OrderBy(_ => rnd.Next()).First();

            switch (rewardType)
            {
                case < 15:
                    _model.CardsWon.Add(cardOne);
                    _model.SpinResult = SpinResult.CardReward;
                    return new List<Card> { cardOne, cardOne, cardOne };
                case < 50:
                    _model.ElectrumValue += 5;
                    _model.SpinResult = SpinResult.ElectrumReward;
                    return new List<Card> { cardOne, cardTwo, cardTwo };
                default:
                    _model.SpinResult = SpinResult.None;
                    return new List<Card> { cardOne, cardTwo, cardThree };
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task AddRewardsToPlayer()
        {
            if (_model.CardsWon.Count > 0)
            {
                var inventoryCards = PlayerData.Shared.GetInventory();
                inventoryCards.AddRange(_model.CardsWon.SerializeCard());
                PlayerData.Shared.SetInventory(inventoryCards);
            }
            PlayerData.Shared.Electrum += _model.ElectrumValue;
            await _cloudSaveManager.SavePlayerData(PlayerData.Shared);
        }
    }
}