using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_SoundManager : MonoBehaviour
{
    private static AudioSource soundFX;
    private static AudioSource backgroundMusic;
    // Start is called before the first frame update
    void Awake()
    {

        if(soundFX != null)
        {
            return;
        }
        DontDestroyOnLoad(gameObject);
        AudioSource[] audioSources = GetComponents<AudioSource>();
        soundFX = audioSources[0];
        backgroundMusic = audioSources[1];
        audioClipsStatic = audioClips;
        UpdateVolume();
    }

    public static void UpdateVolume()
    {
        backgroundMusic.volume = PlayerPrefs.GetFloat("BGMVolume") / 100;
        soundFX.volume = PlayerPrefs.GetFloat("SFXVolume") / 100;
    }

    public List<AudioClip> audioClips;
    private static List<AudioClip> audioClipsStatic;

    public static void PlayAudioClip(string audioName)
    {
        foreach (AudioClip clip in audioClipsStatic)
        {
            if(clip.name == audioName)
            {
                soundFX.PlayOneShot(clip);
            }
        } 
    }

    public static void StopBGM()
    {
        backgroundMusic.Stop();
    }
    public static void PlayBGM(string bgmName)
    {
        foreach (AudioClip clip in audioClipsStatic)
        {
            if (clip.name == bgmName)
            {
                backgroundMusic.clip = clip;
                backgroundMusic.loop = true;
                backgroundMusic.Play();
            }
        }
    }
}
