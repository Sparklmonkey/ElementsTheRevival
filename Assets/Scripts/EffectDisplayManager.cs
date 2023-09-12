using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectDisplayManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _poisonImage, _momentumImage, _psionImage, _immaterialImage, _gravityImage, _delayedImage, _frozenImage, _burrowedImage, _adrenalineImage;
    [SerializeField]
    private TextMeshProUGUI _poisonCount;

    public void UpdateEffectDisplay(Card card, int stack, bool isHidden = true)
    {
        _momentumImage.SetActive(card.passiveSkills.Momentum);
        _psionImage.SetActive(card.passiveSkills.Psion);
        _immaterialImage.SetActive(card.innateSkills.Immaterial);
        _gravityImage.SetActive(card.passiveSkills.GravityPull);
        _delayedImage.SetActive(card.innateSkills.Delay > 0);
        _frozenImage.SetActive(card.Freeze > 0);
        _burrowedImage.SetActive(card.passiveSkills.Burrow);
        _adrenalineImage.SetActive(card.passiveSkills.Adrenaline);
        _poisonImage.SetActive(card.Poison != 0);

        if (card.Poison != 0)
        {
            _poisonImage.GetComponent<Image>().sprite = ImageHelper.GetPoisonSprite(card.Poison > 0);
            _poisonImage.GetComponent<Image>().color = card.IsAflatoxin ? new(108f, 108f, 108f) : new(byte.MaxValue, byte.MaxValue, byte.MaxValue);
            _poisonCount.text = $"{Mathf.Abs(card.Poison)}";
        }
    }
}