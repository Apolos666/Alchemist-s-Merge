using UnityEngine;

public class ScoreManager : GenericSingleton<ScoreManager>
{
    public int CurrentScore { get; private set; }
    public int HighScore { get; private set; }

    public const string HighScoreKey = "HighScore";

    private void OnEnable()
    {
        HighScore = PlayerPrefs.GetInt(HighScoreKey, 0);     
        EventBus.Subscribe<ObjectMergingEvent>(OnScoreUpdate);
        EventBus.Subscribe<PropBeingDestroyEvent>(OnPropBeingDestroyEvent);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<ObjectMergingEvent>(OnScoreUpdate);
        EventBus.Unsubscribe<PropBeingDestroyEvent>(OnPropBeingDestroyEvent);
    }

    private void OnScoreUpdate(ObjectMergingEvent message)
    {
        CurrentScore +=  message.Point;
        EventBus.Publish(new TotalScoreChangedEvent(CurrentScore));
        UpdateHighScore();
    }
    
    private void OnPropBeingDestroyEvent(PropBeingDestroyEvent message)
    {
        CurrentScore += message.Point;
        EventBus.Publish(new TotalScoreChangedEvent(CurrentScore));
        UpdateHighScore();
    }

    private void UpdateHighScore()
    {
        if (CurrentScore <= HighScore) return;
        
        HighScore = CurrentScore;
        PlayerPrefs.SetInt(HighScoreKey, HighScore);
        PlayerPrefs.Save();
        EventBus.Publish(new HighScoreUpdateEvent(HighScore));
    }
}