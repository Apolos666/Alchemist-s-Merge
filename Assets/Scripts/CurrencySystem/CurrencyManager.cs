
public class CurrencyManager : GenericSingleton<CurrencyManager>
{
    public int CurrentGold { get; private set; }
    
    private void Start()
    {
        EventBus.Subscribe<GoldEarnEvent>(OnGoldEarned);
    }
    
    private void OnDestroy()
    {
        EventBus.Unsubscribe<GoldEarnEvent>(OnGoldEarned);
    }

    private void OnGoldEarned(GoldEarnEvent message)
    {
        CurrentGold += message.Amount;
        EventBus.Publish(new TotalGoldChangedEvent(CurrentGold));
    }
}