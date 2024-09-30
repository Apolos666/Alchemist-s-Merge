using UnityEngine;

public class PropUnlockedEvent : IEvent
{
    public int UnlockLevel { get; }
    public Prop UnlockedProp { get; }
    
    public PropUnlockedEvent(int unlockLevel, Prop unlockedProp)
    {
        UnlockLevel = unlockLevel;
        UnlockedProp = unlockedProp;
    }
}