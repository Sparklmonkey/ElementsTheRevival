using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class ImageAnimations : MonoBehaviour
{

	public List<Sprite> sprites;
	public int spritePerFrame = 6;
	public bool loop = true;
	public bool destroyOnEnd = false;

	private int index = 0;
	[SerializeField]
	private Image image;


	public void PlayAnimation(List<Sprite> animation, ActionEffect effect, Color32 color)
    {
		if(PlayerPrefs.GetFloat("AnimSpeed") == 0.05f)
        {
			spritePerFrame = 2;
        }
        else
        {
			spritePerFrame = 0;

		}
		sprites = animation;
		image.color = color;
		StartCoroutine(AnimateImage(effect));
    }

	private IEnumerator AnimateImage(ActionEffect effect)
    {
		while(index != sprites.Count)
		{
			image.sprite = sprites[index];
			index++;
			yield return new WaitForFrames(spritePerFrame);
		}
		image.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue);
		effect();
		Destroy(gameObject);
	}
}
