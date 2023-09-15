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

    private List<Card> _cardsWon = new List<Card>();
    private List<Card> _oppDeck;
    private int GetBonusGain()
    {
        EnemyAi enemyAi = BattleVars.Shared.EnemyAiData;
        if (enemyAi.hpDivide == 0)
        {
            return 0;
        }
        else
        {
            return enemyAi.coinAvg + (Mathf.FloorToInt(BattleVars.Shared.PlayerHp / enemyAi.hpDivide));
        }
    }

    private void Start()
    {
        int bonus = GetBonusGain();
        int coinsWon = bonus;
        PlayerData.Shared.playerScore += (BattleVars.Shared.EnemyAiData.scoreWin + bonus);
        if (BattleVars.Shared.ElementalMastery)
        {
            coinsWon *= 2;
            SoundManager.Instance.PlayAudioClip("ElementalMastery");
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
        double gameTimeInSeconds = TimeSpan.FromTicks(DateTime.Now.Ticks - BattleVars.Shared.GameStartInTicks).TotalSeconds;
        gameTurns.text = $"{BattleVars.Shared.TurnCount}";
        gameTime.text = $"{(int)gameTimeInSeconds}";
        playerScore.text = $"{PlayerData.Shared.playerScore}";
    }

    private void MoveToDashboard()
    {
        if (_cardsWon.Count > 0)
        {
            PlayerData.Shared.cardInventory.AddRange(_cardsWon.SerializeCard());
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
            int count = int.Parse(spinCount.text);
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
        int count = int.Parse(spinCount.text);
        for (int i = 0; i < count; i++)
        {
            yield return StartCoroutine(StartSpin());
            spinCount.text = $"{count - (i + 1)}";
            yield return new WaitForSeconds(0.5f);

        }
        buttonText.text = "Continue";
        _canSpin = true;
        buttonText.transform.parent.GetComponent<Button>().interactable = true;
    }

    private List<Sprite> _spriteList = new List<Sprite>();

    private void SetupImageList(List<Card> deck)
    {

        foreach (Card card in deck)
        {
            _spriteList.Add(ImageHelper.GetCardImage(card.imageID));
        }

        _spriteList.Shuffle();
    }

    private IEnumerator StartSpin()
    {
        FinishSpinCount = 0;
        _canSpin = false;

        List<Card> spinResult = GetSpinResults();
        Card cardOne = spinResult[0];
        Card cardTwo = spinResult[1];
        Card cardThree = spinResult[2];

        if (cardOne.cardName == cardThree.cardName && cardOne.cardName == cardTwo.cardName)
        {
            _shouldWinCard = true;
            _cardsWon.Add(CardDatabase.Instance.GetCardFromId(cardOne.iD));
        }
        else if (cardOne.cardName == cardTwo.cardName || cardOne.cardName == cardThree.cardName || cardTwo.cardName == cardThree.cardName)
        {
            _shouldWinCoins = true;
        }

        List<Sprite> tempList = new List<Sprite>(_spriteList) { ImageHelper.GetCardImage(cardOne.imageID) };
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
                int newCoinCount = int.Parse(electrumValue.text);
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

        int rewardType = Random.Range(0, 100);
        System.Random rnd = new System.Random();
        Card cardOne = _oppDeck.OrderBy(x => rnd.Next())
                      .First();
        Card cardTwo = _oppDeck.Where(c => c.iD != cardOne.iD).OrderBy(x => rnd.Next())
                      .First();
        Card cardThree = _oppDeck.Where(c => c.iD != cardOne.iD && c.iD != cardTwo.iD).OrderBy(x => rnd.Next())
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
