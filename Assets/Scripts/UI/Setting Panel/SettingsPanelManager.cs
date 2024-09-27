using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsPanelManager : MonoBehaviour
{
    [SerializeField] private RectTransform _settingsPanel;
    [SerializeField] private Button _settingsButton;

    [Header("Animation Settings")] [SerializeField]
    private float _animationDuration = 0.5f;

    [SerializeField] private Ease showEase = Ease.OutBack;
    [SerializeField] private Ease hideEase = Ease.InBack;
    [SerializeField] private Vector2 hiddenPosition = new Vector2(300, 0);
    [SerializeField] private Vector2 shownPosition = Vector2.zero;
    
    [Header("Button Rotation Settings")]
    [SerializeField] private float rotationDuration = 0.5f;
    [SerializeField] private float rotationAngle = 360f;
    [SerializeField] private Ease rotationEase = Ease.OutCirc;

    private bool _isVisible = false;
    private GraphicRaycaster _raycaster;
    private EventSystem _eventSystem;

    private void Start()
    {
        _settingsPanel.anchoredPosition = hiddenPosition;
        _settingsPanel.gameObject.SetActive(false);

        _settingsButton.onClick.AddListener(ToggleSettings);
        _raycaster = GetComponent<GraphicRaycaster>();
        _eventSystem = EventSystem.current;
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

    private void ToggleSettings()
    {
        if (_isVisible)
            HideSettings();
        else
            ShowSettings();
        
        RotateSettingsButton();
    }

    private void RotateSettingsButton()
    {
        _settingsButton.transform.DOKill();
        
        _settingsButton.transform.DORotate(new Vector3(0, 0, rotationAngle), rotationDuration, RotateMode.FastBeyond360)
            .SetEase(rotationEase)
            .SetRelative()
            .OnComplete(() => _settingsButton.transform.DORotate(Vector3.zero, rotationDuration, RotateMode.FastBeyond360));
    }

    private void ShowSettings()
    {
        _isVisible = true;
        AnimatePanel(shownPosition, showEase);
    }

    private void HideSettings()
    {
        _isVisible = false;
        AnimatePanel(hiddenPosition, hideEase);
    }

    private void AnimatePanel(Vector2 targetPosition, Ease easeType)
    {
        // Kill any ongoing tweens to prevent conflicts
        _settingsPanel.DOKill();

        // Animate the panel's position
        _settingsPanel.DOAnchorPos(targetPosition, _animationDuration)
            .SetEase(easeType)
            .OnStart(() => _settingsPanel.gameObject.SetActive(true))
            .OnComplete(() => 
            {
                if (!_isVisible)
                    _settingsPanel.gameObject.SetActive(false);
            });

        // Add a fade effect
        var canvasGroup = _settingsPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = _settingsPanel.gameObject.AddComponent<CanvasGroup>();

        canvasGroup.DOFade(_isVisible ? 1 : 0, _animationDuration);

        // Add a scale effect
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
        // Clean up listener to prevent memory leaks
        _settingsButton.onClick.RemoveListener(ToggleSettings);

        // Kill all tweens associated with the settings panel
        _settingsPanel.DOKill();
        _settingsButton.transform.DOKill();
    }
}