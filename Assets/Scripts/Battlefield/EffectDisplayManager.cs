using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectDisplayManager : MonoBehaviour
{
    [SerializeField]
    private GameObject poisonImage;

    [SerializeField]
    private GameObject momentumImage;

    [SerializeField]
    private GameObject psionImage;

    [SerializeField]
    private GameObject immaterialImage;

    [SerializeField]
    private GameObject gravityImage;

    [SerializeField]
    private GameObject delayedImage;

    [SerializeField]
    private GameObject frozenImage;

    [SerializeField]
    private GameObject burrowedImage;

    [SerializeField]
    private GameObject adrenalineImage;

    [SerializeField]
    private TextMeshProUGUI poisonCount;

    public void UpdateEffectDisplay(Card card)
    {
        momentumImage.SetActive(card.passiveSkills.Momentum);
        psionImage.SetActive(card.passiveSkills.Psion);
        immaterialImage.SetActive(card.innateSkills.Immaterial);
        gravityImage.SetActive(card.passiveSkills.GravityPull);
        delayedImage.SetActive(card.Counters.Delay > 0);
        frozenImage.SetActive(card.Counters.Freeze > 0);
        burrowedImage.SetActive(card.passiveSkills.Burrow);
        adrenalineImage.SetActive(card.passiveSkills.Adrenaline);
        poisonImage.SetActive(card.Counters.Poison != 0);

        if (card.Counters.Poison == 0) return;
        poisonImage.GetComponent<Image>().sprite = ImageHelper.GetPoisonSprite(card.Counters.Poison > 0);
        poisonImage.GetComponent<Image>().color = card.Counters.Aflatoxin > 0 ? new(108f, 108f, 108f) : new(byte.MaxValue, byte.MaxValue, byte.MaxValue);
        poisonCount.text = $"{Mathf.Abs(card.Counters.Poison)}";
    }
}