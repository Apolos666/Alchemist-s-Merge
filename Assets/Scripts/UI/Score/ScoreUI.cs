using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    private readonly ScoreTextEffect _scoreTextEffect = new ScoreTextEffect();
    private readonly ScoreTextEffect _highScoreTextEffect = new ScoreTextEffect();

    private void Start()
    {
        InitializeScores().Forget();
    }

    private async UniTaskVoid InitializeScores()
    {
        await _scoreTextEffect.AnimateTextTo(0, _scoreText, _animationDuration, _animationCurve);
        await _highScoreTextEffect.AnimateTextTo(PlayerPrefs.GetInt(ScoreManager.HighScoreKey, 0), _highScoreText, _animationDuration, _animationCurve);
    }

    private void OnEnable()
    {
        EventBus.Subscribe<TotalScoreChangedEvent>(OnTotalScoreChanged);
        EventBus.Subscribe<HighScoreUpdateEvent>(OnHighScoreUpdate);
    }
    
    private void OnDisable()
    {
        EventBus.Unsubscribe<TotalScoreChangedEvent>(OnTotalScoreChanged);
        EventBus.Unsubscribe<HighScoreUpdateEvent>(OnHighScoreUpdate);
    }
    
    private async void OnTotalScoreChanged(TotalScoreChangedEvent message)
    {
        await _scoreTextEffect.AnimateTextTo(message.TotalScore, _scoreText, _animationDuration, _animationCurve);
    }

    private async void OnHighScoreUpdate(HighScoreUpdateEvent message)
    {
        await _scoreTextEffect.AnimateTextTo(message.HighScore, _highScoreText, _animationDuration, _animationCurve);
    }
}
