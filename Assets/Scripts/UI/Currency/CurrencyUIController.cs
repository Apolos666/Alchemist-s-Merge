using DG.Tweening;
using TMPro;
using UnityEngine;

public class CurrencyUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private float _animationDuration;
    
    private int _currentGold;

    private void Start()
    {
        EventBus.Subscribe<TotalGoldChangedEvent>(OnTotalGoldChanged);
        _currentGold = CurrencyManager.Instance.CurrentGold;
        UpdateGoldText(_currentGold);
    }

    private void UpdateGoldText(int newGold)
    {
        DOTween
            .To(() => _currentGold, x =>
            {
                _currentGold = x;
                _goldText.text = _currentGold.ToString();
            }, newGold, _animationDuration)
            .SetEase(Ease.OutQuad);
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