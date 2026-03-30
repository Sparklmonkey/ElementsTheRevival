using System.Collections;
using SplashScreen;
using UnityEngine.Serialization;

namespace V3.Scripts
{
    using System.Collections.Generic;
    using UnityEngine;

    public class ElementAnimationController : MonoBehaviour
    {
        private static readonly int Fade = Shader.PropertyToID("_Fade");
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");
        private static readonly int Scale = Shader.PropertyToID("_Scale");
        public SpriteRenderer titleSprite;
        public GameObject spriteAnimationPrefab;
        public List<Transform> animationPath;
        private SplashScreenModel _model;

        public void SetupAnimationController(SplashScreenModel model)
        {
            _model = model;
            SetupSpritePath();
            StartCoroutine(StartTitleAnimation());
        }

        public void EndAnimation()
        {
            titleSprite.material.SetFloat(Fade, 1f);
        }

        private void SetupSpritePath()
        {
            var spriteAnimationObject = Instantiate(spriteAnimationPrefab, transform);
            var splashSprite = spriteAnimationObject.GetComponent<SplashSpritePathAnimation>(); 
            splashSprite.SetPath(animationPath, 0);
            splashSprite.StartNextSpriteAnimation += StartNextAnimation;
        }
        
        private void StartNextAnimation(int spriteIndex)
        {
            if (spriteIndex == animationPath.Count - 1)
            {
                _model.CompleteAnimation();
                return;
            }

            var newIndex = spriteIndex + 1;
            var spriteAnimationObject = Instantiate(spriteAnimationPrefab, transform);
            var splashSprite = spriteAnimationObject.GetComponent<SplashSpritePathAnimation>();
            splashSprite.SetPath(animationPath.GetRange(0, animationPath.Count - newIndex), newIndex);
            splashSprite.StartNextSpriteAnimation += StartNextAnimation;
        }
        
        
        private IEnumerator StartTitleAnimation()
        {
            var shader = titleSprite.material;
            shader.SetTexture(MainTex, titleSprite.sprite.texture);
            shader.SetFloat(Fade, 0f);
            shader.SetFloat(Scale, 100f);
            var currentTime = 0f;
            while (currentTime < 4f)
            {
                var value = currentTime / 4f;
                currentTime += Time.deltaTime;
                shader.SetFloat(Fade, value);
                yield return null;
            }
            shader.SetFloat(Fade, 1f);
        }
    }
}