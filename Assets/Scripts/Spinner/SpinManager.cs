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
    private bool shouldWinCoins = false, shouldWinCard = false, canSpin = true;
    public static int finishSpinCount = 3;

    private List<Card> cardsWon = new List<Card>();
    private List<Card> oppDeck;
    private int GetBonusGain()
    {
        EnemyAi enemyAi = BattleVars.shared.enemyAiData;
        if (enemyAi.hpDivide == 0)
        {
            return 0;
        }
        else
        {
            return enemyAi.coinAvg + (Mathf.FloorToInt(BattleVars.shared.playerHP / enemyAi.hpDivide));
        }
    }

    private void Start()
    {
        int bonus = GetBonusGain();
        int coinsWon = bonus;
        PlayerData.shared.playerScore += (BattleVars.shared.enemyAiData.scoreWin + bonus);
        if (BattleVars.shared.elementalMastery)
        {
            coinsWon *= 2;
            SoundManager.Instance.PlayAudioClip("ElementalMastery");
            elementalMasteryLabel.SetActive(true);
        }

        oppDeck = new List<string>(BattleVars.shared.enemyAiData.deck.Split(" ")).DeserializeCard();
        if (BattleVars.shared.isArena)
        {
            oppDeck.Add(CardDatabase.Instance.GetShardOfElement(BattleVars.shared.enemyAiData.mark));
        }


        electrumValue.text = coinsWon.ToString();
        spinCount.text = BattleVars.shared.enemyAiData.spins.ToString();
        SetupImageList(oppDeck);
        buttonText.text = BattleVars.shared.enemyAiData.spins == 0 ? "Continue" : "Start Spin";
        spinAllButton.SetActive(BattleVars.shared.enemyAiData.spins > 0);
        double gameTimeInSeconds = TimeSpan.FromTicks(DateTime.Now.Ticks - BattleVars.shared.gameStartInTicks).TotalSeconds;
        gameTurns.text = $"{BattleVars.shared.turnCount}";
        gameTime.text = $"{(int)gameTimeInSeconds}";
        playerScore.text = $"{PlayerData.shared.playerScore}";
    }

    private void MoveToDashboard()
    {
        if (cardsWon.Count > 0)
        {
            PlayerData.shared.cardInventory.AddRange(cardsWon.SerializeCard());
        }
        PlayerData.shared.electrum += int.Parse(electrumValue.text);
        PlayerData.SaveData();
        SceneTransitionManager.Instance.LoadScene("Dashboard");
    }

    public void StartSpinning()
    {
        if (buttonText.text == "Continue" && canSpin)
        {
            MoveToDashboard();
        }
        else if (canSpin)
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
        canSpin = true;
        buttonText.transform.parent.GetComponent<Button>().interactable = true;
    }

    private List<Sprite> spriteList = new List<Sprite>();

    private void SetupImageList(List<Card> deck)
    {

        foreach (Card card in deck)
        {
            spriteList.Add(ImageHelper.GetCardImage(card.imageID));
        }

        spriteList.Shuffle();
    }

    private IEnumerator StartSpin()
    {
        finishSpinCount = 0;
        canSpin = false;

        List<Card> spinResult = GetSpinResults();
        Card cardOne = spinResult[0];
        Card cardTwo = spinResult[1];
        Card cardThree = spinResult[2];

        if (cardOne.cardName == cardThree.cardName && cardOne.cardName == cardTwo.cardName)
        {
            shouldWinCard = true;
            cardsWon.Add(CardDatabase.Instance.GetCardFromId(cardOne.iD));
        }
        else if (cardOne.cardName == cardTwo.cardName || cardOne.cardName == cardThree.cardName || cardTwo.cardName == cardThree.cardName)
        {
            shouldWinCoins = true;
        }

        List<Sprite> tempList = new List<Sprite>(spriteList) { ImageHelper.GetCardImage(cardOne.imageID) };
        spinOne.isUpgraded = cardOne.iD.IsUpgraded();
        StartCoroutine(spinOne.DissolveAnimation(tempList));
        yield return new WaitForSeconds(0.5f);
        tempList = new List<Sprite>(spriteList) { ImageHelper.GetCardImage(cardTwo.imageID) };
        spinTwo.isUpgraded = cardOne.iD.IsUpgraded();
        StartCoroutine(spinTwo.DissolveAnimation(tempList));
        yield return new WaitForSeconds(0.5f);
        tempList = new List<Sprite>(spriteList) { ImageHelper.GetCardImage(cardThree.imageID) };
        spinThree.isUpgraded = cardOne.iD.IsUpgraded();
        yield return StartCoroutine(spinThree.DissolveAnimation(tempList));

    }

    private void Update()
    {
        if (finishSpinCount == 3)
        {
            canSpin = true;
            finishSpinCount = 0;
            if (shouldWinCoins)
            {
                int newCoinCount = int.Parse(electrumValue.text);
                newCoinCount += 5;
                electrumValue.text = newCoinCount.ToString();
            }
            else if (shouldWinCard)
            {
                UpdateCardsWonSection();
            }
            shouldWinCoins = false;
            shouldWinCard = false;
        }
    }

    private void UpdateCardsWonSection()
    {
        if (cardsWon.Count == 1)
        {
            cardWonOne.SetActive(true);
            cardWonOne.GetComponent<CardDisplayDetail>().SetupCardView(cardsWon[0], true, false);
        }
        else if (cardsWon.Count == 2)
        {
            cardWonTwo.SetActive(true);
            cardWonTwo.GetComponent<CardDisplayDetail>().SetupCardView(cardsWon[1], true, false);
        }
        else if (cardsWon.Count == 3)
        {
            cardWonThree.SetActive(true);
            cardWonThree.GetComponent<CardDisplayDetail>().SetupCardView(cardsWon[2], true, false);
        }
    }


    private List<Card> GetSpinResults()
    {
        finishSpinCount = 0;
        canSpin = false;

        int rewardType = Random.Range(0, 100);
        System.Random rnd = new System.Random();
        Card cardOne = oppDeck.OrderBy(x => rnd.Next())
                      .First();
        Card cardTwo = oppDeck.Where(c => c.iD != cardOne.iD).OrderBy(x => rnd.Next())
                      .First();
        Card cardThree = oppDeck.Where(c => c.iD != cardOne.iD && c.iD != cardTwo.iD).OrderBy(x => rnd.Next())
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
