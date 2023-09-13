using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinFlip : MonoBehaviour
{
    [SerializeField]
    private Image coinImage;
    [SerializeField]
    private TextMeshProUGUI result;
    [SerializeField]
    private GameObject parent;
    public List<Sprite> sides;
    private int flipCount = 1;
    private int flipTimes = 4;

    public bool playerStarts = false;
    private void Start()
    {
    }

    public void FlipCoinRandom()
    {
        flipTimes = Random.Range(8, 13);
        playerStarts = flipTimes % 2 == 0;
        StartCoroutine(WaitPlease(0.0001f, 1.0f));
    }

    public IEnumerator WaitPlease(float duration, float size)
    {
        flipTimes = Random.Range(8, 13);
        playerStarts = flipTimes % 2 == 0;
        while (flipTimes > 0)
        {
            while (size > 0.1)
            {
                size = size - 0.14f;
                transform.localScale = new Vector3(1, size, 1);
                yield return null;
            }
            coinImage.sprite = sides[flipCount % 2];
            while (size < 0.99)
            {
                size = size + 0.14f;
                transform.localScale = new Vector3(1, size, size);
                yield return new WaitForSeconds(duration);
            }
            flipCount++;
            flipTimes--;
        }
        result.text = playerStarts ? "You won the coin toss.\n It is your turn" : "You lost the coin toss. \n It is your opponent's turn";

        Invoke(nameof(HideCoinFlip), 2f);
    }

    private void HideCoinFlip()
    {
        parent.SetActive(false);
    }

    public void FlipCoinWithResult(int flipCounts, bool willStart)
    {

        flipTimes = flipCounts;
        playerStarts = willStart;
        StartCoroutine(WaitPlease(0.0001f, 1.0f));
    }
}