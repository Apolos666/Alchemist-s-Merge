using UnityEngine;

public class CurrencyManager : GenericPersistentSingleton<CurrencyManager>
{
    private const string GOLD_KEY = "CurrentGold";
    public int CurrentGold { get; private set; }
    
    private void Start()
    {
        LoadGold();
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
        SaveGold();
    }

    private void SaveGold()
    {
        PlayerPrefs.SetInt(GOLD_KEY, CurrentGold);
        PlayerPrefs.Save();
    }

    private void LoadGold()
    {
        var loadedGold = PlayerPrefs.GetInt(GOLD_KEY, 0);
        CurrentGold = loadedGold;
        EventBus.Publish(new TotalGoldChangedEvent(CurrentGold));
    }
    
    public bool TrySpendGold(int amount)
    {
        if (CurrentGold >= amount)
        {
            CurrentGold -= amount;
            EventBus.Publish(new TotalGoldChangedEvent(CurrentGold));
            SaveGold();
            return true;
        }
        return false;
    }
}