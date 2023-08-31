using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Oracle_SpinManager : MonoBehaviour
{

    private int electrumToAdd;

    private Vector3 finalRotation;
    private bool oracleSpinStarted = false;
    private int maxRotationCount = 3;
    [SerializeField]
    private Material dissolveMaterial;
    [SerializeField]
    private CardDisplay cardDisplay;
    [SerializeField]
    private Image cardHide;
    [SerializeField]
    private Button getCardButton;
    [SerializeField]
    private TextMeshProUGUI cardName, fortuneHead, fortuneBody, electrumReward, nextFalseGod, petName, canKeepCard;
    // Start is called before the first frame update
    void Start()
    {
        (Card, int, Vector3) oracleResult = OracleHelper.GetOracleResults();
        cardToShow = oracleResult.Item1;
        electrumToAdd = oracleResult.Item2;
        finalRotation = oracleResult.Item3;
    }
    Card cardToShow = null;
    public void StartRotation()
    {
        StartCoroutine(OracleRotatation());
        cardDisplay.SetupCardView(cardToShow);
    }

    public void ReturnToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Dashboard");
    }

    private void SetupResultBlock()
    {
        canKeepCard.text = "Yes";
        electrumReward.text = $"{electrumToAdd}";
        nextFalseGod.text = OracleHelper.GetNextFalseGod();
        getCardButton.gameObject.SetActive(true);
        petName.text = OracleHelper.GetPetForNextBattle(cardToShow.costElement);
    }
    public void SaveOracleResults()
    {
        PlayerData.shared.electrum += electrumToAdd;
        PlayerData.shared.petName = petName.text;
        PlayerData.shared.petCount = 3;
        PlayerData.shared.nextFalseGod = nextFalseGod.text;
        PlayerData.shared.cardInventory.Add(cardToShow.iD);
        PlayerData.SaveData();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Dashboard");
    }

    private void SetupFortuneText()
    {
        cardName.text = cardToShow.cardName;
        fortuneHead.text = ElementStrings.GetFortuneHeadString(cardToShow.cardType, cardToShow.costElement, cardToShow.cardName);
        fortuneBody.text = ElementStrings.GetCardBodyString(cardToShow.cardName);
        PlayerData.shared.playedOracleToday = true;
        SetupResultBlock();
    }

    private IEnumerator OracleRotatation()
    {
        int rotationCount = 0;
        oracleSpinStarted = true;

        float previousValue = 200;
        //Vector3 finalDestination = transform.localEulerAngles * 1000 
        while (rotationCount < maxRotationCount || cardToShow == null)
        {
            transform.Rotate(new Vector3(0, 0, -150 * Time.deltaTime));
            yield return null;
            float currentValue = transform.eulerAngles.z;
            if (currentValue > previousValue) { rotationCount++; }
            previousValue = currentValue;
        }

        while (transform.eulerAngles.z > finalRotation.z)
        {
            transform.Rotate(new Vector3(0, 0, -30 * Time.deltaTime));
            yield return null;
        }
        transform.eulerAngles = finalRotation;
        StartCoroutine(MaterializeAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        if(oracleSpinStarted) { return; }
        transform.Rotate(new Vector3(0, 0, -20 * Time.deltaTime));
    }


    private IEnumerator MaterializeAnimation()
    {
        Material dissolveMat = new Material(dissolveMaterial);
        dissolveMat.SetTexture("_MainTex", cardHide.sprite.texture);
        cardHide.material = dissolveMat;
        cardHide.material.SetFloat("_Fade", 1f);
        cardHide.material.SetFloat("_Scale", 25f);
        cardHide.material.SetColor("_EdgeColour", ElementColours.GetElementColour(cardToShow.costElement));

        float currentTime = 1;
        while (currentTime > 0f)
        {
            float value = currentTime / 1;
            currentTime -= Time.deltaTime;
            cardHide.material.SetFloat("_Fade", value);
            yield return null;
        }
        cardHide.material.SetFloat("_Fade", 0);
        cardHide.material = null;
        cardHide.gameObject.SetActive(false);
        SetupFortuneText();
    }
}
