using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopUIController : MonoBehaviour
{
    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _quantityText;
    [SerializeField] private GameItem _item;

    private void Start()
    {
        _buyButton.onClick.AddListener(TryBuyItem);
        _itemNameText.text = _item.ItemName;
        UpdateUI();
        EventBus.Subscribe<TotalGoldChangedEvent>(OnTotalGoldChanged);
        EventBus.Subscribe<ItemPurchasedEvent>(OnItemPurchased);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<TotalGoldChangedEvent>(OnTotalGoldChanged);
        EventBus.Unsubscribe<ItemPurchasedEvent>(OnItemPurchased);
    }

    private void TryBuyItem()
    {
        if (ItemPurchaser.TryPurchaseItem(_item))
        {
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough gold to buy item");
        }
    }

    private void UpdateUI()
    {
        _priceText.text = _item.Price.ToString();
        _quantityText.text = $"Owned: {_item.Quantity}";    
        _buyButton.interactable = CurrencyManager.Instance.CurrentGold >= _item.Price;
    }

    private void OnTotalGoldChanged(TotalGoldChangedEvent evt)
    {
        UpdateUI();
    }

    private void OnItemPurchased(ItemPurchasedEvent message)
    {
        if (message.PurchasedItem == _item)
        {
            UpdateUI();
        }
    }
}