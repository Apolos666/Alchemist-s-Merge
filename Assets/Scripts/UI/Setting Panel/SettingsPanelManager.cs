using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsPanelManager : MonoBehaviour
{
    [SerializeField] private RectTransform _settingsPanel;
    [SerializeField] private Image _overlay;
    [SerializeField] private Button _settingsButton;

    [Header("Animation Settings")]
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private Ease showEase = Ease.OutBack;
    [SerializeField] private Ease hideEase = Ease.InBack;
    [SerializeField] private Vector2 hiddenPosition = new Vector2(300, 0);
    [SerializeField] private Vector2 shownPosition = Vector2.zero;

    [Header("Button Animation Settings")]
    [SerializeField] private bool useButtonAnimation = false;
    [SerializeField] private float rotationDuration = 0.5f;
    [SerializeField] private float rotationAngle = 360f;
    [SerializeField] private Ease rotationEase = Ease.OutCirc;

    [Header("Events")]
    [SerializeField] private UnityEvent OnShowSettings;
    [SerializeField] private UnityEvent OnHideSettings;

    private bool _isVisible = false;
    private GraphicRaycaster _raycaster;
    private EventSystem _eventSystem;
    private ISettingsButtonAnimator _buttonAnimator;

    private void Start()
    {
        _settingsPanel.anchoredPosition = hiddenPosition;
        _settingsPanel.gameObject.SetActive(false);

        _raycaster = GetComponent<GraphicRaycaster>();
        _eventSystem = EventSystem.current;

        SetupButtonAnimator();
    }

    private void SetupButtonAnimator()
    {
        if (useButtonAnimation && _settingsButton != null)
        {
            _buttonAnimator = new RotatingButtonAnimator(_settingsButton.transform, rotationDuration, rotationAngle, rotationEase);
        }
        else
        {
            _buttonAnimator = new NoAnimationButtonAnimator();
        }
    }

    private void Update()
    {
        if (_isVisible && Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUIElement(_settingsPanel))
            {
                HideSettings();
            }
        }
    }

    public void ToggleSettings()
    {
        if (_isVisible)
            HideSettings();
        else
            ShowSettings();

        _buttonAnimator.AnimateButtonClick();
    }

    public void ShowSettings()
    {
        _isVisible = true;
        AnimatePanel(shownPosition, showEase);
        AnimateOverlay(true);
        UIManager.Instance.RegisterOverlay(true);
        OnShowSettings.Invoke();
    }

    public void HideSettings()
    {
        _isVisible = false;
        AnimatePanel(hiddenPosition, hideEase);
        AnimateOverlay(false);
        UIManager.Instance.RegisterOverlay(false);
        OnHideSettings.Invoke();
    }
    
    private void AnimateOverlay(bool show)
    {
        _overlay.DOKill();
        
        _overlay.gameObject.SetActive(true);
        
        var targetColor = show ? Color.black : Color.clear;
        var targetAlpha = show ? 0.18f : 0;
        
        _overlay.DOColor(new Color(targetColor.r, targetColor.g, targetColor.b, targetAlpha), _animationDuration)
            .SetEase(show ? showEase : hideEase)
            .OnComplete(() =>
            {
                if (!show)
                    _overlay.gameObject.SetActive(false);
            });
    }

    private void AnimatePanel(Vector2 targetPosition, Ease easeType)
    {
        _settingsPanel.DOKill();

        _settingsPanel.DOAnchorPos(targetPosition, _animationDuration)
            .SetEase(easeType)
            .OnStart(() => _settingsPanel.gameObject.SetActive(true))
            .OnComplete(() => 
            {
                if (!_isVisible)
                    _settingsPanel.gameObject.SetActive(false);
            });

        var canvasGroup = _settingsPanel.GetComponent<CanvasGroup>();
        
        if (canvasGroup == null)
            canvasGroup = _settingsPanel.gameObject.AddComponent<CanvasGroup>();

        canvasGroup.DOFade(_isVisible ? 1 : 0, _animationDuration);

        _settingsPanel.DOScale(_isVisible ? Vector3.one : Vector3.one * 0.8f, _animationDuration)
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

    private void OnDestroy()
    {
        _settingsPanel.DOKill();
        if (_settingsButton != null)
        {
            _settingsButton.onClick.RemoveListener(ToggleSettings);
        }
    }
}