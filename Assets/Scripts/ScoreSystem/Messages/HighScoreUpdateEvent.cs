public class HighScoreUpdateEvent : IEvent
{
    public int HighScore { get; }
    public HighScoreUpdateEvent(int highScore) => HighScore = highScore;
}