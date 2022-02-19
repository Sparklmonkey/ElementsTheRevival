using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSpinAnimation : MonoBehaviour
{
    [SerializeField]
    private Image imageToDissolve, nextImage;
    [SerializeField]
    private Material dissolveMaterial;

    private List<Sprite> imageList;

    public Coroutine SetupSpinner(List<Sprite> imageList)
    {
        this.imageList = imageList;
        nextImage.sprite = imageList[0];
        return StartCoroutine(DissolveAnimation());
    }

    private IEnumerator DissolveAnimation()
    {
        int count = 0;

        while (count < imageList.Count)
        {
            Material dissolveMat = new Material(dissolveMaterial);
            dissolveMat.SetTexture("_MainTex", imageToDissolve.sprite.texture);
            imageToDissolve.material = dissolveMat;
            imageToDissolve.material.SetFloat("_Fade", 1f);
            imageToDissolve.material.SetFloat("_Scale", 25f);
            imageToDissolve.material.SetColor("_EdgeColour", ElementColours.GetElementColour((Element)Random.Range(0, 12)));

            float currentTime = 0.05f;
            while (currentTime > 0f)
            {
                float value = currentTime / 0.05f;
                currentTime -= Time.deltaTime;
                imageToDissolve.material.SetFloat("_Fade", value);

                yield return null;
            }
            imageToDissolve.material.SetFloat("_Fade", 0f);
            imageToDissolve.gameObject.SetActive(false);
            imageToDissolve.material = null;

            count++;
            if(count == imageList.Count)
            {
                break;
            }
            nextImage.sprite = imageList[count];
            imageToDissolve.sprite = imageList[count - 1];
            imageToDissolve.gameObject.SetActive(true);
        }
        SpinManager.finishSpinCount++;
        Game_SoundManager.PlayAudioClip("SpinCardStop");
    }

}
