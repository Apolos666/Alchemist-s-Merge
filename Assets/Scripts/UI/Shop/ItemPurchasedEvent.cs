public class ItemPurchasedEvent : IEvent
{
    public GameItem PurchasedItem { get; private set; }

    public ItemPurchasedEvent(GameItem item)
    {
        PurchasedItem = item;
    }
}