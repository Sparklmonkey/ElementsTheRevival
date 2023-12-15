using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SpinManager : MonoBehaviour
{
    [SerializeField]
    private CardSpinAnimation spinOne, spinTwo, spinThree;
    [SerializeField]
    private GameObject cardWonOne, cardWonTwo, cardWonThree, spinAllButton;
    [SerializeField]
    private GameObject elementalMasteryLabel;

    [SerializeField]
    private TextMeshProUGUI spinCount, buttonText, electrumValue, gameTime, gameTurns, playerScore;
    private bool _shouldWinCoins = false, _shouldWinCard = false, _canSpin = true;
    public static int FinishSpinCount = 3;

    private List<Card> _cardsWon = new();
    private List<Card> _oppDeck;
    private int GetBonusGain()
    {
        var enemyAi = BattleVars.Shared.EnemyAiData;
        if (enemyAi.hpDivide == 0)
        {
            return 0;
        }
        else
        {
            return enemyAi.coinAvg + Mathf.FloorToInt(BattleVars.Shared.PlayerHp / enemyAi.hpDivide);
        }
    }

    private void Start()
    {
        var bonus = GetBonusGain();
        var coinsWon = bonus;
        PlayerData.Shared.playerScore += BattleVars.Shared.EnemyAiData.scoreWin + bonus;
        if (BattleVars.Shared.ElementalMastery)
        {
            coinsWon *= 2;
            EventBus<PlaySoundEffectEvent>.Raise(new PlaySoundEffectEvent("ElementalMastery"));
            elementalMasteryLabel.SetActive(true);
        }

        _oppDeck = new List<string>(BattleVars.Shared.EnemyAiData.deck.Split(" ")).DeserializeCard();
        if (BattleVars.Shared.IsArena)
        {
            _oppDeck.Add(CardDatabase.Instance.GetShardOfElement(BattleVars.Shared.EnemyAiData.mark));
        }


        electrumValue.text = coinsWon.ToString();
        spinCount.text = BattleVars.Shared.EnemyAiData.spins.ToString();
        SetupImageList(_oppDeck);
        buttonText.text = BattleVars.Shared.EnemyAiData.spins == 0 ? "Continue" : "Start Spin";
        spinAllButton.SetActive(BattleVars.Shared.EnemyAiData.spins > 0);
        var gameTimeInSeconds = TimeSpan.FromTicks(DateTime.Now.Ticks - BattleVars.Shared.GameStartInTicks).TotalSeconds;
        gameTurns.text = $"{BattleVars.Shared.TurnCount}";
        gameTime.text = $"{(int)gameTimeInSeconds}";
        playerScore.text = $"{PlayerData.Shared.playerScore}";
    }

    private void MoveToDashboard()
    {
        if (_cardsWon.Count > 0)
        {
            PlayerData.Shared.inventoryCards.AddRange(_cardsWon.SerializeCard());
        }
        PlayerData.Shared.electrum += int.Parse(electrumValue.text);
        PlayerData.SaveData();
        SceneTransitionManager.Instance.LoadScene("Dashboard");
    }

    public void StartSpinning()
    {
        if (buttonText.text == "Continue" && _canSpin)
        {
            MoveToDashboard();
        }
        else if (_canSpin)
        {
            var count = int.Parse(spinCount.text);
            count--;
            spinCount.text = $"{count}";
            StartCoroutine(StartSpin());
            buttonText.text = count == 0 ? "Continue" : "Spin Again";
            spinAllButton.SetActive(count > 0);
        }
    }

    public void SpinAll()
    {
        StartCoroutine(DoAllSpins());
    }

    private IEnumerator DoAllSpins()
    {
        var count = int.Parse(spinCount.text);
        for (var i = 0; i < count; i++)
        {
            yield return StartCoroutine(StartSpin());
            spinCount.text = $"{count - (i + 1)}";
            yield return new WaitForSeconds(0.5f);

        }
        buttonText.text = "Continue";
        _canSpin = true;
        buttonText.transform.parent.GetComponent<Button>().interactable = true;
    }

    private List<Sprite> _spriteList = new();

    private void SetupImageList(List<Card> deck)
    {

        foreach (var card in deck)
        {
            _spriteList.Add(ImageHelper.GetCardImage(card.imageID));
        }

        _spriteList.Shuffle();
    }

    private IEnumerator StartSpin()
    {
        FinishSpinCount = 0;
        _canSpin = false;

        var spinResult = GetSpinResults();
        var cardOne = spinResult[0];
        var cardTwo = spinResult[1];
        var cardThree = spinResult[2];

        if (cardOne.cardName == cardThree.cardName && cardOne.cardName == cardTwo.cardName)
        {
            _shouldWinCard = true;
            _cardsWon.Add(CardDatabase.Instance.GetCardFromId(cardOne.iD));
        }
        else if (cardOne.cardName == cardTwo.cardName || cardOne.cardName == cardThree.cardName || cardTwo.cardName == cardThree.cardName)
        {
            _shouldWinCoins = true;
        }

        var tempList = new List<Sprite>(_spriteList) { ImageHelper.GetCardImage(cardOne.imageID) };
        spinOne.isUpgraded = cardOne.iD.IsUpgraded();
        StartCoroutine(spinOne.DissolveAnimation(tempList));
        yield return new WaitForSeconds(0.5f);
        tempList = new List<Sprite>(_spriteList) { ImageHelper.GetCardImage(cardTwo.imageID) };
        spinTwo.isUpgraded = cardOne.iD.IsUpgraded();
        StartCoroutine(spinTwo.DissolveAnimation(tempList));
        yield return new WaitForSeconds(0.5f);
        tempList = new List<Sprite>(_spriteList) { ImageHelper.GetCardImage(cardThree.imageID) };
        spinThree.isUpgraded = cardOne.iD.IsUpgraded();
        yield return StartCoroutine(spinThree.DissolveAnimation(tempList));

    }

    private void Update()
    {
        if (FinishSpinCount == 3)
        {
            _canSpin = true;
            FinishSpinCount = 0;
            if (_shouldWinCoins)
            {
                var newCoinCount = int.Parse(electrumValue.text);
                newCoinCount += 5;
                electrumValue.text = newCoinCount.ToString();
            }
            else if (_shouldWinCard)
            {
                UpdateCardsWonSection();
            }
            _shouldWinCoins = false;
            _shouldWinCard = false;
        }
    }

    private void UpdateCardsWonSection()
    {
        if (_cardsWon.Count == 1)
        {
            cardWonOne.SetActive(true);
            cardWonOne.GetComponent<CardDisplayDetail>().SetupCardView(_cardsWon[0], true, false);
        }
        else if (_cardsWon.Count == 2)
        {
            cardWonTwo.SetActive(true);
            cardWonTwo.GetComponent<CardDisplayDetail>().SetupCardView(_cardsWon[1], true, false);
        }
        else if (_cardsWon.Count == 3)
        {
            cardWonThree.SetActive(true);
            cardWonThree.GetComponent<CardDisplayDetail>().SetupCardView(_cardsWon[2], true, false);
        }
    }


    private List<Card> GetSpinResults()
    {
        FinishSpinCount = 0;
        _canSpin = false;

        var rewardType = Random.Range(0, 100);
        var rnd = new System.Random();
        var cardOne = _oppDeck.OrderBy(_ => rnd.Next())
                      .First();
        var cardTwo = _oppDeck.Where(c => c.iD != cardOne.iD).OrderBy(_ => rnd.Next())
                      .First();
        var cardThree = _oppDeck.Where(c => c.iD != cardOne.iD && c.iD != cardTwo.iD).OrderBy(_ => rnd.Next())
                      .First();
        //return new List<Card> { cardOne, cardOne, cardOne };
        if (rewardType < 15)
        {
            //Win Card
            return new List<Card> { cardOne, cardOne, cardOne };
        }
        else if (rewardType < 50)
        {
            //Win Extra Coins
            return new List<Card> { cardOne, cardTwo, cardTwo };
        }
        else
        {
            //Nothing
            return new List<Card> { cardOne, cardTwo, cardThree };
        }
    }
}
