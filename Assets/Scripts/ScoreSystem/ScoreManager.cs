using UnityEngine;

public class ScoreManager : GenericSingleton<ScoreManager>
{
    private int _currentScore;
    private int _highScore;

    public const string HighScoreKey = "HighScore";

    private void OnEnable()
    {
        _highScore = PlayerPrefs.GetInt(HighScoreKey, 0);     
        EventBus.Subscribe<ScoreUpdateEvent>(OnScoreUpdate);
    }
    
    private void OnDisable()
    {
        EventBus.Unsubscribe<ScoreUpdateEvent>(OnScoreUpdate);
    }

    private void OnScoreUpdate(ScoreUpdateEvent message)
    {
        _currentScore +=  message.Score;
        EventBus.Publish(new TotalScoreChangedEvent(_currentScore));
        UpdateHighScore();
    }

    private void UpdateHighScore()
    {
        if (_currentScore <= _highScore) return;
        
        _highScore = _currentScore;
        PlayerPrefs.SetInt(HighScoreKey, _highScore);
        PlayerPrefs.Save();
        EventBus.Publish(new HighScoreUpdateEvent(_highScore));
    }
}