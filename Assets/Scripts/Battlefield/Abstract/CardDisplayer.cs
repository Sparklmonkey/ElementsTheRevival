using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class CardDisplayer : Identifiable
{
    private List<Image> imageList = new List<Image>();
    private List<TextMeshProUGUI> textList;
    private List<string> backupText = new List<string>();
    private Image parentImageRayCast;
    [SerializeField]
    private Material dissolveMaterial;
    [SerializeField]
    private Image validTargetGlow;
    [SerializeField]
    private Image isUsableGlow;
    private Element element;

    private Card card;
    private void Awake()
    {
        parentImageRayCast = transform.parent.GetComponent<Image>();
        parentImageRayCast.raycastTarget = true;
        textList = new List<TextMeshProUGUI>(GetComponentsInChildren<TextMeshProUGUI>());
        List<Image> dirtyImageList = new List<Image>(GetComponentsInChildren<Image>());
        foreach (Image item in dirtyImageList)
        {
            if (item.sprite != null)
            {
                imageList.Add(item);
            }
        }
        ShouldShowTarget(false);
    }

    public void SetCard(Card card)
    {
        this.card = card;
    }

    public Card GetCardOnDisplay()
    {
        return card;
    }
    public void ShouldShowTarget(bool shouldShow)
    {
        if (gameObject.name.Contains("Hand"))
        {
            validTargetGlow.color = shouldShow ? new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue) : new Color(0, 0, 0, 0);
        }
        else
        {
            validTargetGlow.color = shouldShow ? new Color(15, 255, 0, 255) : new Color(0, 0, 0, 0);
        }
    }
    public void ShouldShowUsableGlow(bool shouldShow)
    {
        isUsableGlow.color = shouldShow ? new Color(197, 212, 11, 255) : new Color(0, 0, 0, 0);
    }

    private void HideText()
    {
        foreach (TextMeshProUGUI text in textList)
        {
            text.text = "";
        }
    }

    public void ClearDisplay()
    {
        SetRayCastTarget(false);
        parentImageRayCast.gameObject.SetActive(false);
        backupText = new List<string>();
    }

    public void SetRayCastTarget(bool target)
    {
        parentImageRayCast.raycastTarget = target;
    }

    public void PlayDissolveAnimation()
    {
        StartCoroutine(DissolveAnimation());
    }

    public void PlayMaterializeAnimation(Element element)
    {
        this.element = element;
        StartCoroutine(MaterializeAnimation());
    }

    private void BackUpText()
    {
        foreach (TextMeshProUGUI tmText in textList)
        {
            backupText.Add(tmText.text);
        }
    }

    private void ResetText()
    {
        if(textList.Count > 0 && backupText.Count > 0)
        {
            if(textList.Count == backupText.Count)
            {
                for (int i = 0; i < backupText.Count; i++)
                {
                    textList[i].text = backupText[i];
                }
            }
        }
        backupText.Clear();
    }

    private IEnumerator ActionAnimation()
    {
        SetRayCastTarget(false);
        BackUpText();
        HideText();

        SetDissolveMatState(DissolveState.Start, 1f);
        float animSpeed = PlayerPrefs.GetFloat("AnimSpeed");

        float currentTime = animSpeed / 2f;
        if(animSpeed != 0)
        {
            while (currentTime > 0f)
            {
                float value = currentTime / (animSpeed / 2f);
                currentTime -= Time.deltaTime;
                SetDissolveMatState(DissolveState.Middle, value);
                yield return null;
            }
            SetDissolveMatState(DissolveState.End, 0f);


            SetDissolveMatState(DissolveState.Start, 0f);

            currentTime = 0f;
            while (currentTime < 0.5f)
            {
                float value = currentTime / 0.5f;
                currentTime += Time.deltaTime;
                SetDissolveMatState(DissolveState.Middle, value);
                yield return null;
            }
            SetDissolveMatState(DissolveState.End, 1f);
            ResetText();
        }
        SetRayCastTarget(true);
    }

    public void PlayActionAnim()
    {
        StartCoroutine(ActionAnimation());
    }

    private void SetDissolveMatState(DissolveState state, float valueToSet)
    {
        switch (state)
        {
            case DissolveState.Start:
                BattleVars.shared.isAnimationPlaying = true;
                foreach (TextMeshProUGUI text in textList)
                {
                    Material dissolveMat = new Material(dissolveMaterial);
                    dissolveMat.SetTexture("_MainTex", text.mainTexture);
                    text.material = dissolveMat;
                    text.material.SetFloat("_Fade", valueToSet);
                    text.material.SetFloat("_Scale", 100f);
                }

                foreach (Image image in imageList)
                {
                    Material dissolveMat = new Material(dissolveMaterial);
                    dissolveMat.SetTexture("_MainTex", image.sprite.texture);
                    image.material = dissolveMat;
                    image.material.SetFloat("_Fade", valueToSet);
                    image.material.SetFloat("_Scale", 100f);
                    image.material.SetColor("_EdgeColour", ElementColours.GetElementColour(element));
                }
                return;
            case DissolveState.Middle:
                foreach (TextMeshProUGUI text in textList)
                {
                    text.material.SetFloat("_Fade", valueToSet);
                }

                foreach (Image image in imageList)
                {
                    image.material.SetFloat("_Fade", valueToSet);
                }
                return;
            case DissolveState.End:
                foreach (Image image in imageList)
                {
                    image.material.SetFloat("_Fade", valueToSet);
                    image.material = null;
                }
                foreach (TextMeshProUGUI text in textList)
                {
                    text.material.SetFloat("_Fade", valueToSet);
                    text.material = null;
                }
                BattleVars.shared.isAnimationPlaying = false;
                return;
            default:
                break;
        }
    }

    private IEnumerator MaterializeAnimation()
    {
        BackUpText();
        HideText();

        SetDissolveMatState(DissolveState.Start, 0f);

        float currentTime = 0f;
        float animSpeed = PlayerPrefs.GetFloat("AnimSpeed");
        if(animSpeed != 0)
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
        ResetText();
        SetRayCastTarget(true);
    }


    private IEnumerator DissolveAnimation()
    {
        SetRayCastTarget(false);
        HideText();

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

    private enum DissolveState
    {
        Start,
        Middle,
        End
    }

}
