using UnityEngine;

public class PropUnlockedEvent : IEvent
{
    public int UnlockLevel { get; }
    public Sprite UnlockedPropIcon { get; } 
    public PropUnlockedEvent(int unlockLevel, Sprite unlockedPropIcon)
    {
        UnlockLevel = unlockLevel;
        UnlockedPropIcon = unlockedPropIcon;
    }
}