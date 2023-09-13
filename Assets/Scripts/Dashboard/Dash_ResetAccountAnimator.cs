using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Dash_ResetAccountAnimator : MonoBehaviour
{
    [SerializeField]
    private Image accountInfoFrame;
    [SerializeField]
    private GameObject content;
    public void AnimateAccountInfoFrame()
    {
        gameObject.SetActive(true);
        StartCoroutine(AnimateShowFrame(1.0f, 0.5f));

    }

    public void DeAnimateFrame()
    {
        content.SetActive(false);
        StartCoroutine(AnimateShowFrame(0f, 0.5f));
    }

    private IEnumerator AnimateShowFrame(float aValue, float aTime)
    {
        float alpha = accountInfoFrame.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            accountInfoFrame.color = newColor;
            yield return null;
        }
        content.SetActive(aValue == 1f);
        if (aValue == 0f)
        {
            gameObject.SetActive(false);
        }
    }
}
