using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityAnimation : MonoBehaviour
{
	[SerializeField]
	private List<Sprite> sprites;
	public Image image;
	private void OnEnable()
	{
		StartCoroutine(AnimateImage());
	}

	private IEnumerator AnimateImage()
	{
		int index = 0;
		while (gameObject.activeInHierarchy)
		{
			if(index >= sprites.Count) { index = 0; }
			image.sprite = sprites[index];
			index++;
			yield return new WaitForFrames(10);
		}
	}

    private void OnDisable()
    {
		image.sprite = sprites[0];
		StopCoroutine(AnimateImage());
    }
}
