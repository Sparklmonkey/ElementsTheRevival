using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMono<SoundManager>
{
    [SerializeField]
    private AudioSource soundFX;
    [SerializeField]
    private AudioSource backgroundMusic;
    
    private EventBinding<PlaySoundEffectEvent> _playSoundEffectBinding;
    
    private void OnDisable() {
        EventBus<PlaySoundEffectEvent>.Unregister(_playSoundEffectBinding);
    }
    private void OnEnable()
    {
        _playSoundEffectBinding = new EventBinding<PlaySoundEffectEvent>(PlayAudioClip);
        EventBus<PlaySoundEffectEvent>.Register(_playSoundEffectBinding);
    }
    // Start is called before the first frame update
    private void Start()
    {
        UpdateVolume();
    }

    public void UpdateVolume()
    {
        backgroundMusic.volume = PlayerPrefs.GetFloat("BGMVolume") / 100;
        soundFX.volume = PlayerPrefs.GetFloat("SFXVolume") / 100;
    }

    public List<AudioClip> audioClips;

    private void PlayAudioClip(PlaySoundEffectEvent playSoundEffectEvent)
    {
        var clipToPlay = audioClips.Find(x => x.name == playSoundEffectEvent.SoundClipName);
        soundFX.PlayOneShot(clipToPlay);
    }

    public void StopBGM()
    {
        backgroundMusic.Stop();
    }

    public void PlayBGM(string bgmName)
    {
        var clipToPlay = audioClips.Find(x => x.name == bgmName);

        backgroundMusic.clip = clipToPlay;
        backgroundMusic.loop = true;
        backgroundMusic.Play();
    }
}
