using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class CardDisplayer : MonoBehaviour
{
    private List<Image> _imageList;
    private List<TextMeshProUGUI> _textList;
    [SerializeField]
    private Material dissolveMaterial;
    public TMP_FontAsset underlayBlack, underlayWhite;
    [SerializeField]
    private Image validTargetGlow;
    [SerializeField]
    private Image isUsableGlow;
    private Element _element;
    private void Awake()
    {
        _imageList = new();
        _textList = new(GetComponentsInChildren<TextMeshProUGUI>());
        List<Image> dirtyImageList = new(GetComponentsInChildren<Image>());
        foreach (Image item in dirtyImageList)
        {
            if (item.sprite != null)
            {
                _imageList.Add(item);
            }
        }
        ShouldShowTarget(false);
    }

    public void ShouldShowTarget(bool shouldShow)
    {
        validTargetGlow.color = shouldShow ? new Color(15, 255, 0, 255) : new Color(0, 0, 0, 0);
    }

    public void ShouldShowUsableGlow(bool shouldShow)
    {
        if (gameObject.name.Contains("Hand"))
        {
            validTargetGlow.color = shouldShow ? new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue) : new Color(0, 0, 0, 0);
        }
        else
        {
            isUsableGlow.gameObject.SetActive(shouldShow);
        }
    }

    private void ShouldShowText(bool shouldShow)
    {
        foreach (TextMeshProUGUI text in _textList)
        {
            text.gameObject.SetActive(shouldShow);
        }
    }

    public void ClearDisplay()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public void PlayDissolveAnimation()
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(DissolveAnimation());
        }
    }

    public void PlayMaterializeAnimation(Element element)
    {
        this._element = element;
        StartCoroutine(MaterializeAnimation());
    }

    private void SetDissolveMatState(DissolveState state, float valueToSet)
    {
        switch (state)
        {
            case DissolveState.Start:
                BattleVars.Shared.IsAnimationPlaying = true;
                foreach (Image image in _imageList)
                {
                    Material dissolveMat = new(dissolveMaterial);
                    dissolveMat.SetTexture("_MainTex", image.sprite.texture);
                    image.material = dissolveMat;
                    image.material.SetFloat("_Fade", valueToSet);
                    image.material.SetFloat("_Scale", 100f);
                    image.material.SetColor("_EdgeColour", ElementColours.GetElementColour(_element));
                }
                return;
            case DissolveState.Middle:
                foreach (Image image in _imageList)
                {
                    image.material.SetFloat("_Fade", valueToSet);
                }
                return;
            case DissolveState.End:
                foreach (Image image in _imageList)
                {
                    image.material.SetFloat("_Fade", valueToSet);
                    image.material = null;
                }
                BattleVars.Shared.IsAnimationPlaying = false;
                return;
            default:
                break;
        }
    }

    private IEnumerator MaterializeAnimation()
    {
        ShouldShowText(false);

        SetDissolveMatState(DissolveState.Start, 0f);

        float currentTime = 0f;
        float animSpeed = PlayerPrefs.GetFloat("AnimSpeed");
        if (animSpeed != 0)
        {
            while (currentTime < animSpeed)
            {
                float value = currentTime / animSpeed;
                currentTime += Time.deltaTime;
                SetDissolveMatState(DissolveState.Middle, value);
                yield return null;
            }
        }
        SetDissolveMatState(DissolveState.End, 1f);
        ShouldShowText(true);
    }


    private IEnumerator DissolveAnimation()
    {
        ShouldShowText(false);

        SetDissolveMatState(DissolveState.Start, 1f);

        float animSpeed = PlayerPrefs.GetFloat("AnimSpeed");
        float currentTime = animSpeed;

        if (animSpeed != 0)
        {
            while (currentTime > 0f)
            {
                float value = currentTime / animSpeed;
                currentTime -= Time.deltaTime;
                SetDissolveMatState(DissolveState.Middle, value);

                yield return null;
            }
        }
        SetDissolveMatState(DissolveState.End, 0f);
        ClearDisplay();
    }


    public abstract void DisplayCard(Card cardToDisplay, int stack = 0, bool isHidden = true);
    public abstract void HideCard(Card cardToDisplay, int stack = 0);

    private enum DissolveState
    {
        Start,
        Middle,
        End
    }

}
