using UnityEngine;

public class Potion : GameItem
{
    [SerializeField] private float _strength;
    public float Strength => _strength;
    [SerializeField] private float _endValue;
    public float EndValue => _endValue;
    [SerializeField] private float _duration;
    public float Duration => _duration;
    
    public override void Use()
    {
        if (CanUse())
        {
            OnItemUsed();
            EventBus.Publish(new PotionAppliedEvent());
        }
    }
}