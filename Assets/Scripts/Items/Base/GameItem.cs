using UnityEngine;

public abstract class GameItem : MonoBehaviour
{
    [SerializeField] protected GameItemData itemData;
    public int Quantity { get; private set; }
    
    private float _currentCooldownTime;

    public string ItemName => itemData.ItemName;
    public int Price => itemData.Price;
    private string ItemId => itemData.ItemId;

    protected virtual void Update()
    {
        if (_currentCooldownTime > 0) 
            _currentCooldownTime = Mathf.Max(0, _currentCooldownTime - Time.deltaTime);
    }
    
    public float GetCooldownProgress() => _currentCooldownTime / itemData.CooldownTime;

    protected virtual bool CanUse() => _currentCooldownTime <= 0 && Quantity > 0;

    public virtual void Use()
    {
        if (Quantity <= 0)
        {
            ItemPurchaser.Instance.TryPurchaseItem(this);
        }   
    }

    protected virtual void OnItemUsed()
    {
        Quantity--;
        _currentCooldownTime = itemData.CooldownTime;
    }

    public virtual void AddQuantity(int amount)
    {
        Quantity += amount;
        SaveQuantity();
    }

    protected virtual void SaveQuantity()
    {
        PlayerPrefs.SetInt(ItemId + "_quantity", Quantity);
        PlayerPrefs.Save();
    }

    protected virtual void LoadQuantity()
    {
        Quantity = PlayerPrefs.GetInt(ItemId + "_quantity", itemData.InitialQuantity);
    }

    protected virtual void Awake()
    {
        LoadQuantity();
    }
}