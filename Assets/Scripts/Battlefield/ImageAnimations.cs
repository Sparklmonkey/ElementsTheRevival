using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimations : MonoBehaviour
{
    public List<Sprite> sprites;
    public int spritePerFrame = 1;

    private int _index = 0;
    [SerializeField] private Image image;

    public IEnumerator AnimateImage(List<Sprite> animation, Color32 color)
    {
        if (!gameObject.activeInHierarchy)
        {
            yield break;
        }

        spritePerFrame = Mathf.FloorToInt(PlayerPrefs.GetFloat("AnimSpeed") * 2);

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
