using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UniversalSeedUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _quantityText;
    [SerializeField] private float _quantityTextAnimationDuration = 0.5f;
    [SerializeField] private Image _cooldownImage;
    [SerializeField] private GameObject _parentEffectImage;
    [SerializeField] private Image _usageEffectImage;
    [SerializeField] private Image _overlayImage;
    [SerializeField] private Button _shakePotionButton;
    
    private UniversalSeed _universalSeed;
    
    private void Start()
    {
        _shakePotionButton.onClick.AddListener(() => _universalSeed.Use());
        _universalSeed = GetComponent<UniversalSeed>();
        InitializeUI();
        EventBus.Subscribe<UniversalSeedAppliedEvent>(OnUniversalSeedUsed);
        _parentEffectImage.SetActive(false);
    }
    
    private void OnDestroy()
    {
        EventBus.Unsubscribe<UniversalSeedAppliedEvent>(OnUniversalSeedUsed);
    }

    private void OnUniversalSeedUsed(UniversalSeedAppliedEvent message)
    {
        UpdateUI();
        SoundEffectManager.Instance.PlaySound("Universal Prop", Vector3.zero);
        UIManager.Instance.RegisterOverlay(true);
        AnimatedUsageEffect();
    }

    private void InitializeUI()
    {
        _quantityText.text = "0";
        DOTween
            .To(() => 0, x => _quantityText.text = x.ToString(), _universalSeed.Quantity,
                _quantityTextAnimationDuration)
            .SetEase(Ease.OutQuad);
    }
    
    private void Update()
    {
        _cooldownImage.fillAmount = _universalSeed.GetCooldownProgress();
    }

    private void UpdateUI()
    {
        _quantityText.text = _universalSeed.Quantity.ToString();
    }
    
    private void AnimatedUsageEffect()
    {
        // Khởi tạo hình ảnh hiệu ứng sử dụng
        _parentEffectImage.SetActive(true);
        _usageEffectImage.transform.localScale = Vector3.zero;
        _usageEffectImage.color = new Color(1f, 1f, 1f, 1f);
        var overlayColor = _overlayImage.color;
        overlayColor.a = 0f;
        _overlayImage.color = overlayColor;
    
        var sequence = DOTween.Sequence();
    
        // Fade in overlay and scale up
        sequence.Append(_overlayImage
            .DOFade(0.25f, 0.3f)
            .SetEase(Ease.InOutQuad));
        sequence.Join(_usageEffectImage.transform
            .DOScale(Vector3.one, 0.3f)
            .SetEase(Ease.OutBack));

        // Shake
        sequence.Append(_usageEffectImage.transform
            .DOShakeScale(0.5f, 0.1f, 10, 90));

        // Hold for a moment
        sequence.AppendInterval(0.2f);

        // Fade out and scale down
        sequence.Append(_usageEffectImage
            .DOFade(0, 0.5f));
        sequence.Join(_usageEffectImage.transform
            .DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.InBack));
        
        sequence.Append(_overlayImage
            .DOFade(0, 0.5f)
            .SetEase(Ease.InOutQuad));

        sequence.OnComplete(() =>
        {
            _parentEffectImage.SetActive(false);
            UIManager.Instance.RegisterOverlay(false);
        });
    }
}