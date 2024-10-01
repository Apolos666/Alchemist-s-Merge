using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private Ease _easeType = Ease.InOutQuad;

    private void Start()
    {
        InitializeScores();
    }

    private void InitializeScores()
    {
        AnimateTextTo(0, _scoreText);
        AnimateTextTo(PlayerPrefs.GetInt(ScoreManager.HighScoreKey, 0), _highScoreText);
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
    
    private void OnTotalScoreChanged(TotalScoreChangedEvent message)
    {
        AnimateTextTo(message.TotalScore, _scoreText);
    }

    private void OnHighScoreUpdate(HighScoreUpdateEvent message)
    {
        AnimateTextTo(message.HighScore, _highScoreText);
    }

    private void AnimateTextTo(int targetValue, TextMeshProUGUI textComponent)
    {
        var startValue = int.Parse(textComponent.text);
        DOTween.To(() => startValue, x => 
            {
                startValue = x;
                textComponent.text = x.ToString();
            }, targetValue, _animationDuration)
            .SetEase(_easeType);
    }
}
