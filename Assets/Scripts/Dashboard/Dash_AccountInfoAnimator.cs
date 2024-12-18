using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DashAccountInfoAnimator : MonoBehaviour
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
        var alpha = accountInfoFrame.color.a;
        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            var newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            accountInfoFrame.color = newColor;
            yield return null;
        }
        content.SetActive(aValue == 1f);
        if (aValue == 1f)
        {
            GetComponent<DashAccountManagement>().UpdateFieldsWithInfo();
        }
        else
        {
            GetComponent<DashAccountManagement>().ClearPwdFields();
            gameObject.SetActive(false);
        }
    }
}
