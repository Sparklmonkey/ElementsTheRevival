using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_SoundManager : MonoBehaviour
{
    public static Game_SoundManager shared;
    [SerializeField]
    private AudioSource soundFX;
    [SerializeField]
    private AudioSource backgroundMusic;
    // Start is called before the first frame update
    void Awake()
    {
        if(shared != null)
        {
            return;
        }
        DontDestroyOnLoad(this);
        shared = this;
        UpdateVolume();
    }

    public void UpdateVolume()
    {
        backgroundMusic.volume = PlayerPrefs.GetFloat("BGMVolume") / 100;
        soundFX.volume = PlayerPrefs.GetFloat("SFXVolume") / 100;
    }

    public List<AudioClip> audioClips;

    public void PlayAudioClip(string audioName)
    {
        AudioClip clipToPlay = audioClips.Find(x => x.name == audioName);
        
        soundFX.PlayOneShot(clipToPlay);
    }

    public void StopBGM()
    {
        backgroundMusic.Stop();
    }
    public void PlayBGM(string bgmName)
    {
        AudioClip clipToPlay = audioClips.Find(x => x.name == bgmName);
        
        backgroundMusic.clip = clipToPlay;
        backgroundMusic.loop = true;
        backgroundMusic.Play();
    }
}
