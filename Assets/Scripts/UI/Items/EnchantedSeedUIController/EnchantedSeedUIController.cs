using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnchantedSeedUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _quantityText;
    [SerializeField] private float _quantityTextAnimationDuration = 0.5f;
    [SerializeField] private Image _cooldownImage;
    [SerializeField] private Image _usageEffectImage;
    [SerializeField] private Button _enchantedSeedButton;
    
    private EnchantedSeed _enchantedSeed;
    
    private void Start()
    {
        _enchantedSeedButton.onClick.AddListener(() => _enchantedSeed.Use());
        _enchantedSeed = GetComponent<EnchantedSeed>();
        InitializeUI();
        EventBus.Subscribe<EnchantedSeedAppliedEvent>(OnEnchantedSeedUsed);
        _usageEffectImage.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<EnchantedSeedAppliedEvent>(OnEnchantedSeedUsed);
    }

    private void InitializeUI()
    {
        _quantityText.text = "0";
        DOTween
            .To(() => 0, x => _quantityText.text = x.ToString(), _enchantedSeed.Quantity,
                _quantityTextAnimationDuration)
            .SetEase(Ease.OutQuad);
    }
    
    private void Update()
    {
        _cooldownImage.fillAmount = _enchantedSeed.GetCooldownProgress();
    }

    private void UpdateUI()
    {
        _quantityText.text = _enchantedSeed.Quantity.ToString();
    }

    private void OnEnchantedSeedUsed(EnchantedSeedAppliedEvent obj)
    {
        UpdateUI();
        UIManager.Instance.RegisterOverlay(true);
        AnimatedUsageEffect();
    }
    
    private void AnimatedUsageEffect()
    {
        // Khởi tạo hình ảnh hiệu ứng sử dụng
        _usageEffectImage.gameObject.SetActive(true);
        _usageEffectImage.transform.localScale = Vector3.zero;
        _usageEffectImage.color = new Color(1f, 1f, 1f, 1f);
    
        var sequence = DOTween.Sequence();
    
        // Scale up
        sequence
            .Append(_usageEffectImage.transform
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

        sequence.OnComplete(() =>
        {
            _usageEffectImage.gameObject.SetActive(false);
            UIManager.Instance.RegisterOverlay(false);
        });
    }
}