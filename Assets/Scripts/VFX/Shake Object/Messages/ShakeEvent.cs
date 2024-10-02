public class ShakeEvent : IEvent
{
    public float EndValue { get; }
    public float Strength { get; }
    public float Duration { get; }

    public ShakeEvent(float endValue, float strength, float duration)
    {
        EndValue = endValue;
        Strength = strength;
        Duration = duration;
    }
}