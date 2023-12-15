public struct PlaySoundEffectEvent : IEvent
{
    public string SoundClipName;
    
    public PlaySoundEffectEvent(string soundClipName)
    {
        SoundClipName = soundClipName;
    }
}