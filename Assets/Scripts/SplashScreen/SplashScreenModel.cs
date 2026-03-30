using System;
using UnityEngine;

namespace SplashScreen
{
    public class SplashScreenModel
    {
        public bool IsLoadingNextScene { get; set; }
        public bool IsCachedLogin { get; set; }
        public bool DataLoaded { get; set; }
        public int AnimationCompletedCount;
        public bool IsAnimationComplete { 
            get => _isAnimationComplete;
            set
            {
                if (_isAnimationComplete != value)
                {
                    _isAnimationComplete = value;
                    OnAnimationCompleteChanged?.Invoke(value);
                }
            }
        }
        private bool _isAnimationComplete;
        public event Action<bool> OnAnimationCompleteChanged;

        public void CompleteAnimation()
        {
            Debug.Log(OnAnimationCompleteChanged);
            OnAnimationCompleteChanged?.Invoke(true);
        }
        public int CurrentIndex { get; set; }
        
        public bool HasSeenSplash
        {
            get => PlayerPrefs.GetInt("HasSeenSplash") == 1;
            set => PlayerPrefs.SetInt("HasSeenSplash", value ? 1 : 0);
        }


        public void InitializePlayerPrefs()
        {
            PlayerPrefs.SetInt("IsGuest", 0);
            PlayerPrefs.SetInt("IsTrainer", 0);

            if (PlayerPrefs.GetInt("HasKeysStored") == 1) return;
            PlayerPrefs.SetInt("HasKeysStored", 1);
            PlayerPrefs.SetInt("QuickPlay", 1);
            PlayerPrefs.SetFloat("AnimSpeed", 1f);
            PlayerPrefs.SetFloat("BGMVolume", 100f);
            PlayerPrefs.SetFloat("SFXVolume", 100f);
        }
    }
}