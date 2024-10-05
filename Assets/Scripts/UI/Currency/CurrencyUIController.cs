using DG.Tweening;
using TMPro;
using UnityEngine;

public class CurrencyUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private float _animationDuration;
    
    private int _currentGold;
    private Tween _currentTween;
    private bool _isFirstUpdate = true;

    private void Start()
    {
        EventBus.Subscribe<TotalGoldChangedEvent>(OnTotalGoldChanged);
        _currentGold = CurrencyManager.Instance.CurrentGold;
        UpdateGoldText(_currentGold);
    }

    private void UpdateGoldText(int newGold)
    {
        if (_currentTween != null && _currentTween.IsActive())
        {
            _currentTween.Kill();
        }

        var startValue = _isFirstUpdate ? 0 : _currentGold;

        _currentTween = DOTween.To(() => startValue, x =>
            {
                _currentGold = Mathf.FloorToInt(x);
                _goldText.text = _currentGold.ToString();
            }, newGold, _animationDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => _isFirstUpdate = false);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<TotalGoldChangedEvent>(OnTotalGoldChanged);
    }

    private void OnTotalGoldChanged(TotalGoldChangedEvent message)
    {
        UpdateGoldText(message.TotalGold);
    }
}