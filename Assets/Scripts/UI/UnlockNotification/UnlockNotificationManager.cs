using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnlockNotificationManager : MonoBehaviour
{
    [SerializeField] private RectTransform _notificationPanel;
    [SerializeField] private Image _itemIcon;
    [SerializeField] private Image _overlay;

    [Header("Animation Settings")]
    [SerializeField] private float _showDuration = 0.5f;
    [SerializeField] private float _hideDuration = 0.3f;
    [SerializeField] private Ease _showEase = Ease.OutBack;
    [SerializeField] private Ease hideEase = Ease.InBack;
    [SerializeField] private Vector2 _hiddenPosition = new Vector2(0, -300);
    [SerializeField] private Vector2 _shownPosition = Vector2.zero;

    private bool _isVisible = false;
    private GraphicRaycaster _raycaster;
    private EventSystem _eventSystem;

    private void Start()
    {
        _notificationPanel.anchoredPosition = _hiddenPosition;
        _notificationPanel.gameObject.SetActive(false);

        _raycaster = GetComponent<GraphicRaycaster>();
        _eventSystem = EventSystem.current;
        
        EventBus.Subscribe<PropUnlockedEvent>(OnPropUnlocked);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<PropUnlockedEvent>(OnPropUnlocked);
    }

    private void Update()
    {
        if (_isVisible && Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUIElement(_notificationPanel))
            {
                HideNotification();
            }
        }
    }

    private void OnPropUnlocked(PropUnlockedEvent message)
    {
        ShowNotification(message.UnlockedPropIcon);
    }

    private void ShowNotification(Sprite icon)
    {
        _itemIcon.sprite = icon;

        _isVisible = true;
        AnimatePanel(_shownPosition, _showEase, _showDuration);
        AnimateOverlay(true);
    }

    private void AnimateOverlay(bool show)
    {
        _overlay.DOKill();
        
        _overlay.gameObject.SetActive(true);
        
        var targetColor = show ? Color.black : Color.clear;
        var targetAlpha = show ? 0.2f : 0;
        
        _overlay.DOColor(new Color(targetColor.r, targetColor.g, targetColor.b, targetAlpha), _showDuration)
            .SetEase(_showEase)
            .OnComplete(() =>
            {
                if (!show)
                    _overlay.gameObject.SetActive(false);
            });
    }

    private void HideNotification()
    {
        _isVisible = false;
        AnimatePanel(_hiddenPosition, hideEase, _hideDuration);
        AnimateOverlay(false);
    }

    private void AnimatePanel(Vector2 targetPosition, Ease easeType, float duration)
    {
        _notificationPanel.DOKill();

        // Animate the panel's position
        _notificationPanel.DOAnchorPos(targetPosition, duration)
            .SetEase(easeType)
            .OnStart(() => _notificationPanel.gameObject.SetActive(true))
            .OnComplete(() => 
            {
                if (!_isVisible)
                    _notificationPanel.gameObject.SetActive(false);
            });

        // Add a fade effect
        var canvasGroup = _notificationPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = _notificationPanel.gameObject.AddComponent<CanvasGroup>();

        canvasGroup.DOFade(_isVisible ? 1 : 0, duration);

        // Add a scale effect
        _notificationPanel.DOScale(_isVisible ? Vector3.one : Vector3.one * 0.8f, duration)
            .SetEase(easeType);

        // Add a rotation effect
        _notificationPanel.DORotate(new Vector3(0, 0, _isVisible ? 0 : 10), duration)
            .SetEase(easeType);
    }

    private bool IsPointerOverUIElement(RectTransform rectTransform)
    {
        var eventData = new PointerEventData(_eventSystem)
        {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        _raycaster.Raycast(eventData, results);

        return results.Any(result => result.gameObject.GetComponent<RectTransform>() == rectTransform);
    }
}
