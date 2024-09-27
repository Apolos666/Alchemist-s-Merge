public class TotalScoreChangedEvent : IEvent
{
    public int TotalScore { get; }

    public TotalScoreChangedEvent(int totalScore) => TotalScore = totalScore;
}