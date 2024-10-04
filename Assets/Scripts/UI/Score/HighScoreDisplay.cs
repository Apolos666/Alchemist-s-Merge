using DG.Tweening;
using TMPro;
using UnityEngine;

public class HighScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private Ease _easeType = Ease.InOutQuad;
    [SerializeField] private string _highScoreKey = "HighScore";

    private void Start()
    {
        LoadAndDisplayHighScore();
    }

    private void LoadAndDisplayHighScore()
    {
        var highScore = PlayerPrefs.GetInt(_highScoreKey, 0);
        AnimateTextTo(highScore, _highScoreText);
    }
    
    private void AnimateTextTo(int targetValue, TextMeshProUGUI textComponent)
    {
        var startValue = string.IsNullOrEmpty(textComponent.text) ? 0 : int.Parse(textComponent.text);
        DOTween.To(() => startValue, x => 
            {
                startValue = x;
                textComponent.text = x.ToString();
            }, targetValue, _animationDuration)
            .SetEase(_easeType);
    }
}