using DG.Tweening;
using TMPro;
using UnityEngine;

public class LosePanel : MonoBehaviour
{
    [SerializeField] private RectTransform _losePanel;
    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private TextMeshProUGUI _highestScoreText;
    [SerializeField] private TextMeshProUGUI _totalPlaytimeText;
    [SerializeField] private float _panelAnimationDuration = 0.5f;
    [SerializeField] private float _textAnimationDuration = 1f;
    
    private Vector2 _initialPanelPosition;

    private void Awake()
    {
        _losePanel.gameObject.SetActive(false);
        _initialPanelPosition = _losePanel.anchoredPosition;
    }

    private void Start()
    {
        EventBus.Subscribe<PlayerDefeatEvent>(OnPlayerDefeat);
    }
    
    private void OnDestroy()
    {
        EventBus.Unsubscribe<PlayerDefeatEvent>(OnPlayerDefeat);
    }

    private void OnPlayerDefeat(PlayerDefeatEvent message)
    {
        _losePanel.gameObject.SetActive(true);
        AnimatePanelAppearance();
        AnimateCurrentScore();
        AnimateHighScore();
        AnimatePlayTime();
    }

    private void AnimatePanelAppearance()
    {
        _losePanel.anchoredPosition = _initialPanelPosition + new Vector2(0, -Screen.height);
        _losePanel
            .DOAnchorPos(_initialPanelPosition, _panelAnimationDuration)
            .SetEase(Ease.OutBack);
    }
    
    private void AnimateCurrentScore()
    {
        var currentScore = ScoreManager.Instance.CurrentScore;
        _currentScoreText.text = "Current Score: 0";
        DOTween.To(() => 0, x => _currentScoreText.text = $"Current Score: {x}", currentScore, _textAnimationDuration)
            .SetEase(Ease.OutQuad);
    }

    private void AnimateHighScore()
    {
        var highScore = ScoreManager.Instance.HighScore;
        _highestScoreText.text = "Highest Score: 0";
        DOTween
            .To(() => 0, x => _highestScoreText.text = $"Highest Score: {x}", highScore, _textAnimationDuration)
            .SetEase(Ease.OutQuad);
    }

    private void AnimatePlayTime()
    {
        var playTime = GameManager.Instance.PlayTime;
        _totalPlaytimeText.text = "Total Playtime: 00:00.00";
        
        DOTween
            .To(() => 0f, x => {
                var minutes = Mathf.FloorToInt(x / 60);
                var seconds = x % 60;
                _totalPlaytimeText.text = $"Total Playtime: {minutes:00}:{seconds:00.00}";
            }, playTime, _textAnimationDuration)
            .SetEase(Ease.Linear);
    }
}
