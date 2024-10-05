using UnityEngine;

public class ItemPurchaser : GenericPersistentSingleton<ItemPurchaser>
{
    public bool TryPurchaseItem(GameItem item)
    {
        if (CurrencyManager.Instance.TrySpendGold(item.Price))
        {
            item.AddQuantity(1);
            SoundEffectManager.Instance.PlaySound("Buying", Vector3.zero);
            EventBus.Publish(new ItemPurchasedEvent(item));
            return true;
        }
        else
        {
            SoundEffectManager.Instance.PlaySound("Not Enough Money", Vector3.zero);
        }
        return false;
    }
}