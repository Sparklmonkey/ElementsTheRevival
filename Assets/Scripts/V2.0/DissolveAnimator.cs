using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DissolveAnimator : MonoBehaviour
{
    public Material dissolveMaterial;
    private void SetDissolveMatState(DissolveState state, float valueToSet, List<Image> imageList)
    {
        switch (state)
        {
            case DissolveState.Start:
                BattleVars.shared.isAnimationPlaying = true;
                foreach (Image image in imageList)
                {
                    Material dissolveMat = new Material(dissolveMaterial);
                    dissolveMat.SetTexture("_MainTex", image.sprite.texture);
                    image.material = dissolveMat;
                    image.material.SetFloat("_Fade", valueToSet);
                    image.material.SetFloat("_Scale", 100f);
                    image.material.SetColor("_EdgeColour", ElementColours.GetElementColour(Element.Aether));
                }
                return;
            case DissolveState.Middle:
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
                BattleVars.shared.isAnimationPlaying = false;
                return;
            default:
                break;
        }
    }

    private IEnumerator MaterializeAnimation()
    {
        SetDissolveMatState(DissolveState.Start, 0f, new List<Image>());

        float currentTime = 0f;
        float animSpeed = PlayerPrefs.GetFloat("AnimSpeed");
        if (animSpeed != 0)
        {
            while (currentTime < animSpeed)
            {
                float value = currentTime / animSpeed;
                currentTime += Time.deltaTime;
                SetDissolveMatState(DissolveState.Middle, value, new List<Image>());
                yield return null;
            }
        }
        SetDissolveMatState(DissolveState.End, 1f, new List<Image>());
    }


    private IEnumerator DissolveAnimation()
    {
        SetDissolveMatState(DissolveState.Start, 1f, new List<Image>());

        float animSpeed = PlayerPrefs.GetFloat("AnimSpeed");
        float currentTime = animSpeed;

        if (animSpeed != 0)
        {
            while (currentTime > 0f)
            {
                float value = currentTime / animSpeed;
                currentTime -= Time.deltaTime;
                SetDissolveMatState(DissolveState.Middle, value, new List<Image>());

                yield return null;
            }
        }
        SetDissolveMatState(DissolveState.End, 0f, new List<Image>());
    }

    private enum DissolveState
    {
        Start,
        Middle,
        End
    }
}
