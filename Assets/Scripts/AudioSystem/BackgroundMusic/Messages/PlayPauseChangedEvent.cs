public class PlayPauseChangedEvent : IEvent
{
    public bool IsPlaying { get; }

    public PlayPauseChangedEvent(bool isPlaying)
    {
        IsPlaying = isPlaying;
    }
}