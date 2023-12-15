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
    [SerializeField]
    private GameObject upgradedIndicator;
    public bool isUpgraded;
    private List<Sprite> _imageList;

    //public Coroutine SetupSpinner(List<Sprite> imageList)
    //{
    //    return StartCoroutine(DissolveAnimation());
    //}

    public IEnumerator DissolveAnimation(List<Sprite> imageList)
    {
        this._imageList = imageList;
        nextImage.sprite = imageList[0];
        var count = 0;
        upgradedIndicator.SetActive(false);

        while (count < imageList.Count)
        {
            var dissolveMat = new Material(dissolveMaterial);
            dissolveMat.SetTexture("_MainTex", imageToDissolve.sprite.texture);
            imageToDissolve.material = dissolveMat;
            imageToDissolve.material.SetFloat("_Fade", 1f);
            imageToDissolve.material.SetFloat("_Scale", 25f);
            imageToDissolve.material.SetColor("_EdgeColour", ElementColours.GetElementColour((Element)Random.Range(0, 12)));

            var currentTime = 0.05f;
            while (currentTime > 0f)
            {
                var value = currentTime / 0.05f;
                currentTime -= Time.deltaTime;
                imageToDissolve.material.SetFloat("_Fade", value);

                yield return null;
            }
            imageToDissolve.material.SetFloat("_Fade", 0f);
            imageToDissolve.gameObject.SetActive(false);
            imageToDissolve.material = null;

            count++;
            if (count == imageList.Count)
            {
                break;
            }
            nextImage.sprite = imageList[count];
            imageToDissolve.sprite = imageList[count - 1];
            imageToDissolve.gameObject.SetActive(true);
        }
        SpinManager.FinishSpinCount++;
        if (isUpgraded)
        {
            upgradedIndicator.SetActive(true);
        }
        EventBus<PlaySoundEffectEvent>.Raise(new PlaySoundEffectEvent("SpinCardStop"));
    }

}
