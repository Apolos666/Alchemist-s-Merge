public class UniversalSeedAppliedEvent : IEvent
{
    public UniversalSeedProp UniversalSeedProp { get; }
    
    public UniversalSeedAppliedEvent(UniversalSeedProp universalSeedProp)
    {
        UniversalSeedProp = universalSeedProp;
    }
}