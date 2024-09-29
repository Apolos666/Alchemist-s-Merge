using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MusicPlayerUI : MonoBehaviour
{
    [SerializeField] private Button _playPauseButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _prevButton;
    [SerializeField] private Slider _backgroundVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private TextMeshProUGUI _trackNameText;
    [SerializeField] private Image _playPauseIcon;
    [SerializeField] private Sprite _playSprite;
    [SerializeField] private Sprite _pauseSprite;

    private void Start()
    {
        SetupButtonListeners();
        SetupEventSubscriptions();
        UpdateTrackName(BackgroundMusicManager.Instance.GetCurrentTrackName());
    }

    private void SetupButtonListeners()
    {
        _playPauseButton.onClick.AddListener(() =>
        {
            AnimateButtonClick(_playPauseButton.transform);
            BackgroundMusicManager.Instance.TogglePlayPause();
        });

        _nextButton.onClick.AddListener(() =>
        {
            AnimateButtonClick(_nextButton.transform);
            BackgroundMusicManager.Instance.PlayNextTrack();
        });

        _prevButton.onClick.AddListener(() =>
        {
            AnimateButtonClick(_prevButton.transform);
            BackgroundMusicManager.Instance.PlayPreviousTrack();
        });

        _backgroundVolumeSlider.onValueChanged.AddListener(value =>
        {
            BackgroundMusicManager.Instance.SetVolume(value);
        });
        
        _sfxVolumeSlider.onValueChanged.AddListener(value =>
        {
            SoundEffectManager.Instance.SetVolume(value);
        });
    }

    private void SetupEventSubscriptions()
    {
        EventBus.Subscribe<TrackChangedEvent>(OnTrackChanged);
        EventBus.Subscribe<PlayPauseChangedEvent>(OnPlayPauseChanged);
        EventBus.Subscribe<VolumeChangedEvent>(OnVolumeChanged);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<TrackChangedEvent>(OnTrackChanged);
        EventBus.Unsubscribe<PlayPauseChangedEvent>(OnPlayPauseChanged);
        EventBus.Unsubscribe<VolumeChangedEvent>(OnVolumeChanged);
    }

    private void OnTrackChanged(TrackChangedEvent message)
    {
        UpdateTrackName(message.TrackName);
    }

    private void OnPlayPauseChanged(PlayPauseChangedEvent message)
    {
        _playPauseIcon.sprite = message.IsPlaying ? _pauseSprite : _playSprite;
        AnimatePlayPauseIcon();
    }

    private void OnVolumeChanged(VolumeChangedEvent message)
    {
        _backgroundVolumeSlider.value = message.Volume;
    }

    private void UpdateTrackName(string trackName)
    {
        _trackNameText.text = $"Current Music: {trackName}";
        AnimateTrackNameChange();
    }

    private static void AnimateButtonClick(Transform buttonTransform)
    {
        buttonTransform
            .DOScale(0.9f, 0.1f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                buttonTransform
                    .DOScale(1f, 0.1f)
                    .SetEase(Ease.OutQuad);
            });
    }

    private void AnimatePlayPauseIcon()
    {
        _playPauseIcon.transform
            .DOScale(1.2f, 0.2f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                _playPauseIcon.transform
                    .DOScale(1f, 0.1f)
                    .SetEase(Ease.OutQuad);
            });
    }

    private void AnimateTrackNameChange()
    {
        _trackNameText.transform
            .DOScale(1.1f, 0.2f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                _trackNameText.transform
                    .DOScale(1f, 0.2f)
                    .SetEase(Ease.InQuad);
            });
    }
}