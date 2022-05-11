using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SpinManager : MonoBehaviour
{
    [SerializeField]
    private CardSpinAnimation spinOne, spinTwo, spinThree;
    [SerializeField]
    private GameObject cardWonOne, cardWonTwo, cardWonThree;
    [SerializeField]
    private GameObject elementalMasteryLabel;

    [SerializeField]
    private TextMeshProUGUI spinCount, buttonText, electrumValue, gameTime, gameTurns, playerScore;
    private bool shouldWinCoins = false, shouldWinCard = false, canSpin = true;
    public static int finishSpinCount = 3;

    private List<Card> cardsWon = new List<Card>();


    private void Start()
    {
        int coinsWon = BattleVars.shared.enemyAiData.coinAvg > 1 ?
            Random.Range(BattleVars.shared.enemyAiData.coinAvg - 1, BattleVars.shared.enemyAiData.coinAvg + 3) : 1;
        if(BattleVars.shared.elementalMastery) 
        { 
            coinsWon *= 2;
            Game_SoundManager.PlayAudioClip("ElementalMastery");
            elementalMasteryLabel.SetActive(true); 
        }

        electrumValue.text = coinsWon.ToString();
        spinCount.text = BattleVars.shared.enemyAiData.spins.ToString();
        SetupImageList(BattleVars.shared.enemyAiData.deck);
        buttonText.text = BattleVars.shared.enemyAiData.spins == 0 ? "Continue" : "Start Spin";
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
        SceneManager.LoadScene("Dashboard");
    }

    public void StartSpinning()
    {
        if (buttonText.text == "Continue" && canSpin)
        {
            MoveToDashboard();
        }
        else if(canSpin)
        {
            int count = int.Parse(spinCount.text);
            count--;
            spinCount.text = $"{count}";
            StartSpin();
            buttonText.text = count == 0 ? "Continue" : "Spin Again";
        }
    }

    private List<Sprite> spriteList = new List<Sprite>();

    private void SetupImageList(List<Card> deck)
    {
        List<string> imageIdList = new List<string>();

        foreach (Card card in deck)
        {
            spriteList.Add(card.cardImage);
        }

        spriteList.Shuffle();
    }

    private async void StartSpin()
    {
        finishSpinCount = 0;
        canSpin = false;
        Card cardOne = BattleVars.shared.enemyAiData.deck[Random.Range(0, BattleVars.shared.enemyAiData.deck.Count)];
        Card cardTwo = BattleVars.shared.enemyAiData.deck[Random.Range(0, BattleVars.shared.enemyAiData.deck.Count)];
        Card cardThree = BattleVars.shared.enemyAiData.deck[Random.Range(0, BattleVars.shared.enemyAiData.deck.Count)];

        if(cardOne.name == cardThree.name && cardOne.name == cardTwo.name)
        {
            shouldWinCard = true;
            cardsWon.Add(CardDatabase.GetCardFromResources(cardOne.name, cardOne.type.FastCardTypeString(), !cardOne.isUpgradable));
        }
        else if(cardOne.name == cardTwo.name || cardOne.name == cardThree.name || cardTwo.name == cardThree.name)
        {
            shouldWinCoins = true;
        }

        List<Sprite> tempList = new List<Sprite>(spriteList) { ImageHelper.GetCardImage(cardOne.imageID) };
        spinOne.SetupSpinner(tempList);
        await new WaitForSeconds(0.33f);
        tempList = new List<Sprite>(spriteList) { ImageHelper.GetCardImage(cardTwo.imageID) };
        spinTwo.SetupSpinner(tempList);
        await new WaitForSeconds(0.66f);
        tempList = new List<Sprite>(spriteList) { ImageHelper.GetCardImage(cardThree.imageID) };
        spinThree.SetupSpinner(tempList);

    }

    private void Update()
    {
        if(finishSpinCount == 3)
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
        if(cardsWon.Count == 1)
        {
            cardWonOne.SetActive(true);
            cardWonOne.GetComponent<CardDisplayDetail>().SetupCardView(cardsWon[0], true, false);
        }
        else if(cardsWon.Count == 2)
        {
            cardWonTwo.SetActive(true);
            cardWonTwo.GetComponent<CardDisplayDetail>().SetupCardView(cardsWon[1], true, false);
        }
        else if(cardsWon.Count == 3)
        {
            cardWonThree.SetActive(true);
            cardWonThree.GetComponent<CardDisplayDetail>().SetupCardView(cardsWon[2], true, false);
        }
    }
}
