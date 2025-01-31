using System;
using System.Collections;
using Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OracleSpinManager : MonoBehaviour
{

    private int _electrumToAdd;

    private Vector3 _finalRotation;
    private bool _oracleSpinStarted = false;
    private int _maxRotationCount = 3;
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
    private void Start()
    {
        var oracleResult = OracleHelper.GetOracleResults();
        _cardToShow = oracleResult.Item1;
        _electrumToAdd = oracleResult.Item2;
        _finalRotation = oracleResult.Item3;
    }

    private Card _cardToShow = null;
    public void StartRotation()
    {
        StartCoroutine(OracleRotatation());
        cardDisplay.SetupCardView(_cardToShow);
    }

    public void ReturnToMenu()
    {
        SceneTransitionManager.Instance.LoadScene("Dashboard");
    }

    private void SetupResultBlock()
    {
        canKeepCard.text = "Yes";
        electrumReward.text = $"{_electrumToAdd}";
        nextFalseGod.text = OracleHelper.GetNextFalseGod();
        getCardButton.gameObject.SetActive(true);
        petName.text = OracleHelper.GetPetForNextBattle(_cardToShow.CostElement);
    }
    public async void SaveOracleResults()
    {
        PlayerData.Shared.electrum += _electrumToAdd;
        PlayerData.Shared.petName = petName.text;
        PlayerData.Shared.petCount = 3;
        PlayerData.Shared.nextFalseGod = nextFalseGod.text;
        var invent = PlayerData.Shared.GetInventory();
        invent.Add(_cardToShow.Id);
        PlayerData.Shared.SetInventory(invent);
        PlayerData.SaveData();
        await ApiManager.Instance.UpdateOraclePlayed();
        SceneTransitionManager.Instance.LoadScene("Dashboard");
    }

    private void SetupFortuneText()
    {
        cardName.text = _cardToShow.CardName;
        fortuneHead.text = ElementStrings.GetFortuneHeadString(_cardToShow.Type, _cardToShow.CostElement, _cardToShow.CardName);
        fortuneBody.text = ElementStrings.GetCardBodyString(_cardToShow.CardName);
        PlayerData.Shared.playedOracleToday = true;
        PlayerData.Shared.oracleLastPlayed = DateTime.Today.ToString("yyyy-MM-dd'T'HH:mm:ss.fffffffzzz");
        SetupResultBlock();
    }

    private IEnumerator OracleRotatation()
    {
        var rotationCount = 0;
        _oracleSpinStarted = true;

        float previousValue = 200;
        //Vector3 finalDestination = transform.localEulerAngles * 1000 
        while (rotationCount < _maxRotationCount || _cardToShow == null)
        {
            transform.Rotate(new Vector3(0, 0, -150 * Time.deltaTime));
            yield return null;
            var currentValue = transform.eulerAngles.z;
            if (currentValue > previousValue) { rotationCount++; }
            previousValue = currentValue;
        }

        while (transform.eulerAngles.z > _finalRotation.z)
        {
            transform.Rotate(new Vector3(0, 0, -30 * Time.deltaTime));
            yield return null;
        }
        transform.eulerAngles = _finalRotation;
        StartCoroutine(MaterializeAnimation());
    }

    // Update is called once per frame
    private void Update()
    {
        if (_oracleSpinStarted) { return; }
        transform.Rotate(new Vector3(0, 0, -20 * Time.deltaTime));
    }


    private IEnumerator MaterializeAnimation()
    {
        var dissolveMat = new Material(dissolveMaterial);
        dissolveMat.SetTexture("_MainTex", cardHide.sprite.texture);
        cardHide.material = dissolveMat;
        cardHide.material.SetFloat("_Fade", 1f);
        cardHide.material.SetFloat("_Scale", 25f);
        cardHide.material.SetColor("_EdgeColour", ElementColours.GetElementColour(_cardToShow.CostElement));

        float currentTime = 1;
        while (currentTime > 0f)
        {
            var value = currentTime / 1;
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
