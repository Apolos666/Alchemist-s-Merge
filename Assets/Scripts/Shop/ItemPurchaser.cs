public class ItemPurchaser : GenericPersistentSingleton<ItemPurchaser>
{
    public static bool TryPurchaseItem(GameItem item)
    {
        if (CurrencyManager.Instance.TrySpendGold(item.Price))
        {
            item.AddQuantity(1);
            EventBus.Publish(new ItemPurchasedEvent(item));
            return true;
        }
        return false;
    }
}