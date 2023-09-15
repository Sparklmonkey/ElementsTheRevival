using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimations : MonoBehaviour
{
    public List<Sprite> sprites;
    public int spritePerFrame = 6;
    public bool loop = true;
    public bool destroyOnEnd = false;

    private int _index = 0;
    [SerializeField] private Image image;

    public IEnumerator AnimateImage(List<Sprite> animation, Color32 color)
    {
        if (!gameObject.activeInHierarchy)
        {
            yield break;
        }

        spritePerFrame = PlayerPrefs.GetFloat("AnimSpeed") == 0.05f ? 2 : 0;

        sprites = animation;
        image.color = color;
        while (_index != sprites.Count)
        {
            image.sprite = sprites[_index];
            _index++;
            yield return new WaitForFrames(spritePerFrame);
        }

        image.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue);
        Destroy(gameObject);
    }
}
