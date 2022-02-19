using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Error_Animated_Battlefield : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> frameOpacity;
    [SerializeField]
    private List<Sprite> animationSprites;
    [SerializeField]
    private TextMeshProUGUI errorMessage;

    private string messageToDisplay;

    public int spritePerFrame = 3;

    private int index = 0;
    [SerializeField]
    private Image image;
    public void DisplayAnimatedError(string errorMessage)
    {
        gameObject.SetActive(true);
        messageToDisplay = errorMessage;
        StartCoroutine(AnimateErrorMessage());
    }

    private IEnumerator AnimateErrorMessage()
    {
        while (index != animationSprites.Count)
        {
            image.sprite = animationSprites[index];
            index++;
            yield return new WaitForFrames(spritePerFrame);
        }

        errorMessage.text = messageToDisplay;

        foreach (GameObject item in frameOpacity)
        {
            item.SetActive(true);
            yield return new WaitForFrames(spritePerFrame);
        }

        yield return new WaitForSeconds(5f);
        foreach (GameObject item in frameOpacity)
        {
            item.SetActive(false);
            yield return new WaitForFrames(spritePerFrame);
        }

        errorMessage.text = "";

        for (int i = animationSprites.Count - 1; i > 0; i--)
        {
            image.sprite = animationSprites[i];
            yield return new WaitForFrames(spritePerFrame);
        }
        index = 0;
        gameObject.SetActive(false);
    }

}
