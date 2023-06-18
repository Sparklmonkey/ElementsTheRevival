using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class CardDisplayer : MonoBehaviour
{
    private List<Image> imageList = new ();
    private List<TextMeshProUGUI> textList;
    private List<string> backupText = new ();
    [SerializeField]
    private Material dissolveMaterial;
    public TMP_FontAsset underlayBlack, underlayWhite;
    [SerializeField]
    private Image validTargetGlow;
    [SerializeField]
    private Image isUsableGlow;
    private Element element;
    public bool isPassive;
    private Card card;
    private void Awake()
    {
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
        isUsableGlow.gameObject.SetActive(shouldShow);
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
        transform.parent.gameObject.SetActive(false);
        backupText = new List<string>();
    }

    public void SetRayCastTarget(bool target)
    {
    }

    public void PlayDissolveAnimation()
    {
        ClearDisplay();
        //StartCoroutine(DissolveAnimation());
    }

    public void PlayMaterializeAnimation(Element element)
    {
        //this.element = element;
        //StartCoroutine(MaterializeAnimation());
        SetRayCastTarget(true);
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
        if (!isPassive)
        {
            ClearDisplay();
        }
    }


    public abstract void DisplayCard(Card cardToDisplay, int stack = 0);
    public abstract void HideCard(Card cardToDisplay, int stack = 0);

    private enum DissolveState
    {
        Start,
        Middle,
        End
    }

}
