using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EffectDisplayManager : MonoBehaviour
{
    [FormerlySerializedAs("_poisonImage")] [SerializeField]
    private GameObject poisonImage;

    [FormerlySerializedAs("_momentumImage")] [SerializeField]
    private GameObject momentumImage;

    [FormerlySerializedAs("_psionImage")] [SerializeField]
    private GameObject psionImage;

    [FormerlySerializedAs("_immaterialImage")] [SerializeField]
    private GameObject immaterialImage;

    [FormerlySerializedAs("_gravityImage")] [SerializeField]
    private GameObject gravityImage;

    [FormerlySerializedAs("_delayedImage")] [SerializeField]
    private GameObject delayedImage;

    [FormerlySerializedAs("_frozenImage")] [SerializeField]
    private GameObject frozenImage;

    [FormerlySerializedAs("_burrowedImage")] [SerializeField]
    private GameObject burrowedImage;

    [FormerlySerializedAs("_adrenalineImage")] [SerializeField]
    private GameObject adrenalineImage;

    [FormerlySerializedAs("_poisonCount")] [SerializeField]
    private TextMeshProUGUI poisonCount;

    public void UpdateEffectDisplay(Card card, int stack, bool isHidden = true)
    {
        momentumImage.SetActive(card.passiveSkills.Momentum);
        psionImage.SetActive(card.passiveSkills.Psion);
        immaterialImage.SetActive(card.innateSkills.Immaterial);
        gravityImage.SetActive(card.passiveSkills.GravityPull);
        delayedImage.SetActive(card.innateSkills.Delay > 0);
        frozenImage.SetActive(card.Freeze > 0);
        burrowedImage.SetActive(card.passiveSkills.Burrow);
        adrenalineImage.SetActive(card.passiveSkills.Adrenaline);
        poisonImage.SetActive(card.Poison != 0);

        if (card.Poison != 0)
        {
            poisonImage.GetComponent<Image>().sprite = ImageHelper.GetPoisonSprite(card.Poison > 0);
            poisonImage.GetComponent<Image>().color = card.IsAflatoxin ? new(108f, 108f, 108f) : new(byte.MaxValue, byte.MaxValue, byte.MaxValue);
            poisonCount.text = $"{Mathf.Abs(card.Poison)}";
        }
    }
}