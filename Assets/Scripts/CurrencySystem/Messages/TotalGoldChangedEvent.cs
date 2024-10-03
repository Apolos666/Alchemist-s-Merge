public class TotalGoldChangedEvent : IEvent
{
    public int TotalGold { get; }

    public TotalGoldChangedEvent(int totalGold)
    {
        TotalGold = totalGold;
    }
}