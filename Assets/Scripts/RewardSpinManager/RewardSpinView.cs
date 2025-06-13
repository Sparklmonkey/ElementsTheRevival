using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace RewardSpinManager
{
    public class RewardSpinView : MonoBehaviour
    {
        [SerializeField] private CardSpinAnimation spinOne, spinTwo, spinThree;
        [SerializeField] private CardDisplayDetail cardWonOne, cardWonTwo, cardWonThree;
        [SerializeField] private GameObject spinAllButton;
        [SerializeField] private GameObject elementalMasteryLabel;
        [SerializeField] private TextMeshProUGUI spinCount, buttonText, electrumValue, gameTime, gameTurns, playerScore;

        private RewardSpinViewModel _viewModel;
        private RewardSpinModel _model;

        private async void Start()
        {
            InitializeModel();
            InitializeViewModel();
            SubscribeToEvents();
        }

        private void InitializeModel()
        {
            _model = new RewardSpinModel
            {
                SpinCount = BattleVars.Shared.EnemyAiData.spins,
                ElementalMasteryActive = BattleVars.Shared.ElementalMastery,
                OpponentDeck = new List<string>(BattleVars.Shared.EnemyAiData.deck.Split(" ")).DeserializeCard()
            };
        
            if (BattleVars.Shared.IsArena)
            {
                _model.OpponentDeck.Add(CardDatabase.Instance.GetShardOfElement(BattleVars.Shared.EnemyAiData.mark));
            }
        }

        private void InitializeViewModel()
        {
            _viewModel = new RewardSpinViewModel(_model);
        }

        private void SubscribeToEvents()
        {
            _viewModel.OnSpinResultsReady += HandleSpinResults;
            _viewModel.OnCardsWonUpdated += UpdateCardsWonSection;
            _viewModel.OnMoveToDashboard += MoveToDashboard;
            _viewModel.PropertyChanged += HandlePropertyChanged;
        }


        private async void MoveToDashboard()
        {
            await _viewModel.AddRewardsToPlayer();
            SceneTransitionManager.Instance.LoadScene("Dashboard");
        }
        
        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(RewardSpinViewModel.ButtonText):
                    buttonText.text = _viewModel.ButtonText;
                    break;
                case nameof(RewardSpinViewModel.SpinCountText):
                    spinCount.text = _viewModel.SpinCountText;
                    break;
                case nameof(RewardSpinViewModel.CanSpinAll):
                    spinAllButton.SetActive(_viewModel.CanSpinAll);
                    break; 
                case nameof(RewardSpinModel.ElectrumValue):
                    electrumValue.text = _model.ElectrumValue.ToString();
                    break;
                case nameof(RewardSpinModel.ElementalMasteryActive):
                    elementalMasteryLabel.SetActive(_model.ElementalMasteryActive);
                    break;
                case nameof(RewardSpinModel.GameTurns):
                    break;
            }
        }

        private void OnDestroy()
        {
            _viewModel.OnSpinResultsReady -= HandleSpinResults;
            _viewModel.OnCardsWonUpdated -= UpdateCardsWonSection;
            _viewModel.OnMoveToDashboard -= MoveToDashboard;
            _viewModel.PropertyChanged -= HandlePropertyChanged;
        }
        
        private void HandleSpinResults(List<Card> spinResult)
        {
            StartCoroutine(StartSpin(spinResult));
        }

        private IEnumerator StartSpin(List<Card> spinResult)
        {
            
            var tempList = new List<Sprite>(_model.SpriteList) { spinResult[0].cardImage };
            spinOne.isUpgraded = spinResult[0].Id.IsUpgraded();
            StartCoroutine(spinOne.DissolveAnimation(tempList));
            yield return new WaitForSeconds(0.5f);
            tempList = new List<Sprite>(_model.SpriteList) { spinResult[1].cardImage };
            spinTwo.isUpgraded = spinResult[1].Id.IsUpgraded();
            StartCoroutine(spinTwo.DissolveAnimation(tempList));
            yield return new WaitForSeconds(0.5f);
            tempList = new List<Sprite>(_model.SpriteList) { spinResult[2].cardImage };
            spinThree.isUpgraded = spinResult[2].Id.IsUpgraded();
            yield return StartCoroutine(spinThree.DissolveAnimation(tempList));

            switch (_model.SpinResult)
            {
                case SpinResult.None:
                    break;
                case SpinResult.CardReward:
                    UpdateCardsWonSection();
                    break;
                case SpinResult.ElectrumReward:
                    _model.ElectrumValue += 10;
                    electrumValue.text = _model.ElectrumValue.ToString();
                    break;
            }
        }
        
        private void UpdateCardsWonSection()
        {
            switch (_model.CardsWon.Count)
            {
                case 1:
                    cardWonOne.gameObject.SetActive(true);
                    cardWonOne.SetupCardView(_model.CardsWon[0], true, false);
                    break;
                case 2:
                    cardWonTwo.gameObject.SetActive(true);
                    cardWonTwo.SetupCardView(_model.CardsWon[1], true, false);
                    break;
                case 3:
                    cardWonThree.gameObject.SetActive(true);
                    cardWonThree.SetupCardView(_model.CardsWon[2], true, false);
                    break;
            }
        }
    }
}