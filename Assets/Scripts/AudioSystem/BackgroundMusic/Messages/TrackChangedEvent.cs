public class TrackChangedEvent : IEvent
{
    public string TrackName { get; }

    public TrackChangedEvent(string trackName)
    {
        TrackName = trackName;
    }
}