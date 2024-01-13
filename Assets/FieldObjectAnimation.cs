using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldObjectAnimation : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private int spritePerFrame = 6;

    private int _index = 0;
    private List<Sprite> _sprites;
    private ID _id;
    private EventBinding<PlayAnimationEvent> _playAnimationBinding;

    public void SetupId(ID id) => _id = id;
    private void OnDisable()
    {
        EventBus<PlayAnimationEvent>.Unregister(_playAnimationBinding);
    }

    private void Awake()
    {
        _playAnimationBinding = new EventBinding<PlayAnimationEvent>(PlayAnimation);
        EventBus<PlayAnimationEvent>.Register(_playAnimationBinding);
    }
    
    private IEnumerator AnimateImage(List<Sprite> animation, Color32 color)
    {
        if (!gameObject.activeInHierarchy) yield break;

        spritePerFrame = PlayerPrefs.GetFloat("AnimSpeed") == 0.05f ? 2 : 0;

        _sprites = animation;
        image.color = color;
        while (_index != _sprites.Count)
        {
            image.sprite = _sprites[_index];
            _index++;
            yield return new WaitForFrames(spritePerFrame);
        }

        image.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue);
        Destroy(gameObject);
    }
    
    private void PlayAnimation(PlayAnimationEvent playAnimationEvent)
    {
        if (!playAnimationEvent.Id.Equals(_id)) return;
        if (PlayerPrefs.GetFloat("AnimSpeed") == 0) return;
        
        //Get Sprites
        List<Sprite> sprites = new (Resources.LoadAll<Sprite>($"Sprites/Animations/{playAnimationEvent.AnimName}/"));
        sprites.OrderSprites();

        //Get Parent Rect for size adjustment
        var parentRect = transform.parent.gameObject.GetComponent<RectTransform>();

        //Set anim anchors
        rectTransform.anchorMin = new(0.5f, 0.5f);
        rectTransform.anchorMax = new(0.5f, 0.5f);
        Vector2 parentSize = new(parentRect.rect.width, parentRect.rect.height);
        (Vector2, Vector2) adjustedSizeAndOffset = (new(), new());
        Color32 color = new (byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        switch (playAnimationEvent.AnimName)
        {
            case "Lightning":
                adjustedSizeAndOffset = AnimationManager.Instance.GetLightningAnimModification(parentSize);
                break;
            case "DrainLife":
                adjustedSizeAndOffset = AnimationManager.Instance.GetDrainLifeAnimModification(parentSize);
                break;
            case "DeadAndAlive":
                adjustedSizeAndOffset = AnimationManager.Instance.GetDeadAndAliveAnimModifiers(parentSize);
                break;
            case "IceBolt":
                adjustedSizeAndOffset = AnimationManager.Instance.GetIceBoltAnimModification(parentSize);
                break;
            case "ParallelUniverse":
                adjustedSizeAndOffset = AnimationManager.Instance.GetParallelUniverseAnimModification(parentSize);
                break;
            case "Steal":
                adjustedSizeAndOffset = AnimationManager.Instance.GetStealAnimModification(parentSize);
                break;
            case "Web":
                adjustedSizeAndOffset = AnimationManager.Instance.GetWebAnimModification(parentSize);
                break;
            case "Dive":
                adjustedSizeAndOffset = AnimationManager.Instance.GetDiveAnimModification(parentSize);
                break;
            case "Sniper":
                adjustedSizeAndOffset = AnimationManager.Instance.GetSniperAnimModification(parentSize);
                break;
            case "RagePotion":
                adjustedSizeAndOffset = AnimationManager.Instance.GetRagePotionAnimModification(parentSize);
                break;
            case "Purify":
                adjustedSizeAndOffset = AnimationManager.Instance.GetPurifyAnimModification(parentSize);
                break;
            case "Blessing":
                adjustedSizeAndOffset = AnimationManager.Instance.GetBlessingAnimModification(parentSize);
                break;
            case "Mitosis":
                adjustedSizeAndOffset = AnimationManager.Instance.GetMitosisAnimModification(parentSize);
                break;
            case "Mutation":
                adjustedSizeAndOffset = AnimationManager.Instance.GetMutationAnimModification(parentSize);
                break;
            case "CardDeath":
                adjustedSizeAndOffset = AnimationManager.Instance.GetCardDeathAnimModification(parentSize);
                break;
            case "QuantaGenerate":
                adjustedSizeAndOffset = AnimationManager.Instance.GetElementGenerateAnimModification(parentSize);
                color = ElementColours.GetElementColour(playAnimationEvent.Element);
                break;
        }

        rectTransform.sizeDelta = adjustedSizeAndOffset.Item1;
        StartCoroutine(AnimateImage(sprites, color));
    }
}
