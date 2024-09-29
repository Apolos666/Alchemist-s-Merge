public class VolumeChangedEvent : IEvent
{
    public float Volume { get; }

    public VolumeChangedEvent(float volume)
    {
        Volume = volume;
    }
}