using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : SingletonMono<AnimationManager>
{
    [SerializeField]
    private GameObject animationPrefab;

    public void StartAnimation(string animName, Transform animTransform, Element element = Element.Other)
    {
        StartCoroutine(PlayAnimation(animName, animTransform, element));
    }

    private IEnumerator PlayAnimation(string animName, Transform animTransform, Element element = Element.Other)
    {
        if (PlayerPrefs.GetFloat("AnimSpeed") == 0)
        {
            yield break;
        }
        //Get Sprites
        List<Sprite> sprites = new (Resources.LoadAll<Sprite>($"Sprites/Animations/{animName}/"));
        sprites.OrderSprites();

        //Get Parent Rect for size adjustment
        var parentRect = animTransform.gameObject.GetComponent<RectTransform>();

        //Instantiate Animation Prefab 
        var anim = Instantiate(animationPrefab, transform);
        //Get Animation Rect to ajdust
        var rect = anim.GetComponent<RectTransform>();
        anim.transform.position = animTransform.position;
        //Set anim anchors
        rect.anchorMin = new(0.5f, 0.5f);
        rect.anchorMax = new(0.5f, 0.5f);
        Vector2 parentSize = new(parentRect.rect.width, parentRect.rect.height);
        (Vector2, Vector2) adjustedSizeAndOffset = (new(), new());
        Color32 color = new (byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        switch (animName)
        {
            case "Lightning":
                adjustedSizeAndOffset = GetLightningAnimModification(parentSize);
                break;
            case "DrainLife":
                adjustedSizeAndOffset = GetDrainLifeAnimModification(parentSize);
                break;
            case "DeadAndAlive":
                adjustedSizeAndOffset = GetDeadAndAliveAnimModifiers(parentSize);
                break;
            case "IceBolt":
                adjustedSizeAndOffset = GetIceBoltAnimModification(parentSize);
                break;
            case "ParallelUniverse":
                adjustedSizeAndOffset = GetParallelUniverseAnimModification(parentSize);
                break;
            case "Steal":
                adjustedSizeAndOffset = GetStealAnimModification(parentSize);
                break;
            case "Web":
                adjustedSizeAndOffset = GetWebAnimModification(parentSize);
                break;
            case "Dive":
                adjustedSizeAndOffset = GetDiveAnimModification(parentSize);
                break;
            case "Sniper":
                adjustedSizeAndOffset = GetSniperAnimModification(parentSize);
                break;
            case "RagePotion":
                adjustedSizeAndOffset = GetRagePotionAnimModification(parentSize);
                break;
            case "Purify":
                adjustedSizeAndOffset = GetPurifyAnimModification(parentSize);
                break;
            case "Blessing":
                adjustedSizeAndOffset = GetBlessingAnimModification(parentSize);
                break;
            case "Mitosis":
                adjustedSizeAndOffset = GetMitosisAnimModification(parentSize);
                break;
            case "Mutation":
                adjustedSizeAndOffset = GetMutationAnimModification(parentSize);
                break;
            case "CardDeath":
                adjustedSizeAndOffset = GetCardDeathAnimModification(parentSize);
                break;
            case "QuantaGenerate":
                adjustedSizeAndOffset = GetElementGenerateAnimModification(parentSize);
                color = ElementColours.GetElementColour(element);
                break;
        }

        rect.sizeDelta = adjustedSizeAndOffset.Item1;
        //rect.anchoredPosition = adjustedSizeAndOffset.Item2;
        yield return StartCoroutine(anim.GetComponent<ImageAnimations>().AnimateImage(sprites, color));
    }

    public (Vector2, Vector2) GetLightningAnimModification(Vector2 parentSize)
    {
        Vector2 baseImageSize = new(123, 207);
        Vector2 effectSize = new(73, 74);
        Vector2 offset = new(6.9f, 66.6f);

        var newSize = GetAdjustedSize(parentSize, baseImageSize, effectSize);
        var newOffset = GetAdjustedOffset(newSize, baseImageSize, offset);

        return (newSize, newOffset);
    }

    public (Vector2, Vector2) GetCardDeathAnimModification(Vector2 parentSize)
    {
        Vector2 baseImageSize = new(520, 101);
        Vector2 effectSize = new(73, 74);
        Vector2 offset = new(8.5f, 0f);

        var newSize = GetAdjustedSize(parentSize, baseImageSize, effectSize);
        var newOffset = GetAdjustedOffset(newSize, baseImageSize, offset);

        return (newSize, newOffset);
    }

    public (Vector2, Vector2) GetMutationAnimModification(Vector2 parentSize)
    {
        Vector2 baseImageSize = new(106, 106);
        Vector2 effectSize = new(106, 106);
        Vector2 offset = new(0f, 0f);

        var newSize = GetAdjustedSize(parentSize, baseImageSize, effectSize);
        var newOffset = GetAdjustedOffset(newSize, baseImageSize, offset);

        return (newSize, newOffset);
    }
    public (Vector2, Vector2) GetBlessingAnimModification(Vector2 parentSize)
    {
        Vector2 baseImageSize = new(217, 166);
        Vector2 effectSize = new(73, 74);
        Vector2 offset = new(0f, 0f);

        var newSize = GetAdjustedSize(parentSize, baseImageSize, effectSize);
        var newOffset = GetAdjustedOffset(newSize, baseImageSize, offset);

        return (newSize, newOffset);
    }
    public (Vector2, Vector2) GetMitosisAnimModification(Vector2 parentSize)
    {
        Vector2 baseImageSize = new(561, 426);
        Vector2 effectSize = new(561, 426);
        Vector2 offset = new(0f, 0f);

        var newSize = GetAdjustedSize(parentSize, baseImageSize, effectSize);
        var newOffset = GetAdjustedOffset(newSize, baseImageSize, offset);

        return (newSize, newOffset);
    }

    public (Vector2, Vector2) GetPurifyAnimModification(Vector2 parentSize)
    {
        Vector2 baseImageSize = new(106, 106);
        Vector2 effectSize = new(73, 74);
        Vector2 offset = new(0f, 0f);

        var newSize = GetAdjustedSize(parentSize, baseImageSize, effectSize);
        var newOffset = GetAdjustedOffset(newSize, baseImageSize, offset);

        return (newSize, newOffset);
    }

    public (Vector2, Vector2) GetElementGenerateAnimModification(Vector2 parentSize)
    {
        Vector2 baseImageSize = new(153, 78);
        Vector2 effectSize = new(73, 74);
        Vector2 offset = new(0f, 1.5f);

        var newSize = GetAdjustedSize(parentSize, baseImageSize, effectSize);
        var newOffset = GetAdjustedOffset(newSize, baseImageSize, offset);

        return (newSize, newOffset);
    }

    public (Vector2, Vector2) GetDiveAnimModification(Vector2 parentSize)
    {
        Vector2 baseImageSize = new(149, 112);
        Vector2 effectSize = new(73, 74);
        Vector2 offset = new(0f, 17f);

        var newSize = GetAdjustedSize(parentSize, baseImageSize, effectSize);
        var newOffset = GetAdjustedOffset(newSize, baseImageSize, offset);

        return (newSize, newOffset);
    }

    public (Vector2, Vector2) GetSniperAnimModification(Vector2 parentSize)
    {
        Vector2 baseImageSize = new(123, 317);
        Vector2 effectSize = new(73, 74);
        Vector2 offset = new(-51.5f, 148.5f);

        var newSize = GetAdjustedSize(parentSize, baseImageSize, effectSize);
        var newOffset = GetAdjustedOffset(newSize, baseImageSize, offset);

        return (newSize, newOffset);
    }

    public (Vector2, Vector2) GetRagePotionAnimModification(Vector2 parentSize)
    {
        Vector2 baseImageSize = new(73, 78);
        Vector2 effectSize = new(73, 74);
        Vector2 offset = new(0f, 2.5f);

        var newSize = GetAdjustedSize(parentSize, baseImageSize, effectSize);
        var newOffset = GetAdjustedOffset(newSize, baseImageSize, offset);

        return (newSize, newOffset);
    }

    public (Vector2, Vector2) GetParallelUniverseAnimModification(Vector2 parentSize)
    {
        Vector2 baseImageSize = new(105, 106);
        Vector2 effectSize = new(73, 74);
        Vector2 offset = new(0f, 0f);

        var newSize = GetAdjustedSize(parentSize, baseImageSize, effectSize);
        var newOffset = GetAdjustedOffset(newSize, baseImageSize, offset);

        return (newSize, newOffset);
    }

    public (Vector2, Vector2) GetStealAnimModification(Vector2 parentSize)
    {
        Vector2 baseImageSize = new(106, 106);
        Vector2 effectSize = new(73, 74);
        Vector2 offset = new(0f, 0f);

        var newSize = GetAdjustedSize(parentSize, baseImageSize, effectSize);
        var newOffset = GetAdjustedOffset(newSize, baseImageSize, offset);

        return (newSize, newOffset);
    }

    public (Vector2, Vector2) GetWebAnimModification(Vector2 parentSize)
    {
        Vector2 baseImageSize = new(383, 117);
        Vector2 effectSize = new(73, 74);
        Vector2 offset = new(118.5f, 0f);

        var newSize = GetAdjustedSize(parentSize, baseImageSize, effectSize);
        var newOffset = GetAdjustedOffset(newSize, baseImageSize, offset);

        return (newSize, newOffset);
    }

    public (Vector2, Vector2) GetDeadAndAliveAnimModifiers(Vector2 parentSize)
    {
        Vector2 baseImageSize = new(521, 102);
        Vector2 effectSize = new(73, 74);
        Vector2 offset = new(47.5f, 0f);

        var newSize = GetAdjustedSize(parentSize, baseImageSize, effectSize);
        var newOffset = GetAdjustedOffset(newSize, baseImageSize, offset);

        return (newSize, newOffset);
    }

    public (Vector2, Vector2) GetDrainLifeAnimModification(Vector2 parentSize)
    {
        Vector2 baseImageSize = new(144, 161);
        Vector2 effectSize = new(73, 73);
        Vector2 offset = new(30f, 9.6f);

        var newSize = GetAdjustedSize(parentSize, baseImageSize, effectSize);
        var newOffset = GetAdjustedOffset(newSize, baseImageSize, offset);

        return (newSize, newOffset);
    }

    public (Vector2, Vector2) GetIceBoltAnimModification(Vector2 parentSize)
    {
        Vector2 baseImageSize = new(172, 255);
        Vector2 effectSize = new(73, 73);
        Vector2 offset = new(31.5f, 62.5f);

        var newSize = GetAdjustedSize(parentSize, baseImageSize, effectSize);
        var newOffset = GetAdjustedOffset(newSize, baseImageSize, offset);

        return (newSize, newOffset);
    }

    public Vector2 GetAdjustedOffset(Vector2 neededSize, Vector2 baseImageSize, Vector2 offset)
    {
        Vector2 result = new(offset.x / baseImageSize.x * neededSize.x, offset.y / baseImageSize.y * neededSize.y);
        return result;
    }

    public Vector2 GetAdjustedSize(Vector2 neededSize, Vector2 baseImageSize, Vector2 effectSize)
    {
        Vector2 result = new(neededSize.x * baseImageSize.x / effectSize.x, neededSize.y * baseImageSize.y / effectSize.y);
        return result;
    }
}
