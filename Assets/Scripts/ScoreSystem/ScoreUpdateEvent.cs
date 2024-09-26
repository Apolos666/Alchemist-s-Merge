public class ScoreUpdateEvent : IEvent
{
    public int Score { get; }
    public ScoreUpdateEvent(int score) => Score = score;
}