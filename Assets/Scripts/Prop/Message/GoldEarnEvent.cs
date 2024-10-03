using UnityEngine;

public class GoldEarnEvent : IEvent
{
    public int Amount { get; }
    public Vector3 Position { get; }
    
    public GoldEarnEvent(int amount, Vector3 position)
    {
        Amount = amount;
        Position = position;
    }
}